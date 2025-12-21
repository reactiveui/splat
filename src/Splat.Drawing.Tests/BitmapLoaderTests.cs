// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Reflection;

#if !IS_SHARED_NET

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the Bitmap Loader.
/// </summary>
[TestFixture]
public sealed class BitmapLoaderTests
{
    /// <summary>
    /// Test to ensure the bitmap loader initializes properly.
    /// </summary>
    /// <remarks>
    /// Looks crude and pointless, but was produced to track an issue on Android between VS2017 and VS2019.
    /// </remarks>
    [Test]
    public void ReturnsInstance()
    {
        var instance = new PlatformBitmapLoader();
        Assert.That(instance, Is.Not.Null);
    }

    /// <summary>
    /// Test to ensure creating a default bitmap succeeds on all platforms.
    /// </summary>
    [Test]
    public void Create_Succeeds()
    {
        var instance = new PlatformBitmapLoader();

        object? result = null;
        Assert.DoesNotThrow(() => result = instance.Create(1, 1));

        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Test to ensure loading a bitmap succeeds on all platforms.
    /// </summary>
    /// <param name="imageName">The resource name of the image to load.</param>
    [TestCase("splatlogo.bmp")]
    [TestCase("splatlogo.jpg")]
    [TestCase("splatlogo.png")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2025:Do not pass 'IDisposable' instances into unawaited tasks", Justification = "Test so lack of dispose ok")]
    public void Load_Succeeds(string imageName)
    {
        var instance = new PlatformBitmapLoader();

        using var sourceStream = GetStream(imageName);

        object? result = null;
        Assert.DoesNotThrow(() => result = instance.Load(
                sourceStream,
                640,
                480));

        Assert.That(result, Is.Not.Null);
    }

    private static Stream GetStream(string imageName)
    {
#if ANDROID
        return Android.App.Application.Context.Assets.Open(imageName);
#else
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(imageName)!;
#endif
    }
}

#endif
