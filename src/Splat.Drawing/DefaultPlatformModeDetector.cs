// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;

#if NETFX_CORE
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
#endif

namespace Splat
{
    /// <summary>
    /// Detects if we are in design mode or unit test mode based on the current platform.
    /// </summary>
    public class DefaultPlatformModeDetector : IPlatformModeDetector
    {
#if !NETFX_CORE
        private const string XamlDesignPropertiesType = "System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        private const string XamlControlBorderType = "System.Windows.Controls.Border, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        private const string XamlDesignPropertiesDesignModeMethodName = "GetIsInDesignMode";
        private const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        private const string WpfDesignerPropertiesDesignModeMethod = "GetIsInDesignMode";
        private const string WpfDependencyPropertyType = "System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        private const string WinFormsDesignerPropertiesType = "Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime";
        private const string WinFormsDesignerPropertiesDesignModeMethod = "DesignModeEnabled";

        private static bool? _cachedInDesignModeResult;
#endif

        /// <inheritdoc />
        public bool? InDesignMode()
        {
#if NETFX_CORE
            return DesignMode.DesignModeEnabled;
#else
            if (_cachedInDesignModeResult.HasValue)
            {
                return _cachedInDesignModeResult.Value;
            }

            // Check Silverlight / WP8 Design Mode
            var type = Type.GetType(XamlDesignPropertiesType, false);
            if (type != null)
            {
                var mInfo = type.GetMethod(XamlDesignPropertiesDesignModeMethodName);
                var dependencyObject = Type.GetType(XamlControlBorderType, false);

                if (dependencyObject != null)
                {
                    _cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            }
            else if ((type = Type.GetType(WpfDesignerPropertiesType, false)) != null)
            {
                // loaded the assembly, could be .net
                var mInfo = type.GetMethod(WpfDesignerPropertiesDesignModeMethod);
                var dependencyObject = Type.GetType(WpfDependencyPropertyType, false);
                if (dependencyObject != null)
                {
                    _cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            }
            else if ((type = Type.GetType(WinFormsDesignerPropertiesType, false)) != null)
            {
                // check WinRT next
                _cachedInDesignModeResult = (bool)type.GetProperty(WinFormsDesignerPropertiesDesignModeMethod).GetMethod.Invoke(null, null);
            }
            else
            {
                var designEnvironments = new[] { "BLEND.EXE", "XDESPROC.EXE" };

                var entry = Assembly.GetEntryAssembly();
                if (entry != null)
                {
                    var exeName = new FileInfo(entry.Location).Name;

                    if (designEnvironments.Any(x =>
                        x.IndexOf(exeName, StringComparison.InvariantCultureIgnoreCase) != -1))
                    {
                        _cachedInDesignModeResult = true;
                    }
                }
            }

            _cachedInDesignModeResult = false;

            return _cachedInDesignModeResult;
#endif
        }
    }
}
