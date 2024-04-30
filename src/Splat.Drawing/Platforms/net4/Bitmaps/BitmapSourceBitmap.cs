// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>
/// A bitmap that wraps a <see cref="BitmapSourceBitmap"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BitmapSourceBitmap"/> class.
/// </remarks>
/// <param name="bitmap">The platform native bitmap we are wrapping.</param>
internal sealed class BitmapSourceBitmap(BitmapSource bitmap) : IBitmap
{
    /// <inheritdoc />
    public float Width => (float)(Inner?.Width ?? 0f);

    /// <inheritdoc />
    public float Height => (float)(Inner?.Height ?? 0f);

    /// <summary>
    /// Gets the platform <see cref="BitmapSource"/>.
    /// </summary>
    public BitmapSource? Inner { get; private set; } = bitmap;

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target) => Inner switch
    {
        null => Task.CompletedTask,
        _ => Task.Run(() =>
        {
            var encoder = format == CompressedBitmapFormat.Jpeg ?
                new JpegBitmapEncoder() { QualityLevel = (int)(quality * 100.0f) } :
                (BitmapEncoder)new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(Inner));
            encoder.Save(target);
        })
    };

    /// <inheritdoc />
    public void Dispose() => Inner = null;
}
