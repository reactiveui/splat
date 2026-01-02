// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Unit tests for <see cref="FullLoggerExtensions"/> class.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate us of Exception for testing")]
public sealed class FullLoggerExtensionsTests
{
    // Debug tests
    [Test]
    public async Task Debug_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Debug(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task Debug_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Debug(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Debug_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Debug };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Debug(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
        await Assert.That(textLogger.Logs).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Debug_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Debug(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
        await Assert.That(textLogger.Logs).Count().IsEqualTo(0);
    }

    // Debug<T> tests
    [Test]
    public async Task DebugGeneric_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Debug<string>(null!, () => "test"))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task DebugGeneric_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Debug<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DebugGeneric_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Debug };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Debug<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task DebugGeneric_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Debug<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // DebugException tests
    [Test]
    public async Task DebugException_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.DebugException(null!, () => "test", new Exception()))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task DebugException_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.DebugException(null!, new Exception())).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DebugException_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Debug };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.DebugException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task DebugException_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.DebugException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsFalse();
    }

    // Info tests
    [Test]
    public async Task Info_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Info(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task Info_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Info(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Info_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Info(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task Info_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Info(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // Info<T> tests
    [Test]
    public async Task InfoGeneric_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Info<string>(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task InfoGeneric_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Info<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task InfoGeneric_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Info<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task InfoGeneric_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Info<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // InfoException tests
    [Test]
    public async Task InfoException_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.InfoException(null!, () => "test", new Exception()))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task InfoException_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.InfoException(null!, new Exception())).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task InfoException_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Info };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.InfoException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task InfoException_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.InfoException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsFalse();
    }

    // Warn tests
    [Test]
    public async Task Warn_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Warn(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task Warn_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Warn(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Warn_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Warn(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task Warn_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Warn(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // Warn<T> tests
    [Test]
    public async Task WarnGeneric_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Warn<string>(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task WarnGeneric_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Warn<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task WarnGeneric_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Warn<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task WarnGeneric_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Warn<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // WarnException tests
    [Test]
    public async Task WarnException_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.WarnException(null!, () => "test", new Exception()))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task WarnException_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.WarnException(null!, new Exception())).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task WarnException_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.WarnException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task WarnException_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.WarnException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsFalse();
    }

    // Error tests
    [Test]
    public async Task Error_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Error(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task Error_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Error(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Error_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Error(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task Error_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Error(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // Error<T> tests
    [Test]
    public async Task ErrorGeneric_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Error<string>(null!, () => "test"))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task ErrorGeneric_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Error<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ErrorGeneric_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Error<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task ErrorGeneric_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Error<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsFalse();
    }

    // ErrorException tests
    [Test]
    public async Task ErrorException_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.ErrorException(null!, () => "test", new Exception()))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task ErrorException_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.ErrorException(null!, new Exception())).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ErrorException_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.ErrorException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsTrue();
    }

    [Test]
    public async Task ErrorException_WhenDisabled_DoesNotInvokeFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.ErrorException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsFalse();
    }

    // Fatal tests
    [Test]
    public async Task Fatal_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Fatal(null!, () => "test")).Throws<ArgumentNullException>();

    [Test]
    public async Task Fatal_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Fatal(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Fatal_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Fatal(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    // Fatal<T> tests
    [Test]
    public async Task FatalGeneric_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.Fatal<string>(null!, () => "test"))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task FatalGeneric_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.Fatal<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FatalGeneric_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.Fatal<string>(() =>
        {
            invoked = true;
            return "test message";
        });

        await Assert.That(invoked).IsTrue();
    }

    // FatalException tests
    [Test]
    public async Task FatalException_WithNullLogger_ThrowsArgumentNullException() =>
        await Assert.That(() => FullLoggerExtensions.FatalException(null!, () => "test", new Exception()))
            .Throws<ArgumentNullException>();

    [Test]
    public async Task FatalException_WithNullFunction_ThrowsArgumentNullException()
    {
        var logger = new WrappingFullLogger(new TextLogger());
        await Assert.That(() => logger.FatalException(null!, new Exception())).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FatalException_WhenEnabled_InvokesFunction()
    {
        var textLogger = new TextLogger { Level = LogLevel.Fatal };
        var logger = new WrappingFullLogger(textLogger);
        var invoked = false;

        logger.FatalException(
            () =>
            {
                invoked = true;
                return "test message";
            },
            new Exception("test exception"));

        await Assert.That(invoked).IsTrue();
    }
}
