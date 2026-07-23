// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Caches typeof(T) per generic instantiation to avoid repeated evaluation.
/// Particularly beneficial in AOT scenarios and tight loops.
/// </summary>
/// <typeparam name="T">The type to cache.</typeparam>
internal static class TypeCache<T>
{
    /// <summary>Cached Type instance for T.</summary>
    internal static readonly Type Type = typeof(T);
}
