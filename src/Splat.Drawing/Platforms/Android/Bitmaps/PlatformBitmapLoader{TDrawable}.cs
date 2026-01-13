// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// AOT-compatible platform bitmap loader for Android.
/// This version requires explicit drawable name-to-ID mapping and uses no reflection.
/// </summary>
/// <typeparam name="TDrawable">The Resource.Drawable type from your application (e.g., MyApp.Resource.Drawable).
/// This is used only for type safety and documentation - no reflection is performed on it.</typeparam>
/// <example>
/// <code>
/// // In your MAUI application startup:
/// var loader = new PlatformBitmapLoader&lt;MyApp.Resource.Drawable&gt;(name => name switch
/// {
///     "icon" => Resource.Drawable.icon,
///     "logo" => Resource.Drawable.logo,
///     "splash" => Resource.Drawable.splash,
///     _ => 0
/// });
/// Locator.CurrentMutable.RegisterConstant&lt;IBitmapLoader&gt;(loader);
/// </code>
/// </example>
public class PlatformBitmapLoader<TDrawable> : IBitmapLoader, IEnableLogger
{
    private readonly Func<string, int> _drawableResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformBitmapLoader{TDrawable}"/> class.
    /// </summary>
    /// <param name="drawableResolver">A function that maps drawable names to resource IDs.
    /// Use a switch expression for AOT-friendly compile-time mapping.</param>
    /// <example>
    /// <code>
    /// var loader = new PlatformBitmapLoader&lt;Resource.Drawable&gt;(name => name switch
    /// {
    ///     "icon" => Resource.Drawable.icon,
    ///     "logo" => Resource.Drawable.logo,
    ///     _ => 0  // Return 0 for unknown resources
    /// });
    /// </code>
    /// </example>
    public PlatformBitmapLoader(Func<string, int> drawableResolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(drawableResolver);
        _drawableResolver = drawableResolver;
    }

    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) =>
        PlatformBitmapLoaderHelpers.LoadFromStream(sourceStream, desiredWidth, desiredHeight, this);

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
    {
        // Try parsing as integer ID first
        if (int.TryParse(source, out var id))
        {
            return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(id));
        }

        // Try resolver with original name
        var resourceId = _drawableResolver(source);
        if (resourceId != 0)
        {
            return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(resourceId));
        }

        // Try without extension (Android strips extensions)
        var nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(source);
        resourceId = _drawableResolver(nameWithoutExtension);
        if (resourceId != 0)
        {
            return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(resourceId));
        }

        throw new ArgumentException(
            $"Drawable resource '{source}' not found. Either pass an integer ID cast to a string, or ensure the resource name is registered in your drawable resolver.",
            nameof(source));
    }

    /// <inheritdoc />
    public IBitmap? Create(float width, float height) =>
        PlatformBitmapLoaderHelpers.CreateBitmap(width, height);
}
