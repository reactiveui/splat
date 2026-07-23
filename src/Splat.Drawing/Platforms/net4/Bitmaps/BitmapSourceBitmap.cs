// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>Provides an implementation of the IBitmap interface that wraps a WPF BitmapSource.</summary>
/// <remarks>This class enables interoperability with WPF imaging APIs by exposing a BitmapSource as an IBitmap.
/// The wrapped BitmapSource can be accessed via the Inner property. After disposal, the Inner property is set to null
/// and the instance should not be used.</remarks>
/// <param name="bitmap">The platform-specific BitmapSource to wrap. Cannot be null.</param>
internal sealed class BitmapSourceBitmap(BitmapSource bitmap) : IBitmap
{
    /// <summary>The scale that converts a normalized 0-1 quality into a 0-100 JPEG quality level.</summary>
    private const float QualityPercentScale = 100.0F;

    /// <inheritdoc />
    public float Width => (float)(Inner?.Width ?? 0F);

    /// <inheritdoc />
    public float Height => (float)(Inner?.Height ?? 0F);

    /// <summary>Gets the platform <see cref="BitmapSource"/>.</summary>
    internal BitmapSource? Inner { get; private set; } = bitmap;

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target) => Inner switch
    {
        null => Task.CompletedTask,
        _ => Task.Run(() =>
        {
            var encoder = format == CompressedBitmapFormat.Jpeg
                ? new JpegBitmapEncoder { QualityLevel = (int)(quality * QualityPercentScale) }
                : (BitmapEncoder)new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(Inner));
            encoder.Save(target);
        })
    };

    /// <inheritdoc />
    public void Dispose() => Inner = null;
}
