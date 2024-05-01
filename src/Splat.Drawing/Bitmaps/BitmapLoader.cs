// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// This class loads and creates bitmap resources in a platform-independent.
/// way.
/// </summary>
public static class BitmapLoader
{
    // TODO: This needs to be improved once we move the "Detect in Unit Test
    // Runner" code into Splat
    private static IBitmapLoader? _current = Locator.Current.GetService<IBitmapLoader>();

    /// <summary>
    /// Gets or sets the current bitmap loader.
    /// </summary>
    /// <exception cref="BitmapLoaderException">When there is no exception loader having been found.</exception>
    [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
    public static IBitmapLoader Current
    {
        get
        {
            var ret = _current;
            return ret switch
            {
                null => throw new BitmapLoaderException("Could not find a default bitmap loader. This should never happen, your dependency resolver is broken"),
                _ => ret
            };
        }
        set => _current = value;
    }
}
