// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>
/// Wraps a android native bitmap into the splat <see cref="IBitmap"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AndroidBitmap"/> class.
/// </remarks>
/// <param name="inner">The bitmap we are wrapping.</param>
internal sealed class AndroidBitmap(Bitmap inner) : IBitmap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Is Disposed using Interlocked method")]
    private Bitmap? _inner = inner;

    /// <inheritdoc />
    public float Width => _inner?.Width ?? 0;

    /// <inheritdoc />
    public float Height => _inner?.Height ?? 0;

    /// <summary>
    /// Gets the internal bitmap we are wrapping.
    /// </summary>
    internal Bitmap Inner => _inner ?? throw new InvalidOperationException("Attempt to access a disposed Bitmap");

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target)
    {
        if (_inner is null)
        {
            return Task.CompletedTask;
        }

        var fmt = (format == CompressedBitmapFormat.Jpeg ? Bitmap.CompressFormat.Jpeg : Bitmap.CompressFormat.Png)!;
        return Task.Run(() => _inner.Compress(fmt, (int)(quality * 100), target));
    }

    /// <inheritdoc />
    public void Dispose() => Interlocked.Exchange(ref _inner, null)?.Dispose();
}
