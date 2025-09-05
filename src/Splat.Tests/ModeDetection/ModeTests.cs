// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ModeDetection;

namespace Splat.Tests.ModeDetection;

/// <summary>
/// Unit tests for the <see cref="Mode"/> class.
/// </summary>
[TestFixture]
public class ModeTests
{
    /// <summary>
    /// Tests the <see cref="Mode.Run"/> mode.
    /// </summary>
    [Test]
    public void RunModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Run);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        Assert.That(inUnitTestRunner, Is.False);
    }

    /// <summary>
    /// Tests the <see cref="Mode.Test"/> mode.
    /// </summary>
    [Test]
    public void TestModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        Assert.That(inUnitTestRunner, Is.True);
    }

    /// <summary>
    /// Tests that ModeDetector caches results properly.
    /// </summary>
    [Test]
    public void ModeDetector_CachesResults()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act - Call multiple times
        var result1 = ModeDetector.InUnitTestRunner();
        var result2 = ModeDetector.InUnitTestRunner();
        var result3 = ModeDetector.InUnitTestRunner();

        using (Assert.EnterMultipleScope())
        {
            // Assert - Should all be the same (cached)
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.True);
            Assert.That(result3, Is.True);
        }
    }

    /// <summary>
    /// Tests that overriding mode detector clears cache.
    /// </summary>
    [Test]
    public void ModeDetector_OverrideClearsCache()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);
        var result1 = ModeDetector.InUnitTestRunner();
        Assert.That(result1, Is.True);

        // Act - Override with different mode
        ModeDetector.OverrideModeDetector(Mode.Run);
        var result2 = ModeDetector.InUnitTestRunner();

        // Assert - Should reflect new mode
        Assert.That(result2, Is.False);
    }

    /// <summary>
    /// Tests that ModeDetector handles null detector gracefully.
    /// </summary>
    [Test]
    public void ModeDetector_HandlesNullDetector()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(null!);

        // Act
        var result = ModeDetector.InUnitTestRunner();

        // Assert - Should return false as fallback
        Assert.That(result, Is.False);
    }
}
