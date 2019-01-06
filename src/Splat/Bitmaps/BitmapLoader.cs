using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// This class loads and creates bitmap resources in a platform-independent.
    /// way.
    /// </summary>
    public static class BitmapLoader
    {
        // TODO: This needs to be improved once we move the "Detect in Unit Test
        // Runner" code into Splat
        private static IBitmapLoader _Current = AssemblyFinder.AttemptToLoadType<IBitmapLoader>("Splat.PlatformBitmapLoader");

        /// <summary>
        /// Gets or sets the current bitmap loader.
        /// </summary>
        /// <exception cref="BitmapLoaderException">When there is no exception loader having been found.</exception>
        [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
        public static IBitmapLoader Current
        {
            get
            {
                var ret = _Current;
                if (ret == null)
                {
                    throw new BitmapLoaderException("Could not find a default bitmap loader. This should never happen, your dependency resolver is broken");
                }

                return ret;
            }
            set => _Current = value;
        }
    }
}
