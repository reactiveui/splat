// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering <c>ServiceLocationDrawingInitializationExtensions</c>.</summary>
public sealed class ServiceLocationDrawingInitializationExtensionsCoverageTests
{
    /// <summary>Verifies that registering on a fresh resolver does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterPlatformBitmapLoader_OnFreshResolver_DoesNotThrow()
    {
        var resolver = new ModernDependencyResolver();

        await Assert.That(() => resolver.RegisterPlatformBitmapLoader()).ThrowsNothing();
    }

    /// <summary>Verifies that registering twice on the same resolver remains safe.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterPlatformBitmapLoader_CalledTwice_DoesNotThrow()
    {
        var resolver = new ModernDependencyResolver();

        resolver.RegisterPlatformBitmapLoader();

        await Assert.That(() => resolver.RegisterPlatformBitmapLoader()).ThrowsNothing();
    }

    /// <summary>Verifies that registering on a null resolver throws an argument exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterPlatformBitmapLoader_NullResolver_Throws()
    {
        const IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.RegisterPlatformBitmapLoader()).Throws<ArgumentNullException>();
    }
}
