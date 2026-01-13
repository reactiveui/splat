// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Splat;

/// <summary>
/// Defines an interface for a bitmap image that supports retrieving its dimensions and saving the image in a compressed
/// format. Every platform provides FromNative and ToNative methods to convert this object to the platform-specific versions.
/// </summary>
/// <remarks>Implementations of this interface provide access to bitmap image data and allow saving the image to a
/// stream in various compressed formats. The interface inherits from <see cref="IDisposable"/>, so implementations may
/// hold unmanaged resources that should be released when no longer needed.</remarks>
public interface IBitmap : IDisposable
{
    /// <summary>
    /// Gets the width in pixel units (depending on platform).
    /// </summary>
    float Width { get; }

    /// <summary>
    /// Gets the height in pixel units (depending on platform).
    /// </summary>
    float Height { get; }

    /// <summary>
    /// Saves an image to a target stream.
    /// </summary>
    /// <param name="format">The format to save the image in.</param>
    /// <param name="quality">If JPEG is specified, this is a quality
    /// factor between 0.0 and 1.0f where 1.0f is the best quality.</param>
    /// <param name="target">The target stream to save to.</param>
    /// <returns>A signal indicating the Save has completed.</returns>
    Task Save(CompressedBitmapFormat format, float quality, Stream target);
}
