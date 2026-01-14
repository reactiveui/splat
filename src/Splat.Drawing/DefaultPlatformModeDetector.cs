// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
/// environments, including WPF and WinForms, by checking for known design mode indicators. The detection
/// result is cached by the PlatformModeDetector class for performance. Thread safety is not guaranteed.</remarks>
public class DefaultPlatformModeDetector : IPlatformModeDetector
{
#if !NETFX_CORE
    private const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
    private const string WpfDesignerPropertiesDesignModeProperty = "IsInDesignTool";
    private const string LicenseManagerType = "System.ComponentModel.LicenseManager, System.ComponentModel.TypeConverter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
    private const string LicenseUsageModeProperty = "UsageMode";
#endif

    /// <inheritdoc />
    public bool? InDesignMode()
    {
#if NETFX_CORE
        return false;
#else
        // Check WPF Design Mode - use static IsInDesignTool property instead of GetIsInDesignMode
        // This properly detects design mode for nested UserControls
        var type = Type.GetType(WpfDesignerPropertiesType, false);
        if (type is not null)
        {
            var propInfo = type.GetProperty(WpfDesignerPropertiesDesignModeProperty, BindingFlags.Public | BindingFlags.Static);
            if (propInfo is not null)
            {
                return (bool)(propInfo.GetValue(null) ?? false);
            }
        }

        // Fallback: Check LicenseManager.UsageMode for broader compatibility
        // This handles WinForms and other scenarios where platform-specific design mode detection isn't available
        if ((type = Type.GetType(LicenseManagerType, false)) is not null)
        {
            var propInfo = type.GetProperty(LicenseUsageModeProperty, BindingFlags.Public | BindingFlags.Static);
            if (propInfo is not null)
            {
                var usageMode = propInfo.GetValue(null);
                // LicenseUsageMode.Designtime = 1
                if (usageMode is not null && (int)usageMode == 1)
                {
                    return true;
                }
            }
        }

        // Fallback: Check process name for known designers
        var designEnvironments = new[] { "DEVENV.EXE", "XDESSPROC.EXE", "BLEND.EXE", "XDESPROC.EXE" };
#if NETFRAMEWORK || TIZEN
        var entry = Assembly.GetEntryAssembly()?.Location;
#else
        var entry = System.AppContext.BaseDirectory;
#endif
        if (entry is not null)
        {
            var exeName = new FileInfo(entry).Name;

            foreach (var designEnv in designEnvironments)
            {
#if NETFRAMEWORK || TIZEN
                if (designEnv.IndexOf(exeName, StringComparison.InvariantCultureIgnoreCase) != -1)
#else
                if (designEnv.Contains(exeName, StringComparison.InvariantCultureIgnoreCase))
#endif
                {
                    return true;
                }
            }
        }

        // No design mode detected
        return false;
#endif
    }
}
