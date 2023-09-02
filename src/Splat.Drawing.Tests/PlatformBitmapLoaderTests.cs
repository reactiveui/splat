// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Platform;

/// <summary>
/// Unit tests for the platform specific Bitmap loader.
/// </summary>
public sealed class PlatformBitmapLoaderTests
{
#if !NETSTANDARD && !NETCOREAPP2
    /// <summary>
    /// Check to ensure an instance is returned.
    /// </summary>
    [Fact]
    public void Constructor_ReturnsInstance()
    {
        var instance = new Splat.PlatformBitmapLoader();
        Assert.NotNull(instance);
    }
#endif

#if ANDROID
    /// <summary>
    /// Checks to ensure a dynamic assembly behaves on android.
    /// </summary>
    /// <remarks>
    /// Introduced because of Splat #330.
    /// </remarks>
    [Fact]
    public void GetTypesFromAssembly_ReturnsResultsOnDynamicAssembly()
    {
        var name = new AssemblyName("SomeRandomDynamicAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            name, AssemblyBuilderAccess.Run);

        // can't test with a logger, as it invokes the splat init, which puts the test in a false state as it will init the platform bitmap loader
        var drawableList = Splat.PlatformBitmapLoader.GetTypesFromAssembly(assemblyBuilder, null);
        Assert.NotNull(drawableList);
        Assert.Equal(0, drawableList.Length);
    }

    /// <summary>
    /// Checks to ensure a list of drawable items is returned.
    /// </summary>
    [Fact]
    public void GetDrawableList_ReturnsResults()
    {
        // can't test with a logger, as it invokes the splat init, which puts the test in a false state as it will init the platform bitmap loader
        var drawableList = Splat.PlatformBitmapLoader.GetDrawableList(null);
        Assert.NotNull(drawableList);
        Assert.True(drawableList.Count > 0);
    }
#endif
}
