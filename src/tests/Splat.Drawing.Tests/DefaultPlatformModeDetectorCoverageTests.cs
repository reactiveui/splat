// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering <see cref="DefaultPlatformModeDetector"/>.</summary>
public sealed class DefaultPlatformModeDetectorCoverageTests
{
    /// <summary>An entry-point path whose executable name matches a known design-environment host.</summary>
    private const string DesignHostPath = "/apps/design/BLEND.EXE";

    /// <summary>An entry-point path whose executable name does not match any known design environment.</summary>
    private const string RegularHostPath = "/apps/myapp/MyApp.dll";

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

    /// <summary>Verifies that a null entry-point path is not treated as a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithNullEntry_IsFalse()
        => await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry(null)).IsFalse();

    /// <summary>Verifies that a known design-environment host executable is recognised as a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithDesignHostExecutable_IsTrue()
        => await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry(DesignHostPath)).IsTrue();

    /// <summary>Verifies that an ordinary host executable is not treated as a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithNonDesignExecutable_IsFalse()
        => await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry(RegularHostPath)).IsFalse();
}
