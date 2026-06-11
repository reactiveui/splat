// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Specifies selective component overrides for <see cref="RectangleMathExtensions.Copy"/>.</summary>
/// <remarks>Any property left <see langword="null"/> leaves the corresponding component of the source rectangle unchanged.</remarks>
public readonly record struct RectangleCopyOptions
{
    /// <summary>Gets the X-coordinate override; <see langword="null"/> keeps the original.</summary>
    public float? X { get; init; }

    /// <summary>Gets the Y-coordinate override; <see langword="null"/> keeps the original. Cannot be combined with <see cref="Top"/>.</summary>
    public float? Y { get; init; }

    /// <summary>Gets the width override; <see langword="null"/> keeps the original.</summary>
    public float? Width { get; init; }

    /// <summary>Gets the height override; <see langword="null"/> keeps the original. Cannot be combined with <see cref="Bottom"/>.</summary>
    public float? Height { get; init; }

    /// <summary>Gets the top (Y-coordinate) override; <see langword="null"/> keeps the original. Cannot be combined with <see cref="Y"/>.</summary>
    public float? Top { get; init; }

    /// <summary>Gets the bottom-edge override relative to Y; <see langword="null"/> keeps the original. Cannot be combined with <see cref="Height"/>.</summary>
    public float? Bottom { get; init; }
}
