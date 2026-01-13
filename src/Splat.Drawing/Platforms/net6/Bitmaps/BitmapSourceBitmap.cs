// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>
/// Provides an implementation of the <see cref="IBitmap"/> interface that wraps a WPF <see cref="BitmapSource"/>
/// object.
/// </summary>
/// <remarks>This class enables interoperability between WPF imaging and APIs that consume the <see
/// cref="IBitmap"/> abstraction. The wrapped <see cref="BitmapSource"/> can be accessed via the <see cref="Inner"/>
/// property.</remarks>
/// <param name="bitmap">The <see cref="BitmapSource"/> instance to wrap. Cannot be null.</param>
internal sealed class BitmapSourceBitmap(BitmapSource bitmap) : IBitmap
{
    /// <inheritdoc />
    public float Width => (float)(Inner?.Width ?? 0);

    /// <inheritdoc />
    public float Height => (float)(Inner?.Height ?? 0);

    /// <summary>
    /// Gets the platform <see cref="BitmapSource"/>.
    /// </summary>
    public BitmapSource? Inner { get; private set; } = bitmap;

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target)
    {
        ArgumentExceptionHelper.ThrowIfNull(target);

        return Task.Run(() =>
        {
            var encoder = format == CompressedBitmapFormat.Jpeg ?
                new JpegBitmapEncoder { QualityLevel = (int)(quality * 100.0f) } :
                (BitmapEncoder)new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(Inner));
            encoder.Save(target);
        });
    }

    /// <inheritdoc />
    public void Dispose() => Inner = null;
}
