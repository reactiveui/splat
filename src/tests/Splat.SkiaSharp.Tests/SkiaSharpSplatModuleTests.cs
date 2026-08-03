// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

using Splat.Builder;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for registering Skia as the bitmap loader.</summary>
public sealed class SkiaSharpSplatModuleTests
{
    /// <summary>Verifies that the module registers a loader that can be resolved.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_RegistersTheBitmapLoader()
    {
        using var resolver = new ModernDependencyResolver();

        new SkiaSharpSplatModule().Configure(resolver);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IBitmapLoader))).IsTrue();
            await Assert.That(resolver.GetService<IBitmapLoader>()).IsTypeOf<SkiaBitmapLoader>();
        }
    }

    /// <summary>Verifies that a module built with a sampling passes it to the loader.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_WithASampling_PassesItToTheLoader()
    {
        using var resolver = new ModernDependencyResolver();
        var sampling = new SKSamplingOptions(SKFilterMode.Nearest);

        new SkiaSharpSplatModule(sampling).Configure(resolver);

        await Assert.That(((SkiaBitmapLoader)resolver.GetService<IBitmapLoader>()!).Sampling).IsEqualTo(sampling);
    }

    /// <summary>Verifies that configuring twice leaves a usable registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Twice_LeavesAUsableRegistration()
    {
        using var resolver = new ModernDependencyResolver();
        var module = new SkiaSharpSplatModule();

        module.Configure(resolver);
        module.Configure(resolver);

        await Assert.That(resolver.GetService<IBitmapLoader>()).IsNotNull();
    }

    /// <summary>Verifies that the module rejects a missing resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_WithoutAResolver_Throws() =>
        await Assert.That(static () => new SkiaSharpSplatModule().Configure(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies that the registration extension registers a loader that can be resolved.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSkiaSharpBitmapLoader_RegistersTheBitmapLoader()
    {
        using var resolver = new ModernDependencyResolver();

        resolver.UseSkiaSharpBitmapLoader();

        await Assert.That(resolver.GetService<IBitmapLoader>()).IsTypeOf<SkiaBitmapLoader>();
    }

    /// <summary>Verifies that the registration extension passes the chosen sampling to the loader.</summary>
    /// <param name="resampler">The cubic resampler coefficient to build the sampling from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(0.33F)]
    public async Task UseSkiaSharpBitmapLoader_WithASampling_PassesItToTheLoader(float resampler)
    {
        using var resolver = new ModernDependencyResolver();
        var sampling = new SKSamplingOptions(new SKCubicResampler(resampler, resampler));

        resolver.UseSkiaSharpBitmapLoader(sampling);

        await Assert.That(((SkiaBitmapLoader)resolver.GetService<IBitmapLoader>()!).Sampling).IsEqualTo(sampling);
    }

    /// <summary>Verifies that the registration extensions reject a missing resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSkiaSharpBitmapLoader_WithoutAResolver_Throws()
    {
        const IMutableDependencyResolver resolver = null!;
        var sampling = new SKSamplingOptions(SKFilterMode.Nearest);

        using (Assert.Multiple())
        {
            await Assert.That(static () => resolver.UseSkiaSharpBitmapLoader()).Throws<ArgumentNullException>();
            await Assert.That(() => resolver.UseSkiaSharpBitmapLoader(sampling)).Throws<ArgumentNullException>();
        }
    }

    /// <summary>Verifies that the module can be composed through the application builder.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [NotInParallel] // Mutates the builder's global built state.
    public async Task UsingModule_RegistersTheBitmapLoaderOnTheBuildersResolver()
    {
        using var resolver = new ModernDependencyResolver();
        AppBuilder.ResetBuilderStateForTests();

        _ = new AppBuilder(resolver).UsingModule(new SkiaSharpSplatModule()).Build();

        await Assert.That(resolver.GetService<IBitmapLoader>()).IsTypeOf<SkiaBitmapLoader>();
    }
}
