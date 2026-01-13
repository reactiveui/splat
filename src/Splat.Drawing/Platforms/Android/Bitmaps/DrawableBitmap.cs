// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics.Drawables;

namespace Splat;

/// <summary>
/// Provides a bitmap implementation that wraps an existing Drawable object for rendering operations.
/// </summary>
/// <remarks>This class is intended for internal use where a Drawable needs to be presented as an IBitmap. The
/// wrapped Drawable is disposed when this object is disposed. Saving the bitmap is not supported and will throw a
/// NotSupportedException.</remarks>
/// <param name="inner">The Drawable instance to be wrapped and exposed as a bitmap. Cannot be null.</param>
internal sealed class DrawableBitmap(Drawable inner) : IBitmap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Is Disposed using Interlocked method")]
    private Drawable? _inner = inner;

    /// <inheritdoc />
    public float Width => Inner.IntrinsicWidth;

    /// <inheritdoc />
    public float Height => Inner.IntrinsicHeight;

    /// <summary>
    /// Gets the internal Drawable we are wrapping.
    /// </summary>
    internal Drawable Inner => _inner ?? throw new InvalidOperationException("Attempting to retrieve a disposed bitmap");

    public Task Save(CompressedBitmapFormat format, float quality, Stream target) => throw new NotSupportedException("You can't save resources");

    public void Dispose()
    {
        var disp = Interlocked.Exchange(ref _inner, null);
        disp?.Dispose();
    }
}
