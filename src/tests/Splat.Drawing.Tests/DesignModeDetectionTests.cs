// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering how <see cref="DefaultPlatformModeDetector"/> decides that a designer is hosting the application.</summary>
[NotInParallel] // Mutates the memoized design-mode result, which is static state.
public sealed class DesignModeDetectionTests
{
    /// <summary>An entry-point path whose executable name matches a known design-environment host.</summary>
    private const string DesignHostPath = "/apps/design/BLEND.EXE";

    /// <summary>An entry-point path whose executable name does not match any known design environment.</summary>
    private const string ApplicationHostPath = "/apps/myapp/MyApp.dll";

    /// <summary>Verifies that a probe reporting a design environment is what the detector reports, rather than being discarded.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DetectDesignMode_WithDesignHostExecutable_ReportsDesignMode() =>
        await Assert.That(DefaultPlatformModeDetector.DetectDesignMode(DesignHostPath)).IsTrue();

    /// <summary>Verifies that an ordinary application host is not reported as a designer.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DetectDesignMode_WithApplicationExecutable_ReportsRuntime() =>
        await Assert.That(DefaultPlatformModeDetector.DetectDesignMode(ApplicationHostPath)).IsFalse();

    /// <summary>Verifies that an unavailable host path is not reported as a designer.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DetectDesignMode_WithUnavailableHostPath_ReportsRuntime() =>
        await Assert.That(DefaultPlatformModeDetector.DetectDesignMode(null)).IsFalse();

    /// <summary>Verifies that a WPF designer signal is reported even when nothing else indicates design mode.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolveDesignMode_WhenWpfProbeReportsDesigner_ReportsDesignMode() =>
        await Assert.That(DefaultPlatformModeDetector.ResolveDesignMode(true, false, ApplicationHostPath)).IsTrue();

    /// <summary>Verifies that a Windows Runtime designer signal is reported even when nothing else indicates design mode.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolveDesignMode_WhenWinRtProbeReportsDesigner_ReportsDesignMode() =>
        await Assert.That(DefaultPlatformModeDetector.ResolveDesignMode(false, true, ApplicationHostPath)).IsTrue();

    /// <summary>Verifies that the host executable decides when neither designer probe reports anything.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolveDesignMode_WhenNoProbeReportsDesigner_FallsBackToHostExecutable() =>
        await Assert.That(DefaultPlatformModeDetector.ResolveDesignMode(false, false, DesignHostPath)).IsTrue();

    /// <summary>Verifies that nothing indicating design mode reports a running application.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolveDesignMode_WhenNothingReportsDesigner_ReportsRuntime() =>
        await Assert.That(DefaultPlatformModeDetector.ResolveDesignMode(false, false, ApplicationHostPath)).IsFalse();

    /// <summary>Verifies that the design-host match ignores case, as Windows executable names do.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithDifferentlyCasedDesignHost_IsTrue() =>
        await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry("/apps/design/Blend.exe")).IsTrue();

    /// <summary>Verifies that a directory path, which has no executable name, is not treated as a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithDirectoryPath_IsFalse() =>
        await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry("/apps/myapp/bin/")).IsFalse();

    /// <summary>Verifies that an empty entry path is not treated as a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithEmptyEntry_IsFalse() =>
        await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry(string.Empty)).IsFalse();

    /// <summary>Verifies that an executable whose name is merely a fragment of a design host name is not a match.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsDesignEnvironmentEntry_WithNameFragmentOfDesignHost_IsFalse() =>
        await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry("/apps/myapp/END.EXE")).IsFalse();

    /// <summary>Verifies that the host path names an executable file rather than the directory it lives in.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HostExecutablePath_NamesAnExecutableFile()
    {
        var hostExecutablePath = DefaultPlatformModeDetector.HostExecutablePath;

        await Assert.That(hostExecutablePath).IsNotNullOrEmpty();
        await Assert.That(new FileInfo(hostExecutablePath!).Name).IsNotEmpty();
    }

    /// <summary>Verifies that the running test host is not mistaken for a design environment.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HostExecutablePath_IsNotADesignEnvironment() =>
        await Assert.That(DefaultPlatformModeDetector.IsDesignEnvironmentEntry(DefaultPlatformModeDetector.HostExecutablePath)).IsFalse();

    /// <summary>Verifies that the detector reports the memoized result rather than re-probing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_ReportsMemoizedResult()
    {
        var saved = DefaultPlatformModeDetector.GetState();
        try
        {
            DefaultPlatformModeDetector.RestoreState(true);

            await Assert.That(new DefaultPlatformModeDetector().InDesignMode()).IsTrue();
        }
        finally
        {
            DefaultPlatformModeDetector.RestoreState(saved);
        }
    }

    /// <summary>Verifies that discarding the memoized result makes the detector probe again.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InDesignMode_AfterStateReset_ProbesAgain()
    {
        var saved = DefaultPlatformModeDetector.GetState();
        try
        {
            DefaultPlatformModeDetector.RestoreState(true);
            DefaultPlatformModeDetector.ResetState();

            await Assert.That(DefaultPlatformModeDetector.GetState()).IsNull();
            await Assert.That(new DefaultPlatformModeDetector().InDesignMode()).IsFalse();
        }
        finally
        {
            DefaultPlatformModeDetector.RestoreState(saved);
        }
    }
}
