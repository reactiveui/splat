// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="LogHost"/> and <c>LogHostExtensions</c> entry points.</summary>
[NotInParallel] // touches global AppLocator state
public class LogHostCoverageTests : IEnableLogger
{
    /// <summary>Test that LogHost.Default returns a static full logger when an ILogManager is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Default_Returns_Static_Full_Logger()
    {
        using var scope = new AppLocatorScope();
        AppLocator.CurrentMutable.Register<ILogManager>(static () => new FuncLogManager(static _ => new WrappingFullLogger(new TextLogger())));

        var logger = LogHost.Default;

        await Assert.That(logger).IsNotNull();
    }

    /// <summary>Test that LogHost.Default can write without throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Default_Can_Write_Without_Throwing()
    {
        using var scope = new AppLocatorScope();
        var textLogger = new TextLogger();
        AppLocator.CurrentMutable.Register<ILogManager>(() => new FuncLogManager(_ => new WrappingFullLogger(textLogger)));

        await Assert.That(static () => LogHost.Default.Info("hello")).ThrowsNothing();
    }

    /// <summary>Test that LogHost.Default throws a LoggingException when no ILogManager is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Default_Without_LogManager_Throws_LoggingException()
    {
        using var scope = new AppLocatorScope();
        AppLocator.CurrentMutable.UnregisterAll<ILogManager>();

        await Assert.That(static () => _ = LogHost.Default).Throws<LoggingException>();
    }

    /// <summary>Test that the this.Log() extension returns a full logger when an ILogManager is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Log_Extension_Returns_Full_Logger()
    {
        using var scope = new AppLocatorScope();
        AppLocator.CurrentMutable.Register<ILogManager>(static () => new FuncLogManager(static _ => new WrappingFullLogger(new TextLogger())));

        var logger = this.Log();

        await Assert.That(logger).IsTypeOf<WrappingFullLogger>();
    }

    /// <summary>Test that the this.Log() extension throws when no ILogManager is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Log_Extension_Without_LogManager_Throws_InvalidOperationException()
    {
        using var scope = new AppLocatorScope();
        AppLocator.CurrentMutable.UnregisterAll<ILogManager>();

        await Assert.That(() => this.Log()).Throws<InvalidOperationException>();
    }
}
