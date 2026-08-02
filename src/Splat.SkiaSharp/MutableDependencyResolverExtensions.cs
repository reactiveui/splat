// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

namespace Splat.SkiaSharp;

/// <summary>Registers Skia as the bitmap loader Splat resolves.</summary>
/// <remarks>
/// The plain .NET target frameworks register no bitmap loader of their own, so <see cref="BitmapLoader.Current"/>
/// throws until something is registered. Registration is explicit: nothing scans assemblies for it.
/// </remarks>
public static class MutableDependencyResolverExtensions
{
    /// <summary>Extension members for <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="instance">An instance of Mutable Dependency Resolver.</param>
    extension(IMutableDependencyResolver instance)
    {
        /// <summary>Registers <see cref="SkiaBitmapLoader"/> as the <see cref="IBitmapLoader"/>.</summary>
        /// <example>
        /// <c>AppLocator.CurrentMutable.UseSkiaSharpBitmapLoader();</c>
        /// </example>
        public void UseSkiaSharpBitmapLoader()
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);

            instance.RegisterLazySingleton(static () => new SkiaBitmapLoader(), typeof(IBitmapLoader));
        }

        /// <summary>Registers <see cref="SkiaBitmapLoader"/> as the <see cref="IBitmapLoader"/>, resizing with the given sampling.</summary>
        /// <param name="sampling">The sampling to resize with.</param>
        /// <example>
        /// <c>AppLocator.CurrentMutable.UseSkiaSharpBitmapLoader(new SKSamplingOptions(SKFilterMode.Nearest));</c>
        /// </example>
        public void UseSkiaSharpBitmapLoader(SKSamplingOptions sampling)
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);

            instance.RegisterLazySingleton(() => new SkiaBitmapLoader(sampling), typeof(IBitmapLoader));
        }
    }
}
