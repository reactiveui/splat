// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
#if !IS_SHARED_NET
using System.IO;

namespace Splat.Tests;

/// <summary>Unit tests for the platform bitmap loader.</summary>
public sealed class BitmapLoaderTests
{
    /// <summary>The width, in pixels, requested when loading a bitmap in the load test.</summary>
    private const int LoadWidth = 640;

    /// <summary>The height, in pixels, requested when loading a bitmap in the load test.</summary>
    private const int LoadHeight = 480;

    /// <summary>Test to ensure the bitmap loader initializes properly.</summary>
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

    /// <summary>Test to ensure creating a default bitmap succeeds on all platforms.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Create_Succeeds()
    {
        var instance = new PlatformBitmapLoader();

        object? result = null;
        await Assert.That(() => result = instance.Create(1, 1)).ThrowsNothing();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>Test to ensure loading a bitmap succeeds on all platforms.</summary>
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

        await using var sourceStream = GetStream(imageName);

        object? result = null;
        await Assert.That(() => result = instance.Load(
                sourceStream,
                LoadWidth,
                LoadHeight)).ThrowsNothing();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>Opens the named test image as a stream from the platform's resource store.</summary>
    /// <param name="imageName">The resource name of the image to open.</param>
    /// <returns>A <see cref="Stream"/> for the requested image.</returns>
    private static Stream GetStream(string imageName)
    {
#if ANDROID
        return Android.App.Application.Context.Assets.Open(imageName);
#else
        var assembly = typeof(BitmapLoaderTests).Assembly;
        return assembly.GetManifestResourceStream(imageName)!;
#endif
    }
}

#endif
