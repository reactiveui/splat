// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Splat;

/// <summary>
/// Represents a bitmap image that was loaded via a ViewModel. Every platform
/// provides FromNative and ToNative methods to convert this object to the
/// platform-specific versions.
/// </summary>
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
