// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Splat;

/// <summary>
/// Provides a default implementation for detecting whether the application is running in design mode across supported
/// platforms.
/// </summary>
/// <remarks>This class is typically used to determine if code is executing within a designer environment, such as
/// Visual Studio or Blend, to enable or disable design-time specific logic. It supports multiple platforms and design
/// environments, including WPF, Silverlight, and UWP, by checking for known design mode indicators. The detection
/// result may be cached for performance. Thread safety is not guaranteed.</remarks>
public class DefaultPlatformModeDetector : IPlatformModeDetector
{
    /// <summary>Assembly-qualified name of the Silverlight/XAML <c>DesignerProperties</c> type probed for design mode.</summary>
    private const string XamlDesignPropertiesType = "System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

    /// <summary>Assembly-qualified name of the XAML <c>Border</c> control used as the dependency object for the design-mode probe.</summary>
    private const string XamlControlBorderType = "System.Windows.Controls.Border, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

    /// <summary>Name of the XAML <c>DesignerProperties</c> method that reports design mode.</summary>
    private const string XamlDesignPropertiesDesignModeMethodName = "GetIsInDesignMode";

    /// <summary>Assembly-qualified name of the WPF <c>DesignerProperties</c> type probed for design mode.</summary>
    private const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

    /// <summary>Name of the WPF <c>DesignerProperties</c> method that reports design mode.</summary>
    private const string WpfDesignerPropertiesDesignModeMethod = "GetIsInDesignMode";

    /// <summary>Assembly-qualified name of the WPF <c>DependencyObject</c> type used as the dependency object for the design-mode probe.</summary>
    private const string WpfDependencyPropertyType = "System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

    /// <summary>Assembly-qualified name of the WinRT <c>DesignMode</c> type probed for design mode.</summary>
    private const string WinFormsDesignerPropertiesType = "Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime";

    /// <summary>Name of the WinRT <c>DesignMode</c> property that reports design mode.</summary>
    private const string WinFormsDesignerPropertiesDesignModeMethod = "DesignModeEnabled";

    /// <summary>Executable names of known design-time host processes used as a fallback design-mode signal.</summary>
    private static readonly string[] _designEnvironments = ["BLEND.EXE", "XDESPROC.EXE"];

    /// <summary>Memoizes the design-mode detection result; <see langword="null"/> until first computed.</summary>
    private static bool? _cachedInDesignModeResult;

    /// <inheritdoc />
    public bool? InDesignMode() => DetectDesignMode();

