// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !MONO
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;
#if NETFRAMEWORK
using System.Reflection;
#endif

namespace Splat;

/// <summary>Provides a default implementation for detecting whether the application is running in design mode.</summary>
/// <remarks>
/// <para>This class is typically used to determine if code is executing within a designer environment, such as
/// Visual Studio or Blend, to enable or disable design-time specific logic. The detection result is memoized, so a
/// process that starts outside a designer never re-runs the probes. Thread safety is not guaranteed.</para>
/// <para>Which probes are compiled depends on the target framework. Splat.Drawing sets <c>UseWPF</c> and
/// <c>UseWindowsForms</c> for the .NET Framework and Windows-specific targets, so those builds call the WPF designer
/// API directly instead of reflecting over it. The target-framework-neutral builds keep a reflective probe, because a
/// Windows desktop application resolves to them when its own platform version predates the Windows-specific targets.
/// The Android and Apple targets cannot host a XAML designer at all and compile no designer probe.</para>
/// </remarks>
public class DefaultPlatformModeDetector : IPlatformModeDetector
{
#if !NETFRAMEWORK && !WINDOWS && !MONO
    /// <summary>Assembly-qualified name of the WPF <c>DesignerProperties</c> type probed for design mode.</summary>
    private const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

    /// <summary>Name of the WPF <c>DesignerProperties</c> method that reports design mode.</summary>
    private const string WpfDesignerPropertiesDesignModeMethod = "GetIsInDesignMode";

    /// <summary>Assembly-qualified name of the WPF element type the designer raises the design-mode default for.</summary>
    private const string WpfFrameworkElementType = "System.Windows.FrameworkElement, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
#endif

#if !MONO
    /// <summary>Assembly-qualified name of the Windows Runtime <c>DesignMode</c> type probed for design mode.</summary>
    private const string WinRtDesignModeType = "Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime";

    /// <summary>Name of the Windows Runtime <c>DesignMode</c> property that reports design mode.</summary>
    private const string WinRtDesignModeEnabledProperty = "DesignModeEnabled";
#endif

    /// <summary>Executable names of known design-time host processes used as a fallback design-mode signal.</summary>
    private static readonly string[] _designEnvironments = ["BLEND.EXE", "XDESPROC.EXE"];

    /// <summary>Memoizes the design-mode detection result; <see langword="null"/> until first computed.</summary>
    private static bool? _cachedInDesignModeResult;

    /// <summary>Gets the path of the executable hosting the current process, or <see langword="null"/> when it is unavailable.</summary>
    /// <remarks>This is the executable file, not the directory the assembly was loaded from. A design-time host is
    /// recognised by its process name, and a directory path yields no executable name to compare.</remarks>
    internal static string? HostExecutablePath =>
#if NETFRAMEWORK
        Assembly.GetEntryAssembly()?.Location;
#else
        Environment.ProcessPath;
#endif

    /// <inheritdoc />
    public bool? InDesignMode() => _cachedInDesignModeResult ??= DetectDesignMode(HostExecutablePath);

