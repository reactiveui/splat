// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;
using System.Text;

using Android.App;
using Android.Graphics;

namespace Splat;

/// <summary>
/// A android based platform bitmap loader which will load our bitmaps for us.
/// </summary>
public class PlatformBitmapLoader : IBitmapLoader, IEnableLogger
{
    private readonly Dictionary<string, int> _drawableList;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformBitmapLoader"/> class.
    /// </summary>
    public PlatformBitmapLoader() => _drawableList = GetDrawableList();

    /// <inheritdoc />
    public async Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
    {
        sourceStream.ThrowArgumentNullExceptionIfNull(nameof(sourceStream));

        // this is a rough check to do with the termination check for #479
        if (sourceStream.Length < 2)
        {
            throw new ArgumentException("The source stream is not a valid image file.", nameof(sourceStream));
        }

        if (!HasCorrectStreamEnd(sourceStream))
        {
            AttemptStreamByteCorrection(sourceStream);
        }

        sourceStream.Position = 0;
        Bitmap? bitmap = null;

        if (desiredWidth is null || desiredHeight is null)
        {
            bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream)).ConfigureAwait(false);
        }
        else
        {
            using var opts = new BitmapFactory.Options()
            {
                OutWidth = (int)desiredWidth.Value,
                OutHeight = (int)desiredHeight.Value,
            };

            using var noPadding = new Rect(0, 0, 0, 0);
            bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream, noPadding, opts)).ConfigureAwait(true);
        }

        return bitmap switch
        {
            null => throw new IOException("Failed to load bitmap from source stream"),
            _ => bitmap.FromNative()
        };
    }

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
    {
        if (_drawableList is null)
        {
            throw new InvalidOperationException("No resources found in any of the drawable folders.");
        }

        var res = Application.Context.Resources;
        var theme = Application.Context.Theme;

        if (res is null)
        {
            throw new InvalidOperationException("No resources found in the application.");
        }

        if (int.TryParse(source, out var id))
        {
            return Task.Run<IBitmap?>(() => GetFromDrawable(res.GetDrawable(id, theme)));
        }

        if (_drawableList.TryGetValue(source, out var value))
        {
            return Task.Run<IBitmap?>(() => GetFromDrawable(res.GetDrawable(value, theme)));
        }

        // NB: On iOS, you have to pass the extension, but on Android it's
        // stripped - try stripping the extension to see if there's a Drawable.
        var key = System.IO.Path.GetFileNameWithoutExtension(source);
        return _drawableList.TryGetValue(key, out var intValue)
            ? Task.Run<IBitmap?>(() => GetFromDrawable(res.GetDrawable(intValue, theme)))
            : throw new ArgumentException("Either pass in an integer ID cast to a string, or the name of a drawable resource");
    }

    /// <inheritdoc />
    public IBitmap? Create(float width, float height)
    {
        var config = Bitmap.Config.Argb8888 ?? throw new InvalidOperationException("The ARGB8888 bitmap format is unavailable");

#pragma warning disable CA2000 // Dispose objects before losing scope
        return Bitmap.CreateBitmap((int)width, (int)height, config).FromNative();
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    internal static Dictionary<string, int> GetDrawableList(IFullLogger? log)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return GetDrawableList(log, assemblies);
    }

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
            return e.Types is not null
                ? e.Types.Where(x => x is not null).Select(x => x!).ToArray()
                : [];
        }
    }

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
            .ToDictionary(k => k.Name, v => ((int?)v.GetRawConstantValue()) ?? 0);

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

    private static DrawableBitmap? GetFromDrawable(Android.Graphics.Drawables.Drawable? drawable) => drawable is null ? null : new DrawableBitmap(drawable);

    /// <summary>
    /// Checks to make sure the last 2 bytes are as expected.
    /// issue #479 xamarin android can throw an objectdisposedexception on stream
    /// suggestion is it relates to https://forums.xamarin.com/discussion/16500/bitmap-decode-byte-array-skia-decoder-returns-false
    /// and truncated jpeg\png files.
    /// </summary>
    /// <param name="sourceStream">Input image source stream.</param>
    /// <returns>Whether the termination is correct.</returns>
    private static bool HasCorrectStreamEnd(Stream sourceStream)
    {
        // 0-based and go back 2.
        sourceStream.Position = sourceStream.Length - 3;
        return sourceStream.ReadByte() == 0xFF
               && sourceStream.ReadByte() == 0xD9;
    }

    private static Dictionary<string, int> GetDrawableList() => GetDrawableList(Locator.Current.GetService<ILogManager>()?.GetLogger(typeof(PlatformBitmapLoader)));

    private void AttemptStreamByteCorrection(Stream sourceStream)
    {
        if (!sourceStream.CanWrite)
        {
            this.Log().Warn("Stream missing terminating bytes but is read only.");
        }
        else
        {
            this.Log().Warn("Carrying out source stream byte correction.");
            sourceStream.Position = sourceStream.Length;
            sourceStream.Write([0xFF, 0xD9]);
        }
    }
}
