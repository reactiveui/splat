using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    internal enum BindingFlags
    {
        Public = 1,
        NonPublic = 1 << 1,
        Instance = 1 << 2,
        Static = 1 << 3,
        FlattenHierarchy = 1 << 4
    }
}
