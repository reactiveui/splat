// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Null Service Type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NullServiceType"/> class.
/// </remarks>
/// <param name="factory">The value factory.</param>
public class NullServiceType(Func<object?> factory)
{
    /// <summary>
    /// Gets the Factory.
    /// </summary>
    public Func<object?> Factory { get; } = factory;
}