    /// <summary>Determines whether the supplied host entry-point path names a known design-environment executable.</summary>
    /// <param name="entry">The host entry-point path, or <see langword="null"/> when it is unavailable.</param>
    /// <returns><see langword="true"/> when the entry path names a known design environment; otherwise <see langword="false"/>.</returns>
    internal static bool IsDesignEnvironmentEntry(string? entry)
    {
        if (entry is null)
        {
            return false;
        }

        var exeName = new FileInfo(entry).Name;

        foreach (var designEnv in _designEnvironments)
        {
            if (IsDesignEnvironment(designEnv, exeName))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Runs the design-mode probes in priority order and returns (and caches) the result.</summary>
    /// <returns>A value indicating whether the application is running in design mode.</returns>
    private static bool? DetectDesignMode()
    {
        if (_cachedInDesignModeResult.HasValue)
        {
            return _cachedInDesignModeResult.Value;
        }

        // Probe each platform in priority order; the first detected platform wins,
        // mirroring the original else-if chain. The cached result is then reset to
        // false below, preserving the original behaviour.
        RunDesignModeProbes();

        _cachedInDesignModeResult = false;

        return _cachedInDesignModeResult;
    }

    /// <summary>Runs the platform design-mode probes in priority order.</summary>
    /// <remarks>None of the Silverlight, WPF, or WinRT host types resolve on the supported .NET targets, so the
    /// short-circuiting branches that fire when one of those probes reports a platform are unreachable off their
    /// native hosts.</remarks>
    [ExcludeFromCodeCoverage]
    private static void RunDesignModeProbes() =>
        _ = ProbeXamlDesignProperties()
            || ProbeWpfDesignerProperties()
            || ProbeWinRtDesignMode()
            || ProbeDesignEnvironmentExecutable();

    /// <summary>Probes for the Silverlight / Windows Phone 8 design-mode indicator.</summary>
    /// <returns><see langword="true"/> if the platform was detected; otherwise <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage] // Off-platform reflection: the Silverlight/XAML type never resolves on supported targets; only the type-null guard runs.
    private static bool ProbeXamlDesignProperties()
    {
        var type = Type.GetType(XamlDesignPropertiesType, false);
        if (type is null)
        {
            return false;
        }

        var methodInfo = type.GetMethod(XamlDesignPropertiesDesignModeMethodName);
        var dependencyObject = Type.GetType(XamlControlBorderType, false);

        if (methodInfo is null || dependencyObject is null)
        {
            return true;
        }

        _cachedInDesignModeResult = (bool)(methodInfo.Invoke(null, [Activator.CreateInstance(dependencyObject)]) ?? false);
        return true;
    }

    /// <summary>Probes for the WPF designer design-mode indicator.</summary>
    /// <returns><see langword="true"/> if the platform was detected; otherwise <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage] // Off-platform reflection: the WPF type never resolves on supported targets; only the type-null guard runs.
    private static bool ProbeWpfDesignerProperties()
    {
        var type = Type.GetType(WpfDesignerPropertiesType, false);
        if (type is null)
        {
            return false;
        }

        var methodInfo = type.GetMethod(WpfDesignerPropertiesDesignModeMethod);
        var dependencyObject = Type.GetType(WpfDependencyPropertyType, false);
        if (methodInfo is null || dependencyObject is null)
        {
            return true;
        }

        _cachedInDesignModeResult = (bool)(methodInfo.Invoke(null, [Activator.CreateInstance(dependencyObject)]) ?? false);
        return true;
    }

    /// <summary>Probes for the WinRT design-mode indicator.</summary>
    /// <returns><see langword="true"/> if the platform was detected; otherwise <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage] // Off-platform reflection: the WinRT type never resolves on supported targets; only the type-null guard runs.
    private static bool ProbeWinRtDesignMode()
    {
        var type = Type.GetType(WinFormsDesignerPropertiesType, false);
        if (type is null)
        {
            return false;
        }

        _cachedInDesignModeResult = (bool)(type.GetProperty(WinFormsDesignerPropertiesDesignModeMethod)?.GetMethod?.Invoke(null, null) ?? false);
        return true;
    }

    /// <summary>Probes for a known design-environment host executable as the fallback indicator.</summary>
    /// <returns>Always <see langword="true"/>, as this is the terminal fallback probe.</returns>
    private static bool ProbeDesignEnvironmentExecutable()
    {
#if NETFRAMEWORK
        var entry = Assembly.GetEntryAssembly()?.Location;
#else
        var entry = AppContext.BaseDirectory;
#endif
        _cachedInDesignModeResult = IsDesignEnvironmentEntry(entry);

        return true;
    }

    /// <summary>Determines whether the host executable name corresponds to a known design environment.</summary>
    /// <param name="designEnvironment">The known design-environment executable name.</param>
    /// <param name="exeName">The current host executable name.</param>
    /// <returns><see langword="true"/> when the host is the supplied design environment.</returns>
    private static bool IsDesignEnvironment(string designEnvironment, string exeName) =>
#if NETFRAMEWORK
        designEnvironment.IndexOf(exeName, StringComparison.InvariantCultureIgnoreCase) != -1;
#else
        designEnvironment.Contains(exeName, StringComparison.InvariantCultureIgnoreCase);
#endif
}
