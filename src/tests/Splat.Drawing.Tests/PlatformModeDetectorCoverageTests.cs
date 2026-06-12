// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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

    /// <summary>A stub mode detector returning a fixed design-mode value.</summary>
    /// <param name="result">The value to return from <see cref="StubModeDetector.InDesignMode"/>.</param>
    private sealed class StubModeDetector(bool? result) : IPlatformModeDetector
    {
        /// <inheritdoc />
        public bool? InDesignMode() => result;
    }
}
