// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A <see cref="IBitmapLoader"/> which will load Tizen based bitmaps.
/// </summary>
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <summary>
    /// A image the size of 100x100 pixels.
    /// This is used due to the fact Tizen does not have a empty image object.
    /// </summary>
    private static readonly byte[] _emptyImage =
        [
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 100, 0, 0, 0, 100, 8, 2, 0, 0, 0, 255, 128, 2, 3, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4,
            103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 0, 52, 73, 68, 65, 84, 120, 94, 237, 193, 1, 13, 0, 0, 0,
            194, 160, 247, 79, 109, 14, 55, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 27, 53, 117, 148, 0, 1, 4, 253, 190, 98, 0, 0, 0,
            0, 73, 69, 78, 68, 174, 66, 96, 130,
        ];

    /// <inheritdoc />
    public IBitmap Create(float width, float height) => new TizenBitmap(_emptyImage);

    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) => Task.Run<IBitmap?>(() => new TizenBitmap(((MemoryStream)sourceStream).ToArray()));

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight) => Task.Run<IBitmap?>(() => new TizenBitmap(source));
}
