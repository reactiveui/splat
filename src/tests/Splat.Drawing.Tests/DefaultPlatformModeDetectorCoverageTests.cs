// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering <see cref="DefaultPlatformModeDetector"/>.</summary>
public sealed class DefaultPlatformModeDetectorCoverageTests
{
    /// <summary>Verifies that <see cref="DefaultPlatformModeDetector.InDesignMode"/> does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_DoesNotThrow()
    {
        var detector = new DefaultPlatformModeDetector();

        await Assert.That(() => detector.InDesignMode()).ThrowsNothing();
    }

    /// <summary>Verifies that <see cref="DefaultPlatformModeDetector.InDesignMode"/> reports not in design mode under the test runner.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_ReturnsFalseUnderTestRunner()
    {
        var detector = new DefaultPlatformModeDetector();

        await Assert.That(detector.InDesignMode()).IsFalse();
    }

    /// <summary>Verifies that repeated calls return the same cached value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_IsStableAcrossCalls()
    {
        var detector = new DefaultPlatformModeDetector();

        var first = detector.InDesignMode();
        var second = detector.InDesignMode();

        await Assert.That(second).IsEqualTo(first);
    }
}
