using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Specifies that compressed bitmap format.
    /// </summary>
    public enum CompressedBitmapFormat
    {
        /// <summary>
        /// Store the bitmap as a PNG format.
        /// </summary>
        Png,

        /// <summary>
        /// Store the bitmap as a JPEG format.
        /// </summary>
        Jpeg,
    }
}
