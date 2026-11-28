// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;
using SkiaSharp;
using Splat;
using Splat.SkiaSharp;

namespace DispatchAdapters;

/// <summary>Draws a dispatch label and encodes it for a thumbnail view.</summary>
internal static class ParcelLabel
{
    /// <summary>Creates a blue label and checks its color and size after encoding and loading a smaller copy.</summary>
    /// <param name="resolver">The resolver that supplies the bitmap loader.</param>
    /// <param name="token">Cancels waiting for encoding and decoding the label.</param>
    /// <returns>Completion after checking the thumbnail.</returns>
    /// <exception cref="InvalidOperationException">The loader or the thumbnail is unavailable.</exception>
    internal static async Task CheckThumbnailAsync(IDependencyResolver resolver, CancellationToken token)
    {
        const int LabelSize = 32;
        const int PreviewSize = 8;
        CheckLabelLayout(LabelSize, PreviewSize);
        resolver.UseSkiaSharpBitmapLoader();
        var loader = resolver.GetService<IBitmapLoader>() as SkiaBitmapLoader ?? throw new InvalidOperationException("Register the Skia bitmap loader.");
        using var label = loader.Create(LabelSize, LabelSize);
        var color = SplatColor.SteelBlue;
        var ink = new SKColor(color.R, color.G, color.B, color.A);
        label.ToNative().Erase(ink);
        await using MemoryStream encoded = new();
        await label.Save(CompressedBitmapFormat.Png, 1, encoded).WaitAsync(token);
        using var preview = loader.LoadFromMemory(encoded.ToArray(), PreviewSize, PreviewSize, token)
            ?? throw new InvalidOperationException("The label could not be loaded.");
        var pixels = preview.ToNative();
        if (pixels.Width == PreviewSize && pixels.Height == PreviewSize && pixels.GetPixel(0, 0) == ink)
        {
            return;
        }

        throw new InvalidOperationException("The thumbnail should keep the label color at the requested size.");
    }

    /// <summary>Reserves a square for the barcode and caches its center for each label size.</summary>
    /// <param name="labelSize">The width and height of the unscaled label.</param>
    /// <param name="previewSize">The width and height of the requested preview.</param>
    /// <exception cref="InvalidOperationException">The scaled barcode center or cache entry is incorrect.</exception>
    private static void CheckLabelLayout(int labelSize, int previewSize)
    {
        const int CachedSizes = 2;
        var centers = new MemoizingMRUCache<int, PointF>(
            static (size, _) =>
            {
                const float BarcodeFraction = 0.5F;
                RectangleF label = new(0, 0, size, size);
                var (barcode, _) = label.Divide(size * BarcodeFraction, RectEdge.Left);
                return barcode.Center();
            },
            CachedSizes);
        var center = centers.Get(labelSize);
        var scale = (float)previewSize / labelSize;
        var previewCenter = center.ScaledBy(scale);
        if (!centers.TryGet(labelSize, out var cached) || cached != center
            || previewCenter != centers.Get(previewSize))
        {
            throw new InvalidOperationException("The preview should keep the barcode in the same relative position.");
        }

        centers.Invalidate(labelSize);
    }
}
