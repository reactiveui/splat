// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for converting between <see cref="IBitmap"/> and the Skia bitmap behind it.</summary>
public sealed class BitmapMixinsTests
{
    /// <summary>The width of the bitmaps the tests convert.</summary>
    private const int BitmapWidth = 20;

    /// <summary>The height of the bitmaps the tests convert.</summary>
    private const int BitmapHeight = 10;

    /// <summary>Verifies that a Skia bitmap becomes an <see cref="IBitmap"/> of the same size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromNative_WrapsTheSkiaBitmap()
    {
        using var bitmap = TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight).FromNative();

        using (Assert.Multiple())
        {
            await Assert.That(bitmap.Width).IsEqualTo((float)BitmapWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)BitmapHeight);
        }
    }

    /// <summary>Verifies that the conversion hands back the very bitmap that was wrapped.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToNative_ReturnsTheWrappedSkiaBitmap()
    {
        var native = TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight);
        using var bitmap = native.FromNative();

        await Assert.That(bitmap.ToNative()).IsSameReferenceAs(native);
    }

    /// <summary>Verifies that a released bitmap says so rather than handing out a dangling reference.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToNative_AfterDispose_Throws()
    {
        var bitmap = TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight).FromNative();
        bitmap.Dispose();

        await Assert.That(bitmap.ToNative).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that a bitmap has to be supplied.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToNative_WithoutABitmap_Throws()
    {
        const IBitmap bitmap = null!;

        await Assert.That(static () => bitmap.ToNative()).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that a bitmap from a different loader is rejected rather than reinterpreted.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToNative_WithAForeignBitmap_Throws()
    {
        using var bitmap = new ForeignBitmap();

        await Assert.That(() => bitmap.ToNative()).Throws<InvalidCastException>();
    }

    /// <summary>An <see cref="IBitmap"/> from somewhere other than this package.</summary>
    private sealed class ForeignBitmap : IBitmap
    {
        /// <inheritdoc />
        public float Width => 0;

        /// <inheritdoc />
        public float Height => 0;

        /// <inheritdoc />
        public Task Save(CompressedBitmapFormat format, float quality, Stream target) => Task.CompletedTask;

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
