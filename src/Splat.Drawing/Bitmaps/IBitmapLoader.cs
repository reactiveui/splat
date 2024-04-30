// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Splat;

/// <summary>
/// Represents the platform-specific image loader class. Unless you are
/// testing image loading, you don't usually need to implement this.
/// </summary>
public interface IBitmapLoader
{
    /// <summary>
    /// Loads a bitmap from a byte stream.
    /// </summary>
    /// <param name="sourceStream">The stream to load the image from.</param>
    /// <param name="desiredWidth">The desired width of the image.</param>
    /// <param name="desiredHeight">The desired height of the image.</param>
    /// <returns>A future result representing the loaded image.</returns>
    Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight);

    /// <summary>
    /// Loads from the application's resources (i.e. from bundle on Cocoa,
    /// from Pack URIs on Windows, etc).
    /// </summary>
    /// <param name="source">The source resource, as a relative path.</param>
    /// <param name="desiredWidth">Desired width.</param>
    /// <param name="desiredHeight">Desired height.</param>
    /// <returns>A future result representing the loaded image.</returns>
    Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight);

    /// <summary>
    /// Creates an empty bitmap of the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the canvas.</param>
    /// <param name="height">The height of the canvas.</param>
    /// <returns>A new image. Use ToNative() to convert this to a native bitmap.</returns>
    IBitmap? Create(float width, float height);
}
