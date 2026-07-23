// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using NLog;
using NLog.Config;
using NLog.Targets;

using Splat.NLog;

namespace Splat.Tests.Logging;

/// <summary>Tests that cover the <see cref="NLogLogger"/> Write overloads and level mapping.</summary>
[NotInParallel]
public class NLogLoggerCoverageTests
{
    /// <summary>A log-level value outside the defined range, used to verify out-of-range handling.</summary>
    private const int OutOfRangeLogLevel = 999;

    /// <summary>A plain message used to verify simple emission.</summary>
    private const string PlainMessage = "coverage plain message";

    /// <summary>The message carried by exceptions passed to the logger.</summary>
    private const string ExceptionMessage = "coverage boom";

    /// <summary>A single-placeholder format string.</summary>
    private const string OneArgumentFormat = "value {0}";

    /// <summary>A two-placeholder format string.</summary>
    private const string TwoArgumentFormat = "values {0} {1}";

    /// <summary>A three-placeholder format string.</summary>
    private const string ThreeArgumentFormat = "values {0} {1} {2}";

    /// <summary>The first format argument.</summary>
    private const string FirstArgument = "alpha";

    /// <summary>The second format argument.</summary>
    private const string SecondArgument = "beta";

    /// <summary>The third format argument.</summary>
    private const string ThirdArgument = "gamma";

    /// <summary>The end-relative index of the second most recent captured log entry.</summary>
    private const int SecondNewestLog = 2;

    /// <summary>The end-relative index of the third most recent captured log entry.</summary>
    private const int ThirdNewestLog = 3;

    /// <summary>Mappings of Splat log levels to equivalent NLog log levels.</summary>
    private static readonly Dictionary<LogLevel, global::NLog.LogLevel> _splat2NLog = new()
    {
        { LogLevel.Debug, global::NLog.LogLevel.Debug },
        { LogLevel.Error, global::NLog.LogLevel.Error },
        { LogLevel.Warn, global::NLog.LogLevel.Warn },
        { LogLevel.Fatal, global::NLog.LogLevel.Fatal },
        { LogLevel.Info, global::NLog.LogLevel.Info },
    };