    /// <summary>Determines whether the supplied host entry-point path names a known design-environment executable.</summary>
    /// <param name="entry">The host entry-point path, or <see langword="null"/> when it is unavailable.</param>
    /// <returns><see langword="true"/> when the entry path names a known design environment; otherwise <see langword="false"/>.</returns>
    /// <remarks>The executable name has to match a known design host exactly. A containment test reports a false
    /// positive for every name that is a fragment of one, including the empty name a directory path yields.</remarks>
    internal static bool IsDesignEnvironmentEntry(string? entry)
    {
        if (string.IsNullOrEmpty(entry))
        {
            return false;
        }

        var executableName = new FileInfo(entry).Name;

        if (executableName.Length == 0)
        {
            return false;
        }

        foreach (var knownHost in _designEnvironments)
        {
            if (string.Equals(knownHost, executableName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Runs the design-mode probes and reports whether any of them detected a designer.</summary>
    /// <param name="hostExecutablePath">The path of the executable hosting the current process, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a designer is hosting the application; otherwise <see langword="false"/>.</returns>
    internal static bool DetectDesignMode(string? hostExecutablePath) =>
#if MONO
        IsDesignEnvironmentEntry(hostExecutablePath);
#else
        ResolveDesignMode(ProbeWpfDesignMode(), ProbeWinRtDesignMode(), hostExecutablePath);
#endif

#if !MONO
    /// <summary>Combines the probe results into a single answer: any probe reporting a designer wins.</summary>
    /// <param name="wpfDesignMode">Whether the WPF probe reported a designer.</param>
    /// <param name="winRtDesignMode">Whether the Windows Runtime probe reported a designer.</param>
    /// <param name="hostExecutablePath">The path of the executable hosting the current process, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a designer is hosting the application; otherwise <see langword="false"/>.</returns>
    internal static bool ResolveDesignMode(bool wpfDesignMode, bool winRtDesignMode, string? hostExecutablePath) =>
        wpfDesignMode || winRtDesignMode || IsDesignEnvironmentEntry(hostExecutablePath);
#endif

    /// <summary>Gets the memoized design-mode result. Used by test scopes.</summary>
    /// <returns>The memoized result, or <see langword="null"/> when the probes have not run yet.</returns>
    internal static bool? GetState() => _cachedInDesignModeResult;

    /// <summary>Restores the memoized design-mode result. Used by test scopes.</summary>
    /// <param name="state">The memoized result to restore.</param>
    internal static void RestoreState(bool? state) => _cachedInDesignModeResult = state;

    /// <summary>Discards the memoized design-mode result so the next query re-runs the probes. Used by test scopes.</summary>
    internal static void ResetState() => _cachedInDesignModeResult = null;

#if !MONO
#if NETFRAMEWORK || WINDOWS
    /// <summary>Reads the design-mode default that the WPF designer raises for the elements it loads.</summary>
    /// <returns><see langword="true"/> when a XAML designer is hosting the application; otherwise <see langword="false"/>.</returns>
    /// <remarks>The designer raises the default that <c>FrameworkElement</c> reports for the <c>IsInDesignMode</c>
    /// attached property, which is what makes every element it loads answer <see langword="true"/>. Asking
    /// <c>DesignerProperties.GetIsInDesignMode</c> about a freshly constructed, unparented <c>DependencyObject</c>
    /// reads the unraised base default instead, so it answers <see langword="false"/> for any control the designer did
    /// not set the attached property on directly - a user control nested inside another user control, for example.
    /// Reading the metadata needs no element at all, so it also avoids creating a dispatcher-affine object.</remarks>
    [ExcludeFromCodeCoverage] // Only a hosting designer raises this default, and the call that would simulate it mutates the property metadata for the whole process.
    private static bool ProbeWpfDesignMode() =>
        System.ComponentModel.DesignerProperties.IsInDesignModeProperty
            .GetMetadata(typeof(System.Windows.FrameworkElement)).DefaultValue is true;
#else
    /// <summary>Reads the design-mode default that the WPF designer raises, when WPF is loaded into the process.</summary>
    /// <returns><see langword="true"/> when a XAML designer is hosting the application; otherwise <see langword="false"/>.</returns>
    /// <remarks>The element handed to <c>GetIsInDesignMode</c> is a <c>FrameworkElement</c> rather than a bare
    /// <c>DependencyObject</c>, because the designer raises the attached property's default for
    /// <c>FrameworkElement</c>. A bare <c>DependencyObject</c> reads the unraised base default and therefore answers
    /// <see langword="false"/> for any control the designer did not set the attached property on directly - a user
    /// control nested inside another user control, for example.</remarks>
    [ExcludeFromCodeCoverage] // Off-platform reflection: PresentationFramework only resolves inside a WPF host.
    private static bool ProbeWpfDesignMode()
    {
        var designerProperties = Type.GetType(WpfDesignerPropertiesType, false);

        if (designerProperties is null)
        {
            return false;
        }

        var designModeMethod = designerProperties.GetMethod(WpfDesignerPropertiesDesignModeMethod);
        var frameworkElement = Type.GetType(WpfFrameworkElementType, false);

        return designModeMethod is not null
            && frameworkElement is not null
            && designModeMethod.Invoke(null, [Activator.CreateInstance(frameworkElement)]) is true;
    }
#endif

    /// <summary>Reads the Windows Runtime design-mode indicator, when the Windows Runtime is available.</summary>
    /// <returns><see langword="true"/> when a Windows Runtime designer is hosting the application; otherwise <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage] // Off-platform reflection: the Windows Runtime projection only resolves inside a Windows Runtime host.
    private static bool ProbeWinRtDesignMode() =>
        Type.GetType(WinRtDesignModeType, false)?
            .GetProperty(WinRtDesignModeEnabledProperty)?
            .GetMethod?
            .Invoke(null, null) is true;
#endif
}
