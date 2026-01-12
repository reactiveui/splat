// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Splat;

/// <summary>
/// A android based platform bitmap loader which will load our bitmaps for us.
/// </summary>
public class PlatformBitmapLoader : IBitmapLoader, IEnableLogger
{
    private static Func<string, int>? _drawableResolver;
    private static Type? _drawableType;
    private readonly Dictionary<string, int> _drawableList;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformBitmapLoader"/> class.
    /// </summary>
    /// <remarks>
    /// For AOT compatibility, call <see cref="RegisterDrawableResolver"/> or <see cref="RegisterDrawables{TDrawable}"/>
    /// before creating instances of this class.
    /// </remarks>
    [RequiresUnreferencedCode("Constructor may use reflection for drawable discovery if RegisterDrawableResolver() or RegisterDrawables<T>() are not called first. For full AOT compatibility, use PlatformBitmapLoader<T> instead.")]
    public PlatformBitmapLoader()
    {
        // If a resolver is registered, we don't need the dictionary at all (AOT-safe)
        if (_drawableResolver is not null)
        {
            _drawableList = [];
            return;
        }

        // If a drawable type is registered, use it (targeted reflection, user opted in)
        if (_drawableType is not null)
        {
            _drawableList = GetDrawableListFromRegisteredType();
            return;
        }

        // Fall back to assembly scanning (reflection-based, not AOT-compatible)
        _drawableList = GetDrawableListViaReflection();
    }

    /// <summary>
    /// Registers a drawable resolver function for AOT-friendly resource lookup.
    /// This avoids assembly scanning and is recommended for modern MAUI applications.
    /// </summary>
    /// <param name="resolver">A function that maps drawable names to resource IDs.</param>
    /// <example>
    /// <code>
    /// PlatformBitmapLoader.RegisterDrawableResolver(name => name switch
    /// {
    ///     "icon" => Resource.Drawable.icon,
    ///     "logo" => Resource.Drawable.logo,
    ///     _ => 0
    /// });
    /// </code>
    /// </example>
    public static void RegisterDrawableResolver(Func<string, int> resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        _drawableResolver = resolver;
    }

    /// <summary>
    /// Registers a Resource.Drawable type for AOT-friendly resource lookup.
    /// This avoids assembly scanning and is recommended for modern MAUI applications.
    /// </summary>
    /// <typeparam name="TDrawable">The Resource.Drawable type from your application.</typeparam>
    /// <example>
    /// <code>
    /// PlatformBitmapLoader.RegisterDrawables&lt;MyApp.Resource.Drawable&gt;();
    /// </code>
    /// </example>
    public static void RegisterDrawables<TDrawable>() => _drawableType = typeof(TDrawable);

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

        // Try registered resolver (AOT-friendly path)
        if (_drawableResolver is not null)
        {
            var resourceId = _drawableResolver(source);
            if (resourceId != 0)
            {
                return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(resourceId));
            }

