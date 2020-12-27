using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Unit Tests for the Static Logging wrapper implementation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class StaticLoggerTests
    {
        private const string Message = "Message";

        private static char[] NewLine => Environment.NewLine.ToCharArray();

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger { Level = LogLevel.Debug };
            var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

            logger.Debug<DummyObjectClass1>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger { Level = LogLevel.Debug };
            var logger = GetLogger(textLogger);

            logger.Debug<DummyObjectClass2>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger { Level = LogLevel.Debug };
            var logger = GetLogger(textLogger);

            logger.Info<DummyObjectClass1>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger { Level = LogLevel.Debug };
            var logger = GetLogger(textLogger);

            logger.Info<DummyObjectClass2>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger { Level = LogLevel.Debug };
            var logger = GetLogger(textLogger);

            logger.Warn<DummyObjectClass1>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = GetLogger(textLogger);

            logger.Warn<DummyObjectClass2>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = GetLogger(textLogger);

            logger.Error<DummyObjectClass1>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = GetLogger(textLogger);

            logger.Error<DummyObjectClass2>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = GetLogger(textLogger);

            logger.Fatal<DummyObjectClass1>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = GetLogger(textLogger);

            logger.Fatal<DummyObjectClass2>("This is a test.");

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
        }

        /// <summary>
        /// Test to ensure debug writes.
        /// </summary>
        [Fact]
        public void Debug_Writes_Message()
        {
            Test_Write_Message(logger => logger.Debug(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Debug_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Debug(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure info writes.
        /// </summary>
        [Fact]
        public void Info_Writes_Message()
        {
            Test_Write_Message(logger => logger.Info(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Info_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Info(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure warn writes.
        /// </summary>
        [Fact]
        public void Warn_Writes_Message()
        {
            Test_Write_Message(logger => logger.Warn(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Warn_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Warn(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure error writes.
        /// </summary>
        [Fact]
        public void Error_Writes_Message()
        {
            Test_Write_Message(logger => logger.Error(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Error_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Error(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Fatal_Writes_Message()
        {
            Test_Write_Message(logger => logger.Fatal(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Fatal_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Fatal(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Logger_Level_Debug_Should_Be_correct()
        {
            var logger = GetLogger(LogLevel.Debug);

            Assert.Equal(LogLevel.Debug, logger.Level);
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Logger_Level_Info_Should_Be_correct()
        {
            var logger = GetLogger(LogLevel.Info);

            Assert.Equal(LogLevel.Info, logger.Level);
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Logger_Level_Warn_Should_Be_correct()
        {
            var logger = GetLogger(LogLevel.Warn);

            Assert.Equal(LogLevel.Warn, logger.Level);
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Logger_Level_Error_Should_Be_correct()
        {
            var logger = GetLogger(LogLevel.Error);

            Assert.Equal(LogLevel.Error, logger.Level);
        }

        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Logger_Level_Fatal_Should_Be_correct()
        {
            var logger = GetLogger(LogLevel.Fatal);

            Assert.Equal(LogLevel.Fatal, logger.Level);
        }

        private static void Test_Write_Message(Action<StaticFullLogger> testMethodFunc)
        {
            var textLogger = new TextLogger();
            Assert.Equal(0, textLogger.Logs.Count);
            var staticLogger = GetLogger(textLogger);

            testMethodFunc(staticLogger);
            Assert.Equal(1, textLogger.Logs.Count);
            var line = textLogger.Logs.First();
            Assert.True(line.message.Contains("()"));
        }

        private static StaticFullLogger GetLogger(TextLogger textLogger) => new StaticFullLogger(new WrappingFullLogger(textLogger));

        private static StaticFullLogger GetLogger(LogLevel logLevel)
        {
            var textLogger = new TextLogger();
            textLogger.Level = logLevel;

            var wrappingFullLogger = new WrappingFullLogger(textLogger);
            return new StaticFullLogger(wrappingFullLogger);
        }
    }
}
