// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics.Drawables;

namespace Splat;

/// <summary>
/// Initializes a new instance of the <see cref="DrawableBitmap"/> class.
/// </summary>
/// <param name="inner">The drawable bitmap to wrap.</param>
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
