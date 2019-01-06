using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Splat
{
    /// <summary>
    /// "Implement" this interface in your class to get access to the Log()
    /// Mixin, which will give you a Logger that includes the class name in the
    /// log.
    /// </summary>
    [SuppressMessage("Design", "CA1040: Avoid empty interfaces", Justification = "Deliberate use")]
    [ComVisible(false)]
    public interface IEnableLogger
    {
    }
}
