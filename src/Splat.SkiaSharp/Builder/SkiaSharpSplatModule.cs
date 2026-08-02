// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

using Splat.SkiaSharp;

namespace Splat.Builder;

/// <summary>Registers Skia as the bitmap loader when the application is built.</summary>
/// <remarks>
/// Registration is a plain method call rather than assembly scanning, so it survives trimming and
/// ahead-of-time compilation.
/// </remarks>
/// <param name="sampling">The sampling to resize with, or <see langword="null"/> for the loader's default.</param>
public sealed class SkiaSharpSplatModule(SKSamplingOptions? sampling) : IModule
{
    /// <summary>Initializes a new instance of the <see cref="SkiaSharpSplatModule"/> class.</summary>
    public SkiaSharpSplatModule()
        : this(null)
    {
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        if (sampling is null)
        {
            resolver.UseSkiaSharpBitmapLoader();
            return;
        }

        resolver.UseSkiaSharpBitmapLoader(sampling.Value);
    }
}
