// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Specifies that compressed bitmap format.
/// </summary>
public enum CompressedBitmapFormat
{
    /// <summary>
    /// Store the bitmap as a PNG format.
    /// </summary>
    Png,

    /// <summary>
    /// Store the bitmap as a JPEG format.
    /// </summary>
    Jpeg,
}
