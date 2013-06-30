using System;

namespace Splat
{
    public interface IModeDetector
    {
        bool? InUnitTestRunner();
        bool? InDesignMode();
    }

    public static class ModeDetector
    {
        static IModeDetector current { get; set; }

        public static void OverrideModeDetector(IModeDetector modeDetector)
        {
            current = modeDetector;
        }

        public static bool InUnitTestRunner() 
        {
            var ret = default(bool?);

            if (current != null) {
                ret = current.InUnitTestRunner();
                if (ret != null) return ret.Value;
            }

            // We have no sane platform-independent way to detect a unit test 
            // runner :-/
            return false;
        }
                
        static bool? cachedInDesignModeResult;
        public static bool InDesignMode()
        {
            var ret = default(bool?);

            if (current != null) {
                ret = current.InDesignMode();
                if (ret != null) return ret.Value;
            }

            if (cachedInDesignModeResult != null) return cachedInDesignModeResult.Value;

            // Check Silverlight / WP8 Design Mode
            var type = Type.GetType("System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", false);
            if (type != null) {
                var mInfo = type.GetMethod("GetIsInDesignMode");
                var dependencyObject = Type.GetType("System.Windows.Controls.Border, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", false);

                if (dependencyObject != null) {
                    cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            } else if((type = Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false)) != null) {
                // loaded the assembly, could be .net 
                var mInfo = type.GetMethod("GetIsInDesignMode");
                Type dependencyObject = Type.GetType("System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
                if (dependencyObject != null) {
                    cachedInDesignModeResult = (bool)mInfo.Invoke(null, new object[] { Activator.CreateInstance(dependencyObject) });
                }
            } else if ((type = Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime", false)) != null) {
                // check WinRT next
                cachedInDesignModeResult = (bool)type.GetProperty("DesignModeEnabled").GetMethod.Invoke(null, null);
            } else {
                cachedInDesignModeResult = false;
            }

            return cachedInDesignModeResult.GetValueOrDefault();
        }
    }
}