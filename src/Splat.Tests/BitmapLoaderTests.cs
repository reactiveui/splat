using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests
{
#if ANDROID
    /// <summary>
    /// Unit Tests for the Bitmap Loader.
    /// </summary>
    public sealed class BitmapLoaderTests
    {
        /// <summary>
        /// Test to ensure the bitmap loader initializes properly.
        /// </summary>
        /// <remarks>
        /// Looks crude and pointless, but was produced to track an issue on Android between VS2017 and VS2019.
        /// </remarks>
        [Fact]
        public void ReturnsInstance()
        {
            var instance = Splat.BitmapLoader.Current;
            Assert.NotNull(instance);
        }
    }
#endif
}
