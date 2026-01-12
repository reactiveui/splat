// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Platform;

public sealed class PlatformBitmapLoaderTests
{
#if !IS_SHARED_NET
    /// <summary>
    /// Check to ensure an instance is returned.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ReturnsInstance()
    {
        var instance = new PlatformBitmapLoader();
        await Assert.That(instance).IsNotNull();
    }
#endif

#if ANDROID
    /// <summary>
    /// Checks to ensure a dynamic assembly behaves on android.
    /// </summary>
    /// <remarks>
    /// Introduced because of Splat #330.
    /// </remarks>
    [Test]
    public void GetTypesFromAssembly_ReturnsResultsOnDynamicAssembly()
    {
        var name = new AssemblyName("SomeRandomDynamicAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            name, AssemblyBuilderAccess.Run);

        // can't test with a logger, as it invokes the splat init, which puts the test in a false state as it will init the platform bitmap loader
        var drawableList = Splat.PlatformBitmapLoader.GetTypesFromAssembly(assemblyBuilder, null);
        Assert.That(drawableList, Is.Not.Null);
        Assert.That(drawableList.Length, Is.EqualTo(0));
    }

    /// <summary>
    /// Checks to ensure a list of drawable items is returned.
    /// </summary>
    [Test]
    public void GetDrawableList_ReturnsResults()
    {
        // can't test with a logger, as it invokes the splat init, which puts the test in a false state as it will init the platform bitmap loader
        var drawableList = Splat.PlatformBitmapLoader.GetDrawableList(null);
        Assert.That(drawableList, Is.Not.Null);
        Assert.That(drawableList.Count > 0, Is.True);
    }
#endif
}
