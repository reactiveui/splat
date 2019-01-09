// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat
{
    /// <summary>
    /// A helper class which detect if we are currently running via a unit test or design mode.
    /// </summary>
    public static class ModeDetector
    {
        private const string DetectPlatformDetectorName = "Splat.PlatformModeDetector";
        private const string XamlDesignPropertiesType = "System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        private const string XamlControlBorderType = "System.Windows.Controls.Border, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        private const string XamlDesignPropertiesDesignModeMethodName = "GetIsInDesignMode";
        private const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        private const string WpfDesignerPropertiesDesignModeMethod = "GetIsInDesignMode";
        private const string WpfDependencyPropertyType = "System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        private const string WinFormsDesignerPropertiesType = "Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime";
        private const string WinFormsDesignerPropertiesDesignModeMethod = "DesignModeEnabled";

        private static bool? cachedInUnitTestRunnerResult;
        private static bool? cachedInDesignModeResult;

        /// <summary>
        /// Initializes static members of the <see cref="ModeDetector"/> class.
        /// </summary>
        static ModeDetector()
        {
            Current = AssemblyFinder.AttemptToLoadType<IModeDetector>(DetectPlatformDetectorName);
        }

        /// <summary>
        /// Gets or sets the current mode detector set.
        /// </summary>
        private static IModeDetector Current { get; set; }

        /// <summary>
        /// Overrides the mode detector with one of your own provided ones.
        /// </summary>
        /// <param name="modeDetector">The mode detector to use.</param>
        public static void OverrideModeDetector(IModeDetector modeDetector)
        {
            Current = modeDetector;
            cachedInDesignModeResult = null;
            cachedInUnitTestRunnerResult = null;
        }

        /// <summary>
        /// Gets a value indicating whether we are currently running from a unit test.
        /// </summary>
        /// <returns>If we are currently running from a unit test.</returns>
        public static bool InUnitTestRunner()
        {
            if (cachedInUnitTestRunnerResult.HasValue)
            {
                return cachedInUnitTestRunnerResult.Value;
            }

            if (Current != null)
            {
                cachedInUnitTestRunnerResult = Current.InUnitTestRunner();
                if (cachedInUnitTestRunnerResult.HasValue)
                {
                    return cachedInUnitTestRunnerResult.Value;
                }
            }

            // We have no sane platform-independent way to detect a unit test
            // runner :-/
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether we are currently running from within a GUI design editor.
        /// </summary>
        /// <returns>If we are currently running from design mode.</returns>
        public static bool InDesignMode()
        {
            if (cachedInDesignModeResult.HasValue)
            {
                return cachedInDesignModeResult.Value;
            }

            if (Current != null)
            {
                cachedInDesignModeResult = Current.InDesignMode();
                if (cachedInDesignModeResult.HasValue)
                {
                    return cachedInDesignModeResult.Value;
                }
            }

            // Check Silverlight / WP8 Design Mode
            var type = Type.GetType(XamlDesignPropertiesType, false);
            if (type != null)
            {
                var mInfo = type.GetMethod(XamlDesignPropertiesDesignModeMethodName);
                var dependencyObject = Type.GetType(XamlControlBorderType, false);

                if (dependencyObject != null)
                {
                    cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            }
            else if ((type = Type.GetType(WpfDesignerPropertiesType, false)) != null)
            {
                // loaded the assembly, could be .net
                var mInfo = type.GetMethod(WpfDesignerPropertiesDesignModeMethod);
                var dependencyObject = Type.GetType(WpfDependencyPropertyType, false);
                if (dependencyObject != null)
                {
                    cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            }
            else if ((type = Type.GetType(WinFormsDesignerPropertiesType, false)) != null)
            {
                // check WinRT next
                cachedInDesignModeResult = (bool)type.GetProperty(WinFormsDesignerPropertiesDesignModeMethod).GetMethod.Invoke(null, null);
            }
            else
            {
                cachedInDesignModeResult = false;
            }

            return cachedInDesignModeResult.GetValueOrDefault();
        }
    }
}