    /// <summary>Verifies the simple <see cref="NLogLogger.Write(string, LogLevel)"/> overload emits at each level.</summary>
    /// <param name="logLevel">The Splat log level under test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(LogLevel.Debug)]
    [Arguments(LogLevel.Info)]
    [Arguments(LogLevel.Warn)]
    [Arguments(LogLevel.Error)]
    [Arguments(LogLevel.Fatal)]
    public async Task Write_Message_LogLevel_Emits(LogLevel logLevel)
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("a message", logLevel);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(logLevel);
        await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo("a message");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(Exception, string, LogLevel)"/> overload emits message and exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write(new InvalidOperationException("boom"), "with exception", LogLevel.Error);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        await Assert.That(target.Logs[^1].message).Contains("with exception");
        await Assert.That(target.Logs[^1].message).Contains("boom");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(string, Type, LogLevel)"/> type-based overload emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_Type_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("typed message", typeof(NLogLoggerCoverageTests), LogLevel.Warn);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo("typed message");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(Exception, string, Type, LogLevel)"/> type-and-exception overload emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_Type_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write(new InvalidOperationException("kaboom"), "typed with exception", typeof(NLogLoggerCoverageTests), LogLevel.Fatal);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        await Assert.That(target.Logs[^1].message).Contains("typed with exception");
        await Assert.That(target.Logs[^1].message).Contains("kaboom");
    }

    /// <summary>Verifies that writing below the configured minimum level emits nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Below_Minimum_Level_Does_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Write("ignored", LogLevel.Debug);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Verifies that the cached <see cref="NLogLogger.Level"/> reflects the configured minimum level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Reflects_Configured_Minimum()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>Verifies the IsXxxEnabled flags follow the configured minimum level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsEnabled_Flags_Follow_Minimum_Level()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        using (Assert.Multiple())
        {
            await Assert.That(logger.IsDebugEnabled).IsFalse();
            await Assert.That(logger.IsInfoEnabled).IsFalse();
            await Assert.That(logger.IsWarnEnabled).IsTrue();
            await Assert.That(logger.IsErrorEnabled).IsTrue();
            await Assert.That(logger.IsFatalEnabled).IsTrue();
        }
    }

    /// <summary>Verifies that an unknown <see cref="LogLevel"/> value throws when mapped.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Unknown_LogLevel_Throws()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        await Assert.That(() => logger.Write("bad", (LogLevel)OutOfRangeLogLevel)).Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>Verifies that disposing the logger does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_Does_Not_Throw()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        await Assert.That(() => logger.Dispose()).ThrowsNothing();
    }

    /// <summary>Verifies that the constructor throws when given a null inner logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Null_Inner_Throws() =>
        await Assert.That(static () => new NLogLogger(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies the format-provider value overload emits at debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_FormatProvider_Value_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object value = PlainMessage;

        logger.Debug(CultureInfo.InvariantCulture, value);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(PlainMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the exception-and-message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.Debug(exception, PlainMessage);
        logger.Debug(exception, (string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Debug);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the exception-and-function overload emits when debug is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Debug(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the exception-and-function overload skips work when debug is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Debug(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the params-array message overloads emit at debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Message_Args_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object[] args = [FirstArgument, SecondArgument];

        // Invoke the invariant-culture params overload through a delegate so the call site does not require a format provider.
        Action<string, object[]> emit = logger.Debug;
        emit(TwoArgumentFormat, args);
        logger.Debug<NLogLoggerCoverageTests>(TwoArgumentFormat, args);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Debug);
            await Assert.That(target.Logs[^1].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^1].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the strongly-typed format-provider argument overloads emit at debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_FormatProvider_Arguments_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug(CultureInfo.InvariantCulture, OneArgumentFormat, FirstArgument);
        logger.Debug(CultureInfo.InvariantCulture, TwoArgumentFormat, FirstArgument, SecondArgument);
        logger.Debug(CultureInfo.InvariantCulture, ThreeArgumentFormat, FirstArgument, SecondArgument, ThirdArgument);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^ThirdNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].message).Contains(ThirdArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the obsolete debug-exception message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.DebugException(PlainMessage, exception);
        logger.DebugException((string?)null, exception);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the obsolete debug-exception function overload emits when debug is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.DebugException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the obsolete debug-exception function overload skips work when debug is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.DebugException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the format-provider value overload emits at info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_FormatProvider_Value_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object value = PlainMessage;

        logger.Info(CultureInfo.InvariantCulture, value);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(PlainMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the exception-and-message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.Info(exception, PlainMessage);
        logger.Info(exception, (string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Info);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the exception-and-function overload emits when info is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Info(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the exception-and-function overload skips work when info is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Info(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the params-array message overloads emit at info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Message_Args_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object[] args = [FirstArgument, SecondArgument];

        // Invoke the invariant-culture params overload through a delegate so the call site does not require a format provider.
        Action<string, object[]> emit = logger.Info;
        emit(TwoArgumentFormat, args);
        logger.Info<NLogLoggerCoverageTests>(TwoArgumentFormat, args);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Info);
            await Assert.That(target.Logs[^1].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^1].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the strongly-typed format-provider argument overloads emit at info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_FormatProvider_Arguments_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info(CultureInfo.InvariantCulture, OneArgumentFormat, FirstArgument);
        logger.Info(CultureInfo.InvariantCulture, TwoArgumentFormat, FirstArgument, SecondArgument);
        logger.Info(CultureInfo.InvariantCulture, ThreeArgumentFormat, FirstArgument, SecondArgument, ThirdArgument);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^ThirdNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].message).Contains(ThirdArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the obsolete info-exception message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.InfoException(PlainMessage, exception);
        logger.InfoException((string?)null, exception);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the obsolete info-exception function overload emits when info is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.InfoException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the obsolete info-exception function overload skips work when info is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.InfoException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the format-provider value overload emits at warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_FormatProvider_Value_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object value = PlainMessage;

        logger.Warn(CultureInfo.InvariantCulture, value);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(PlainMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the exception-and-message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.Warn(exception, PlainMessage);
        logger.Warn(exception, (string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Warn);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the exception-and-function overload emits when warn is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Warn(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the exception-and-function overload skips work when warn is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Warn(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the params-array message overloads emit at warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Message_Args_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object[] args = [FirstArgument, SecondArgument];

        // Invoke the invariant-culture params overload through a delegate so the call site does not require a format provider.
        Action<string, object[]> emit = logger.Warn;
        emit(TwoArgumentFormat, args);
        logger.Warn<NLogLoggerCoverageTests>(TwoArgumentFormat, args);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Warn);
            await Assert.That(target.Logs[^1].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^1].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the strongly-typed format-provider argument overloads emit at warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_FormatProvider_Arguments_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn(CultureInfo.InvariantCulture, OneArgumentFormat, FirstArgument);
        logger.Warn(CultureInfo.InvariantCulture, TwoArgumentFormat, FirstArgument, SecondArgument);
        logger.Warn(CultureInfo.InvariantCulture, ThreeArgumentFormat, FirstArgument, SecondArgument, ThirdArgument);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^ThirdNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].message).Contains(ThirdArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the obsolete warn-exception message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.WarnException(PlainMessage, exception);
        logger.WarnException((string?)null, exception);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the obsolete warn-exception function overload emits when warn is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.WarnException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the obsolete warn-exception function overload skips work when warn is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.WarnException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the format-provider value overload emits at error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_FormatProvider_Value_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object value = PlainMessage;

        logger.Error(CultureInfo.InvariantCulture, value);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(PlainMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the exception-and-message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.Error(exception, PlainMessage);
        logger.Error(exception, (string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Error);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the exception-and-function overload emits when error is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Error(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the exception-and-function overload skips work when error is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Error(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the params-array message overloads emit at error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Message_Args_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object[] args = [FirstArgument, SecondArgument];

        // Invoke the invariant-culture params overload through a delegate so the call site does not require a format provider.
        Action<string, object[]> emit = logger.Error;
        emit(TwoArgumentFormat, args);
        logger.Error<NLogLoggerCoverageTests>(TwoArgumentFormat, args);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Error);
            await Assert.That(target.Logs[^1].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^1].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the strongly-typed format-provider argument overloads emit at error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_FormatProvider_Arguments_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error(CultureInfo.InvariantCulture, OneArgumentFormat, FirstArgument);
        logger.Error(CultureInfo.InvariantCulture, TwoArgumentFormat, FirstArgument, SecondArgument);
        logger.Error(CultureInfo.InvariantCulture, ThreeArgumentFormat, FirstArgument, SecondArgument, ThirdArgument);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^ThirdNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].message).Contains(ThirdArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the obsolete error-exception message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.ErrorException(PlainMessage, exception);
        logger.ErrorException((string?)null, exception);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the obsolete error-exception function overload emits when error is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.ErrorException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the obsolete error-exception function overload skips work when error is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.ErrorException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the format-provider value overload emits at fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_FormatProvider_Value_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object value = PlainMessage;

        logger.Fatal(CultureInfo.InvariantCulture, value);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(PlainMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the exception-and-message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.Fatal(exception, PlainMessage);
        logger.Fatal(exception, (string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Fatal);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the exception-and-function overload emits when fatal is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Fatal(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the exception-and-function overload skips work when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLoggerWithFatalDisabled();
        var invoked = false;

        logger.Fatal(new InvalidOperationException(ExceptionMessage), () =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the plain-function overload skips work when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLoggerWithFatalDisabled();
        var invoked = false;

        logger.Fatal(() =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the typed plain-function overload skips work when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Generic_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLoggerWithFatalDisabled();
        var invoked = false;

        logger.Fatal<NLogLoggerCoverageTests>(() =>
        {
            invoked = true;
            return PlainMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the params-array message overloads emit at fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Message_Args_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        object[] args = [FirstArgument, SecondArgument];

        // Invoke the invariant-culture params overload through a delegate so the call site does not require a format provider.
        Action<string, object[]> emit = logger.Fatal;
        emit(TwoArgumentFormat, args);
        logger.Fatal<NLogLoggerCoverageTests>(TwoArgumentFormat, args);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Fatal);
            await Assert.That(target.Logs[^1].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^1].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the strongly-typed format-provider argument overloads emit at fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_FormatProvider_Arguments_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal(CultureInfo.InvariantCulture, OneArgumentFormat, FirstArgument);
        logger.Fatal(CultureInfo.InvariantCulture, TwoArgumentFormat, FirstArgument, SecondArgument);
        logger.Fatal(CultureInfo.InvariantCulture, ThreeArgumentFormat, FirstArgument, SecondArgument, ThirdArgument);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^ThirdNewestLog].message).Contains(FirstArgument);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(SecondArgument);
            await Assert.That(target.Logs[^1].message).Contains(ThirdArgument);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the obsolete fatal-exception message overload emits for both a supplied and a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var exception = new InvalidOperationException(ExceptionMessage);

        logger.FatalException(PlainMessage, exception);
        logger.FatalException((string?)null, exception);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^SecondNewestLog].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the obsolete fatal-exception function overload emits when fatal is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Func_Emits_When_Enabled()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.FatalException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsTrue();
            await Assert.That(target.Logs[^1].message).Contains(PlainMessage);
            await Assert.That(target.Logs[^1].message).Contains(ExceptionMessage);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Verifies the obsolete fatal-exception function overload skips work when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Func_Does_Not_Emit_When_Disabled()
    {
        var (logger, target) = GetLoggerWithFatalDisabled();
        var invoked = false;

        logger.FatalException(
            () =>
            {
                invoked = true;
                return PlainMessage;
            },
            new InvalidOperationException(ExceptionMessage));

        using (Assert.Multiple())
        {
            await Assert.That(invoked).IsFalse();
            await Assert.That(target.Logs).IsEmpty();
        }
    }

    /// <summary>Verifies the debug message overloads emit an empty string when given a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Null_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug((string?)null);
        logger.Debug<NLogLoggerCoverageTests>((string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Debug);
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Verifies the info message overloads emit an empty string when given a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Null_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info((string?)null);
        logger.Info<NLogLoggerCoverageTests>((string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Info);
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Verifies the warn message overloads emit an empty string when given a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Null_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn((string?)null);
        logger.Warn<NLogLoggerCoverageTests>((string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Warn);
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Verifies the error message overloads emit an empty string when given a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Null_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error((string?)null);
        logger.Error<NLogLoggerCoverageTests>((string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Error);
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Verifies the fatal message overloads emit an empty string when given a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Null_Message_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal((string?)null);
        logger.Fatal<NLogLoggerCoverageTests>((string?)null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs[^SecondNewestLog].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^SecondNewestLog].logLevel).IsEqualTo(LogLevel.Fatal);
            await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo(string.Empty);
            await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Creates an <see cref="NLogLogger"/> backed by a capturing in-memory target.</summary>
    /// <param name="minimumLogLevel">The minimum Splat log level to configure.</param>
    /// <returns>The configured logger and its capture target.</returns>
    private static (NLogLogger Logger, MemoryTargetWrapper Target) GetLogger(LogLevel minimumLogLevel)
    {
        var configuration = new LoggingConfiguration();

        var errorTarget = new MemoryTargetWrapper
        {
            Layout = "${message} ${exception:format=tostring}",
        };

        configuration.AddTarget(errorTarget);
        var errorLoggingRule = new LoggingRule("*", _splat2NLog[minimumLogLevel], errorTarget);
        configuration.LoggingRules.Add(errorLoggingRule);

        LogManager.Configuration = configuration;

        return (new NLogLogger(LogManager.GetCurrentClassLogger()), errorTarget);
    }

    /// <summary>Creates an <see cref="NLogLogger"/> whose configuration enables every level except fatal.</summary>
    /// <returns>The configured logger and its capture target.</returns>
    private static (NLogLogger Logger, MemoryTargetWrapper Target) GetLoggerWithFatalDisabled()
    {
        var configuration = new LoggingConfiguration();

        var errorTarget = new MemoryTargetWrapper
        {
            Layout = "${message} ${exception:format=tostring}",
        };

        configuration.AddTarget(errorTarget);
        var loggingRule = new LoggingRule("*", global::NLog.LogLevel.Debug, global::NLog.LogLevel.Error, errorTarget);
        configuration.LoggingRules.Add(loggingRule);

        LogManager.Configuration = configuration;

        return (new NLogLogger(LogManager.GetCurrentClassLogger()), errorTarget);
    }

    /// <summary>An NLog target that captures rendered log events for assertions.</summary>
    private sealed class MemoryTargetWrapper : TargetWithLayout
    {
        /// <summary>Mappings of NLog log levels to equivalent Splat log levels.</summary>
        private static readonly Dictionary<global::NLog.LogLevel, LogLevel> _nlog2Splat = new()
        {
            { global::NLog.LogLevel.Debug, LogLevel.Debug },
            { global::NLog.LogLevel.Error, LogLevel.Error },
            { global::NLog.LogLevel.Warn, LogLevel.Warn },
            { global::NLog.LogLevel.Fatal, LogLevel.Fatal },
            { global::NLog.LogLevel.Info, LogLevel.Info },
        };

        /// <summary>The captured log entries.</summary>
        private readonly List<(LogLevel LogLevel, string Message)> _logs = [];

        /// <summary>Initializes a new instance of the <see cref="MemoryTargetWrapper"/> class.</summary>
        public MemoryTargetWrapper() => Name = "coverage wrapper";

        /// <summary>Gets the captured log entries.</summary>
        public IReadOnlyList<(LogLevel logLevel, string message)> Logs => _logs;

        /// <summary>Renders the logging event message and records it.</summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent) => _logs.Add((_nlog2Splat[logEvent.Level], RenderLogEvent(Layout, logEvent)));
    }
}
