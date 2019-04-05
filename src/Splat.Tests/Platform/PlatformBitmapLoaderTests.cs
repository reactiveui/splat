using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.Platform
{
    /// <summary>
    /// Unit tests for the platform specific Bitmap loader.
    /// </summary>
    public sealed class PlatformBitmapLoaderTests
    {
#if ANDROID
        /// <summary>
        /// Checks to ensure an instance is returned.
        /// </summary>
        [Fact]
        public void GetDrawableList_ReturnsResults()
        {
           var drawableList = Splat.PlatformBitmapLoader.GetDrawableList();
           Assert.NotNull(drawableList);
           Assert.True(drawableList.Count > 0);
        }
#endif
    }
}
