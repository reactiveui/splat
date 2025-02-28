// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A manaager which will generate a <see cref="IFullLogger"/> for the specified type.
/// </summary>
public interface ILogManager
{
    /// <summary>
    /// Generate a <see cref="IFullLogger"/> for the specified type.
    /// </summary>
    /// <param name="type">The type to generate the logger for.</param>
    /// <returns>The <see cref="IFullLogger"/> for the specified type.</returns>
    IFullLogger GetLogger(Type type);
}
