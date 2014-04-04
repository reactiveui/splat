using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    // NB: This code isn't included in any of the platform-specific libraries
    // it's just to give the PCL something to link against
    static class PlatformRegistrations
    {
        internal static void Register(IMutableDependencyResolver This)
        {
        }
    }
}
