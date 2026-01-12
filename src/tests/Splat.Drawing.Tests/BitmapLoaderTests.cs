// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !IS_SHARED_NET

using System.IO;
using System.Reflection;

namespace Splat.Tests;

public sealed class BitmapLoaderTests
{
    /// <summary>
    /// Test to ensure the bitmap loader initializes properly.
    /// </summary>
    /// <remarks>
    /// Looks crude and pointless, but was produced to track an issue on Android between VS2017 and VS2019.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ReturnsInstance()
    {
        var instance = new PlatformBitmapLoader();
        await Assert.That(instance).IsNotNull();
    }

    /// <summary>
    /// Test to ensure creating a default bitmap succeeds on all platforms.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Create_Succeeds()
    {
        var instance = new PlatformBitmapLoader();

        object? result = null;
        await Assert.That(() => result = instance.Create(1, 1)).ThrowsNothing();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test to ensure loading a bitmap succeeds on all platforms.
    /// </summary>
    /// <param name="imageName">The resource name of the image to load.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("splatlogo.bmp")]
    [Arguments("splatlogo.jpg")]
    [Arguments("splatlogo.png")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2025:Do not pass 'IDisposable' instances into unawaited tasks", Justification = "Test so lack of dispose ok")]
    public async Task Load_Succeeds(string imageName)
    {
        var instance = new PlatformBitmapLoader();

        using var sourceStream = GetStream(imageName);

        object? result = null;
        await Assert.That(() => result = instance.Load(
                sourceStream,
                640,
                480)).ThrowsNothing();

        await Assert.That(result).IsNotNull();
    }

    private static Stream GetStream(string imageName)
    {
#if ANDROID
        return Android.App.Application.Context.Assets.Open(imageName);
#else
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(imageName)!;
#endif
    }
}

#endif
