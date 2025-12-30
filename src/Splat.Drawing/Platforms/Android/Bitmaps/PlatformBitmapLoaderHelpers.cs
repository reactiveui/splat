// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.App;
using Android.Graphics;

namespace Splat;

/// <summary>
/// Internal helper methods shared between PlatformBitmapLoader implementations.
/// </summary>
internal static class PlatformBitmapLoaderHelpers
{
    /// <summary>
    /// Loads a bitmap from a stream with optional desired dimensions.
    /// </summary>
    internal static async Task<IBitmap?> LoadFromStream(Stream sourceStream, float? desiredWidth, float? desiredHeight, IEnableLogger? logger)
    {
        ArgumentExceptionHelper.ThrowIfNull(sourceStream);

        // this is a rough check to do with the termination check for #479
        ArgumentExceptionHelper.ThrowIf(sourceStream.Length < 2, "The source stream is not a valid image file.", nameof(sourceStream));

        if (!HasCorrectStreamEnd(sourceStream))
        {
            AttemptStreamByteCorrection(sourceStream, logger);
        }

        sourceStream.Position = 0;
        Bitmap? bitmap = null;

        if (desiredWidth is null || desiredHeight is null)
        {
            bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream)).ConfigureAwait(false);
        }
        else
        {
            using var opts = new BitmapFactory.Options()
            {
                OutWidth = (int)desiredWidth.Value,
                OutHeight = (int)desiredHeight.Value,
            };

            using var noPadding = new Rect(0, 0, 0, 0);
            bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream, noPadding, opts)).ConfigureAwait(true);
        }

        return bitmap switch
        {
            null => throw new IOException("Failed to load bitmap from source stream"),
            _ => bitmap.FromNative()
        };
    }

    /// <summary>
    /// Loads a bitmap from a drawable resource ID.
    /// </summary>
    internal static IBitmap? LoadFromDrawableId(int resourceId)
    {
        var res = Application.Context.Resources;
        var theme = Application.Context.Theme;

        if (res is null)
        {
            throw new InvalidOperationException("No resources found in the application.");
        }

        return GetFromDrawable(res.GetDrawable(resourceId, theme));
    }

    /// <summary>
    /// Creates a new bitmap with the specified dimensions.
    /// </summary>
    internal static IBitmap? CreateBitmap(float width, float height)
    {
        var config = Bitmap.Config.Argb8888 ?? throw new InvalidOperationException("The ARGB8888 bitmap format is unavailable");
        return Bitmap.CreateBitmap((int)width, (int)height, config).FromNative();
    }

    /// <summary>
    /// Converts an Android drawable to a Splat bitmap.
    /// </summary>
    internal static DrawableBitmap? GetFromDrawable(Android.Graphics.Drawables.Drawable? drawable) =>
        drawable is null ? null : new DrawableBitmap(drawable);

    /// <summary>
    /// Checks to make sure the last 2 bytes are as expected.
    /// issue #479 xamarin android can throw an objectdisposedexception on stream
    /// suggestion is it relates to https://forums.xamarin.com/discussion/16500/bitmap-decode-byte-array-skia-decoder-returns-false
    /// and truncated jpeg\png files.
    /// </summary>
    /// <param name="sourceStream">Input image source stream.</param>
    /// <returns>Whether the termination is correct.</returns>
    internal static bool HasCorrectStreamEnd(Stream sourceStream)
    {
        // 0-based and go back 2.
        sourceStream.Position = sourceStream.Length - 3;
        return sourceStream.ReadByte() == 0xFF
               && sourceStream.ReadByte() == 0xD9;
    }

    /// <summary>
    /// Attempts to correct stream byte termination if possible.
    /// </summary>
    internal static void AttemptStreamByteCorrection(Stream sourceStream, IEnableLogger? logger)
    {
        if (!sourceStream.CanWrite)
        {
            logger?.Log().Warn("Stream missing terminating bytes but is read only.");
        }
        else
        {
            logger?.Log().Warn("Carrying out source stream byte correction.");
            sourceStream.Position = sourceStream.Length;
            sourceStream.Write([0xFF, 0xD9]);
        }
    }
}
