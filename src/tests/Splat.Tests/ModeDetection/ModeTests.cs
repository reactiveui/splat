// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;
using Splat.ModeDetection;

namespace Splat.Tests.ModeDetection;

[NotInParallel]
public class ModeTests
{
    private ModeDetectorScope? _modeDetectorScope;

    /// <summary>
    /// Setup method to initialize ModeDetectorScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpModeDetectorScope()
    {
        _modeDetectorScope = new ModeDetectorScope();
    }

    /// <summary>
    /// Teardown method to dispose ModeDetectorScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownModeDetectorScope()
    {
        _modeDetectorScope?.Dispose();
        _modeDetectorScope = null;
    }

    /// <summary>
    /// Tests the <see cref="Mode.Run"/> mode.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RunModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Run);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        await Assert.That(inUnitTestRunner).IsFalse();
    }

    /// <summary>
    /// Tests the <see cref="Mode.Test"/> mode.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TestModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        await Assert.That(inUnitTestRunner).IsTrue();
    }

    /// <summary>
    /// Tests that ModeDetector caches results properly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModeDetector_CachesResults()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act - Call multiple times
        var result1 = ModeDetector.InUnitTestRunner();
        var result2 = ModeDetector.InUnitTestRunner();
        var result3 = ModeDetector.InUnitTestRunner();

        using (Assert.Multiple())
        {
            // Assert - Should all be the same (cached)
            await Assert.That(result1).IsTrue();
            await Assert.That(result2).IsTrue();
            await Assert.That(result3).IsTrue();
        }
    }

    /// <summary>
    /// Tests that overriding mode detector clears cache.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModeDetector_OverrideClearsCache()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);
        var result1 = ModeDetector.InUnitTestRunner();
        await Assert.That(result1).IsTrue();

        // Act - Override with different mode
        ModeDetector.OverrideModeDetector(Mode.Run);
        var result2 = ModeDetector.InUnitTestRunner();

        // Assert - Should reflect new mode
        await Assert.That(result2).IsFalse();
    }

    /// <summary>
    /// Tests that ModeDetector handles null detector gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModeDetector_HandlesNullDetector()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(null!);

        // Act
        var result = ModeDetector.InUnitTestRunner();

        // Assert - Should return false as fallback
        await Assert.That(result).IsFalse();
    }
}
