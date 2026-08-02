// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering the static <see cref="PlatformModeDetector"/>.</summary>
[NotInParallel] // Mutates the global PlatformModeDetector static state.
public sealed class PlatformModeDetectorCoverageTests
{
    /// <summary>Verifies that <see cref="PlatformModeDetector.OverrideModeDetector"/> causes delegation to the supplied detector.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task OverrideModeDetector_DelegatesToProvidedDetector()
    {
        var saved = PlatformModeDetector.GetState();
        try
        {
            PlatformModeDetector.OverrideModeDetector(new StubModeDetector(true));

            await Assert.That(PlatformModeDetector.InDesignMode()).IsTrue();
        }
        finally
        {
            PlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>Verifies that overriding with a detector that reports false yields false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task OverrideModeDetector_FalseDetector_ReturnsFalse()
    {
        var saved = PlatformModeDetector.GetState();
        try
        {
            PlatformModeDetector.OverrideModeDetector(new StubModeDetector(false));

            await Assert.That(PlatformModeDetector.InDesignMode()).IsFalse();
        }
        finally
        {
            PlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>Verifies that a repeat query reports the memoized result instead of asking the detector again.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_OnRepeatQuery_ReportsMemoizedResult()
    {
        var saved = PlatformModeDetector.GetState();
        try
        {
            var detector = new StubModeDetector(true);
            PlatformModeDetector.OverrideModeDetector(detector);

            var first = PlatformModeDetector.InDesignMode();
            var second = PlatformModeDetector.InDesignMode();

            using (Assert.Multiple())
            {
                await Assert.That(first).IsTrue();
                await Assert.That(second).IsTrue();
                await Assert.That(detector.QueryCount).IsEqualTo(1);
            }
        }
        finally
        {
            PlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>Verifies that a null design-mode result falls back to false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_NullResult_FallsBackToFalse()
    {
        var saved = PlatformModeDetector.GetState();
        try
        {
            PlatformModeDetector.OverrideModeDetector(new StubModeDetector(null));

            await Assert.That(PlatformModeDetector.InDesignMode()).IsFalse();
        }
        finally
        {
            PlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>Verifies that <see cref="PlatformModeDetector.ResetState"/> restores the default detector behaviour.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResetState_RestoresDefaultDetector()
    {
        var saved = PlatformModeDetector.GetState();
        try
        {
            PlatformModeDetector.OverrideModeDetector(new StubModeDetector(true));
            PlatformModeDetector.ResetState();

            await Assert.That(PlatformModeDetector.InDesignMode()).IsFalse();
        }
        finally
        {
            PlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>A stub mode detector returning a fixed design-mode value and counting how often it was asked.</summary>
    /// <param name="result">The value to return from <see cref="StubModeDetector.InDesignMode"/>.</param>
    private sealed class StubModeDetector(bool? result) : IPlatformModeDetector
    {
        /// <summary>Gets the number of times the detector was asked for the design-mode value.</summary>
        public int QueryCount { get; private set; }

        /// <inheritdoc />
        public bool? InDesignMode()
        {
            QueryCount++;
            return result;
        }
    }
}
