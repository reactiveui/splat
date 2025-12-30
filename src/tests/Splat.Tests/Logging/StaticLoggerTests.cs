// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Unit Tests for the Static Logging wrapper implementation.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class StaticLoggerTests
{
    private static char[] NewLine => Environment.NewLine.ToCharArray();

    /// <summary>
    /// Base tests for the static logger.
    /// </summary>
    public abstract class BaseStaticLoggerTests
    {
        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger { Level = GetLogLevel() };
            var logger = GetLogger(textLogger);

            GetMethodWithGenericTypeShouldWriteMessageAndType<DummyObjectClass1>()(logger, "This is a test.");

            await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"This is a test. ({nameof(GetMethodWithGenericTypeShouldWriteMessageAndType)})");
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger { Level = GetLogLevel() };
            var logger = GetLogger(textLogger);

            GetMethodWithGenericTypeShouldWriteMessageAndType<DummyObjectClass2>()(logger, "This is a test.");

            await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"This is a test. ({nameof(GetMethodWithGenericTypeShouldWriteMessageAndType)})");
        }

        /// <summary>
        /// Test to ensure debug writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_Writes_Message() =>
            await Test_Write_Message(logger => GetMethodToWriteMessage()(logger, "Message"));

        /// <summary>
        /// Test to ensure invariant culture formatted string writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_Writes_Message_InvariantCulture() =>
            await Test_Write_Message(logger => GetMethodToWriteMessageWithInvariantCulture()(logger, CultureInfo.InvariantCulture, "{0}", "Message"));

        /// <summary>
        /// Test to ensure invariant culture formatted string writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_Writes_Message_InvariantCulture_With_Generic_Two_Args() =>
            await Test_Write_Message(logger => GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs()(logger, CultureInfo.InvariantCulture, "{0}", "Message", 1));

        /// <summary>
        /// Test to ensure invariant culture formatted string writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_Writes_Message_InvariantCulture_With_Generic_Three_Args() =>
            await Test_Write_Message(logger => GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs()(logger, CultureInfo.InvariantCulture, "{0}", "Message", 1, 2.3f));

        /// <summary>
        /// Test to ensure debug writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Method_Writes_Exception_And_Message()
        {
            var staticLogger = GetLogger(GetLogLevel());
            GetMethodToWriteExceptionAndMessage()(staticLogger, new ArgumentException("TEST"), "Message");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Test to ensure writes method works.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Direct_Write_At_Level_Message()
        {
            var staticLogger = GetLogger(GetLogLevel());
            staticLogger.Write("Message", GetLogLevel());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Test to ensure writes method works.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Direct_Write_At_Level_Message_And_Type()
        {
            var staticLogger = GetLogger(GetLogLevel());
            staticLogger.Write("Message", typeof(DummyObjectClass1), GetLogLevel());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Test to ensure writes method works.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Direct_Write_At_Level_Message_And_Exception()
        {
            var staticLogger = GetLogger(GetLogLevel());
            var exception = new InvalidOperationException("bleh");
            staticLogger.Write(exception, "Message", GetLogLevel());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Test to ensure writes method works.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Direct_Write_At_Level_Message_Exception_And_Type()
        {
            var staticLogger = GetLogger(GetLogLevel());
            var exception = new InvalidOperationException("bleh");
            staticLogger.Write(exception, "Message", typeof(DummyObjectClass1), GetLogLevel());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Logger_Level_AtBoundary_Should_Be_correct()
        {
            var level = GetLogLevel();
            var logger = GetLogger(level);

            await Assert.That(logger.Level).IsEqualTo(level);
        }

        /// <summary>
        /// Gets the logger action for Write with a generic type.
        /// </summary>
        /// <typeparam name="T">The generic type to log.</typeparam>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>();

        /// <summary>
        /// Gets the logger action for Writing a string message.
        /// </summary>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, string> GetMethodToWriteMessage();

        /// <summary>
        /// Gets the logger action for Writing a string message.
        /// </summary>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture();

        /// <summary>
        /// Gets the logger action for Writing a string message.
        /// </summary>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs();

        /// <summary>
        /// Gets the logger action for Writing a string message.
        /// </summary>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs();

        /// <summary>
        /// Gets the logger action for Writing an exception and string message.
        /// </summary>
        /// <returns>Action to call.</returns>
        protected abstract Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage();

        /// <summary>
        /// Gets the log level boundary for the test.
        /// </summary>
        /// <returns>Log Level.</returns>
        protected abstract LogLevel GetLogLevel();

        private static StaticFullLogger GetLogger(TextLogger textLogger) => new(new WrappingFullLogger(textLogger));

        private static StaticFullLogger GetLogger(LogLevel logLevel)
        {
            var textLogger = new TextLogger
            {
                Level = logLevel
            };

            var wrappingFullLogger = new WrappingFullLogger(textLogger);
            return new(wrappingFullLogger);
        }

        private static async Task Test_Write_Message(Action<StaticFullLogger> testMethodFunc)
        {
            var textLogger = new TextLogger();
            var staticLogger = GetLogger(textLogger);

            using (Assert.Multiple())
            {
                await Assert.That(textLogger.Logs).Count().IsEqualTo(0);
            }

            testMethodFunc(staticLogger);

            using (Assert.Multiple())
            {
                await Assert.That(textLogger.Logs).Count().IsEqualTo(1);

                var line = textLogger.Logs.First();

#if NET8_0_OR_GREATER
                var startOfCallerMemberSuffix = line.message.IndexOf('(', StringComparison.Ordinal);
#else
                var startOfCallerMemberSuffix = line.message.IndexOf('(');
#endif
                await Assert.That(startOfCallerMemberSuffix).IsGreaterThan(0);

                var endOfCallerMemberSuffix = line.message.IndexOf(')', startOfCallerMemberSuffix);
                await Assert.That(endOfCallerMemberSuffix).IsGreaterThan(startOfCallerMemberSuffix + 1);
            }
        }
    }

    [InheritsTests]
    public sealed class DebugStaticLoggerTests : BaseStaticLoggerTests
    {
        /// <inheritdoc/>
        protected override LogLevel GetLogLevel() => LogLevel.Debug;

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>() => (logger, message) => logger.Debug<T>(message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodToWriteMessage() => (logger, s) => logger.Debug(s);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture() => (logger, formatProvider, message, arg1) => logger.Debug(formatProvider, message, arg1);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage() => (logger, exception, message) => logger.Debug(exception, message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs() => (logger, formatProvider, message, arg1, arg2) => logger.Debug(formatProvider, message, arg1, arg2);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs() => (logger, formatProvider, message, arg1, arg2, arg3) => logger.Debug(formatProvider, message, arg1, arg2, arg3);
    }

    [InheritsTests]
    public sealed class InfoStaticLoggerTests : BaseStaticLoggerTests
    {
        /// <inheritdoc/>
        protected override LogLevel GetLogLevel() => LogLevel.Info;

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>() => (logger, message) => logger.Info<T>(message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodToWriteMessage() => (logger, s) => logger.Info(s);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture() => (logger, formatProvider, message, arg1) => logger.Info(formatProvider, message, arg1);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage() => (logger, exception, message) => logger.Info(exception, message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs() => (logger, formatProvider, message, arg1, arg2) => logger.Info(formatProvider, message, arg1, arg2);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs() => (logger, formatProvider, message, arg1, arg2, arg3) => logger.Info(formatProvider, message, arg1, arg2, arg3);
    }

    [InheritsTests]
    public sealed class WarningStaticLoggerTests : BaseStaticLoggerTests
    {
        /// <inheritdoc/>
        protected override LogLevel GetLogLevel() => LogLevel.Warn;

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>() => (logger, message) => logger.Warn<T>(message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodToWriteMessage() => (logger, s) => logger.Warn(s);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture() => (logger, formatProvider, message, arg1) => logger.Warn(formatProvider, message, arg1);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage() => (logger, exception, message) => logger.Warn(exception, message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs() => (logger, formatProvider, message, arg1, arg2) => logger.Warn(formatProvider, message, arg1, arg2);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs() => (logger, formatProvider, message, arg1, arg2, arg3) => logger.Warn(formatProvider, message, arg1, arg2, arg3);
    }

    [InheritsTests]
    public sealed class ErrorStaticLoggerTests : BaseStaticLoggerTests
    {
        /// <inheritdoc/>
        protected override LogLevel GetLogLevel() => LogLevel.Error;

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>() => (logger, message) => logger.Error<T>(message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodToWriteMessage() => (logger, s) => logger.Error(s);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture() => (logger, formatProvider, message, arg1) => logger.Error(formatProvider, message, arg1);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage() => (logger, exception, message) => logger.Error(exception, message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs() => (logger, formatProvider, message, arg1, arg2) => logger.Error(formatProvider, message, arg1, arg2);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs() => (logger, formatProvider, message, arg1, arg2, arg3) => logger.Error(formatProvider, message, arg1, arg2, arg3);
    }

    [InheritsTests]
    public sealed class FatalStaticLoggerTests : BaseStaticLoggerTests
    {
        /// <inheritdoc/>
        protected override LogLevel GetLogLevel() => LogLevel.Fatal;

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodWithGenericTypeShouldWriteMessageAndType<T>() => (logger, message) => logger.Fatal<T>(message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, string> GetMethodToWriteMessage() => (logger, s) => logger.Fatal(s);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string> GetMethodToWriteMessageWithInvariantCulture() => (logger, formatProvider, message, arg1) => logger.Fatal(formatProvider, message, arg1);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, Exception, string> GetMethodToWriteExceptionAndMessage() => (logger, exception, message) => logger.Fatal(exception, message);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int> GetMethodToWriteMessageWithInvariantCultureWithTwoGenericArgs() => (logger, formatProvider, message, arg1, arg2) => logger.Fatal(formatProvider, message, arg1, arg2);

        /// <inheritdoc/>
        protected override Action<StaticFullLogger, CultureInfo, string, string, int, float> GetMethodToWriteMessageWithInvariantCultureWithThreeGenericArgs() => (logger, formatProvider, message, arg1, arg2, arg3) => logger.Fatal(formatProvider, message, arg1, arg2, arg3);
    }
}