            // Try without extension
            var key = System.IO.Path.GetFileNameWithoutExtension(source);
            resourceId = _drawableResolver(key);
            if (resourceId != 0)
            {
                return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(resourceId));
            }

            ArgumentExceptionHelper.ThrowIf(true, "Either pass in an integer ID cast to a string, or the name of a drawable resource", nameof(source));
            return null!; // unreachable
        }

        // Fall back to dictionary lookup (from reflection or registered type)
        if (_drawableList is null)
        {
            throw new InvalidOperationException("No resources found in any of the drawable folders.");
        }

        if (_drawableList.TryGetValue(source, out var value))
        {
            return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(value));
        }

        // NB: On iOS, you have to pass the extension, but on Android it's
        // stripped - try stripping the extension to see if there's a Drawable.
        var key2 = System.IO.Path.GetFileNameWithoutExtension(source);
        if (_drawableList.TryGetValue(key2, out var intValue))
        {
            return Task.Run(() => PlatformBitmapLoaderHelpers.LoadFromDrawableId(intValue));
        }

        ArgumentExceptionHelper.ThrowIf(true, "Either pass in an integer ID cast to a string, or the name of a drawable resource", nameof(source));
        return null!; // unreachable
    }

    /// <inheritdoc />
    public IBitmap? Create(float width, float height) =>
        PlatformBitmapLoaderHelpers.CreateBitmap(width, height);

    [RequiresUnreferencedCode("Assembly scanning uses reflection and is not AOT-compatible. Use RegisterDrawables<T>() or RegisterDrawableResolver() for AOT scenarios.")]
    internal static Dictionary<string, int> GetDrawableList(IFullLogger? log)
    {
        // Check for registered drawable type first (AOT-friendly)
        if (_drawableType is not null)
        {
            return GetDrawableListFromType(_drawableType, log);
        }

        // Fall back to assembly scanning (reflection-based, not AOT-compatible)
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return GetDrawableList(log, assemblies);
    }

    [RequiresUnreferencedCode("Uses reflection to extract drawable fields. For full AOT compatibility, use RegisterDrawableResolver() instead.")]
    private static Dictionary<string, int> GetDrawableListFromRegisteredType()
    {
        var log = Locator.Current.GetService<ILogManager>()?.GetLogger(typeof(PlatformBitmapLoader));
        return GetDrawableListFromType(_drawableType!, log);
    }

    [RequiresUnreferencedCode("Assembly scanning uses reflection and is not AOT-compatible. Call RegisterDrawableResolver() or RegisterDrawables<T>() before instantiation to avoid reflection.")]
    private static Dictionary<string, int> GetDrawableListViaReflection()
    {
        var log = Locator.Current.GetService<ILogManager>()?.GetLogger(typeof(PlatformBitmapLoader));
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return GetDrawableList(log, assemblies);
    }

    [RequiresUnreferencedCode("Uses reflection to extract drawable fields. For full AOT compatibility, use RegisterDrawableResolver() instead.")]
    private static Dictionary<string, int> GetDrawableListFromType(Type drawableType, IFullLogger? log)
    {
        if (log?.IsDebugEnabled == true)
        {
            log.Debug($"GetDrawableListFromType: Using registered type {drawableType.FullName}");
        }

        var fields = drawableType.GetFields();
        var result = new Dictionary<string, int>(fields.Length);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(int) && field.IsLiteral)
            {
                result[field.Name] = (int?)field.GetRawConstantValue() ?? 0;
            }
        }

        if (log?.IsDebugEnabled == true)
        {
            var output = new StringBuilder();
            output.Append("DrawableList. Got ").Append(result.Count).AppendLine(" items from registered type.");

            foreach (var keyValuePair in result)
            {
                output.Append("DrawableList Item: ").AppendLine(keyValuePair.Key);
            }

            log.Debug(output.ToString());
        }

        return result;
    }

    [RequiresUnreferencedCode("Assembly scanning uses reflection and is not AOT-compatible.")]
    private static Type[] GetTypesFromAssembly(
        Assembly assembly,
        IFullLogger? log)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            // The array returned by the Types property of this exception contains a Type
            // object for each type that was loaded and null for each type that could not
            // be loaded, while the LoaderExceptions property contains an exception for
            // each type that could not be loaded.
            if (log is not null)
            {
                log.Warn(e, "Exception while detecting drawing types.");

                foreach (var loaderException in e.LoaderExceptions)
                {
                    if (loaderException is null)
                    {
                        continue;
                    }

                    log.Warn(loaderException, "Inner Exception for detecting drawing types.");
                }
            }

            // null check here because mono doesn't appear to follow the MSDN documentation
            // as of July 2019.
            if (e.Types is null)
            {
                return [];
            }

            var result = new List<Type>(e.Types.Length);
            foreach (var type in e.Types)
            {
                if (type is not null)
                {
                    result.Add(type);
                }
            }

            return [.. result];
        }
    }

    [RequiresUnreferencedCode("Assembly scanning uses reflection and is not AOT-compatible.")]
    private static Dictionary<string, int> GetDrawableList(
        IFullLogger? log,
        Assembly[] assemblies)
    {
        // VS2019 onward
        var drawableTypes = assemblies
            .AsParallel()
            .SelectMany(a => GetTypesFromAssembly(a, log))
            .Where(x => x.Name.Equals("Resource", StringComparison.Ordinal) && x.GetNestedType("Drawable") is not null)
            .Select(x => x.GetNestedType("Drawable"))
            .Where(x => x != null)
            .Select(x => x!)
            .ToArray();

        if (log?.IsDebugEnabled == true)
        {
            var output = new StringBuilder();
            output.Append("DrawableList. Got ").Append(drawableTypes.Length).AppendLine(" types.");

            foreach (var drawableType in drawableTypes)
            {
                output.Append("DrawableList Type: ").AppendLine(drawableType.Name);
            }

            log.Debug(output.ToString());
        }

        var result = drawableTypes
            .AsParallel()
            .SelectMany(x => x.GetFields())
            .Where(x => x.FieldType == typeof(int) && x.IsLiteral)
            .ToDictionary(k => k.Name, v => (int?)v.GetRawConstantValue() ?? 0);

        if (log?.IsDebugEnabled == true)
        {
            var output = new StringBuilder();
            output.Append("DrawableList. Got ").Append(result.Count).AppendLine(" items.");

            foreach (var keyValuePair in result)
            {
                output.Append("DrawableList Item: ").AppendLine(keyValuePair.Key);
            }

            log.Debug(output.ToString());
        }

        return result;
    }
}
