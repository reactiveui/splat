using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Detects if unit tests or design mode are currently running for the current application or library.
    /// </summary>
    public interface IModeDetector
    {
        /// <summary>
        /// Gets a value indicating whether the current library or application is running through a unit test.
        /// </summary>
        /// <returns>If we are currently running in a unit test.</returns>
        bool? InUnitTestRunner();

        /// <summary>
        /// Gets a value indicating whether the current library or application is running in a GUI design mode tool.
        /// </summary>
        /// <returns>If we are currently running in design mode.</returns>
        bool? InDesignMode();
    }
}
