// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics.Drawables;

namespace Splat;

/// <summary>Provides a bitmap implementation that wraps an existing Drawable object for rendering operations.</summary>
/// <remarks>This class is intended for internal use where a Drawable needs to be presented as an IBitmap. The
/// wrapped Drawable is disposed when this object is disposed. Saving the bitmap is not supported and will throw a
/// NotSupportedException.</remarks>
/// <param name="inner">The Drawable instance to be wrapped and exposed as a bitmap. Cannot be null.</param>
internal sealed class DrawableBitmap(Drawable inner) : IBitmap
{
    /// <summary>The wrapped Android drawable; set to <see langword="null"/> once disposed.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Is Disposed using Interlocked method")]
    private Drawable? _inner = inner;

    /// <inheritdoc />
    public float Width => Inner.IntrinsicWidth;

    /// <inheritdoc />
    public float Height => Inner.IntrinsicHeight;

    /// <summary>Gets the internal Drawable we are wrapping.</summary>
    internal Drawable Inner => _inner ?? throw new InvalidOperationException("Attempting to retrieve a disposed bitmap");

    /// <summary>Saving a wrapped Drawable is not supported and always throws a <see cref="NotSupportedException"/>.</summary>
    /// <param name="format">The compressed bitmap format to save in.</param>
    /// <param name="quality">The compression quality to use when saving.</param>
    /// <param name="target">The stream to write the saved bitmap to.</param>
    /// <returns>A task representing the save operation. This method never completes normally and always throws.</returns>
    public Task Save(CompressedBitmapFormat format, float quality, Stream target) => throw new NotSupportedException("You can't save resources");

    /// <summary>Disposes the wrapped <see cref="Drawable"/>, releasing its resources.</summary>
    public void Dispose()
    {
        var disp = Interlocked.Exchange(ref _inner, null);
        disp?.Dispose();
    }
}
