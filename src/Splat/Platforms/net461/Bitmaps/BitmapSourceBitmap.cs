using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// A bitmap that wraps a <see cref="BitmapSourceBitmap"/>.
    /// </summary>
    internal sealed class BitmapSourceBitmap : IBitmap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapSourceBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">The platform native bitmap we are wrapping.</param>
        public BitmapSourceBitmap(BitmapSource bitmap)
        {
            Inner = bitmap;
        }

        /// <inheritdoc />
        public float Width => (float)Inner.Width;

        /// <inheritdoc />
        public float Height => (float)Inner.Height;

        /// <summary>
        /// Gets the platform <see cref="BitmapSource"/>.
        /// </summary>
        public BitmapSource Inner { get; private set; }

        /// <inheritdoc />
        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() =>
            {
                var encoder = format == CompressedBitmapFormat.Jpeg ?
                    new JpegBitmapEncoder() { QualityLevel = (int)(quality * 100.0f) } :
                    (BitmapEncoder)new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(Inner));
                encoder.Save(target);
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Inner = null;
        }
    }
}
