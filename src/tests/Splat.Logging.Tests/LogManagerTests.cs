// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Tests for log manager classes.</summary>
public class LogManagerTests
{
    /// <summary>Test that DefaultLogManager creates WrappingFullLogger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultLogManager_Should_Create_WrappingFullLogger()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register<ILogger>(static () => new TextLogger());
        var logManager = new DefaultLogManager(resolver);
        var logger = logManager.GetLogger(typeof(LogManagerTests));

        await Assert.That(logger).IsTypeOf<WrappingFullLogger>();
    }

    /// <summary>Test that FuncLogManager uses provided factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FuncLogManager_Should_Use_Factory()
    {
        const int expectedFactoryCallCount = 2;
        var callCount = 0;
        var logManager = new FuncLogManager(type =>
        {
            callCount++;
            return new WrappingFullLogger(new TextLogger());
        });

        var logger1 = logManager.GetLogger(typeof(LogManagerTests));
        var logger2 = logManager.GetLogger<LogManagerTests>();

        using (Assert.Multiple())
        {
            await Assert.That(callCount).IsEqualTo(expectedFactoryCallCount);
            await Assert.That(logger1).IsTypeOf<WrappingFullLogger>();
            await Assert.That(logger2).IsTypeOf<WrappingFullLogger>();
        }
    }

    /// <summary>Test that LogManagerMixin creates IFullLogger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task LogManagerMixin_Should_Create_FullLogger()
    {
        var logManager = new FuncLogManager(static _ => new WrappingFullLogger(new TextLogger()));
        var logger = logManager.GetLogger<LogManagerTests>();

        await Assert.That(logger).IsTypeOf<WrappingFullLogger>();
    }
}
