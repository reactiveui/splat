// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>Wraps a android native bitmap into the splat <see cref="IBitmap"/>.</summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AndroidBitmap"/> class.
/// </remarks>
/// <param name="inner">The bitmap we are wrapping.</param>
internal sealed class AndroidBitmap(Bitmap inner) : IBitmap
{
    /// <summary>The scale factor used to convert a normalized quality (0-1) into a percentage (0-100).</summary>
    private const int QualityPercentageScale = 100;

    /// <summary>The wrapped Android bitmap; set to <see langword="null"/> once disposed.</summary>
    private Bitmap? _inner = inner;

    /// <inheritdoc />
    public float Width => Volatile.Read(ref _inner)?.Width ?? 0;

    /// <inheritdoc />
    public float Height => Volatile.Read(ref _inner)?.Height ?? 0;

    /// <summary>Gets the internal bitmap we are wrapping.</summary>
    internal Bitmap Inner => Volatile.Read(ref _inner) ?? throw new InvalidOperationException("Attempt to access a disposed Bitmap");

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target)
    {
        var inner = Volatile.Read(ref _inner);
        if (inner is null)
        {
            return Task.CompletedTask;
        }

        var fmt = (format == CompressedBitmapFormat.Jpeg ? Bitmap.CompressFormat.Jpeg : Bitmap.CompressFormat.Png)!;
        return Task.Run(() => inner.Compress(fmt, (int)(quality * QualityPercentageScale), target));
    }

    /// <inheritdoc />
    public void Dispose() => Interlocked.Exchange(ref _inner, null)?.Dispose();
}
