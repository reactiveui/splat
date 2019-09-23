// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;

namespace Splat
{
    /// <summary>
    /// A android based platform bitmap loader which will load our bitmaps for us.
    /// </summary>
    public class PlatformBitmapLoader : IBitmapLoader
    {
        private readonly Dictionary<string, int> _drawableList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformBitmapLoader"/> class.
        /// </summary>
        public PlatformBitmapLoader()
        {
            _drawableList = GetDrawableList();
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by bitmap.")]
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            if (sourceStream is null)
            {
                throw new ArgumentNullException(nameof(sourceStream));
            }

            sourceStream.Position = 0;
            Bitmap bitmap = null;

            if (desiredWidth == null)
            {
                bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream)).ConfigureAwait(false);
            }
            else
            {
                var opts = new BitmapFactory.Options()
                {
                    OutWidth = (int)desiredWidth.Value,
                    OutHeight = (int)desiredHeight.Value,
                };

                var noPadding = new Rect(0, 0, 0, 0);
                bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream, noPadding, opts)).ConfigureAwait(true);
            }

            if (bitmap == null)
            {
                throw new IOException("Failed to load bitmap from source stream");
            }

            return bitmap.FromNative();
        }

        /// <inheritdoc />
        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            if (_drawableList == null)
            {
                throw new InvalidOperationException("No resources found in any of the drawable folders.");
            }

            var res = Application.Context.Resources;
            var theme = Application.Context.Theme;

            var id = default(int);
            if (int.TryParse(source, out id))
            {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(id, theme)));
            }

            if (_drawableList.ContainsKey(source))
            {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(_drawableList[source], theme)));
            }

            // NB: On iOS, you have to pass the extension, but on Android it's
            // stripped - try stripping the extension to see if there's a Drawable.
            var key = System.IO.Path.GetFileNameWithoutExtension(source);
            if (_drawableList.ContainsKey(key))
            {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(_drawableList[key], theme)));
            }

            throw new ArgumentException("Either pass in an integer ID cast to a string, or the name of a drawable resource");
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handled by user.")]
        public IBitmap Create(float width, float height)
        {
            return Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb8888).FromNative();
        }

        internal static Dictionary<string, int> GetDrawableList(IFullLogger log)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return GetDrawableList(log, assemblies);
        }

        internal static Dictionary<string, int> GetDrawableList(
            IFullLogger log,
            Assembly[] assemblies)
        {
            // VS2019 onward
            var drawableTypes = assemblies
                .SelectMany(a => GetTypesFromAssembly(a, log))
                .Where(x => x.Name == "Resource" && x.GetNestedType("Drawable") != null)
                .Select(x => x.GetNestedType("Drawable"))
                .ToArray();

            if (log != null)
            {
                log.Debug(() => "DrawableList. Got " + drawableTypes.Length + " types.");
                foreach (var drawableType in drawableTypes)
                {
                    log.Debug(() => "DrawableList Type: " + drawableType.Name);
                }
            }

            var result = drawableTypes
                .SelectMany(x => x.GetFields())
                .Where(x => x.FieldType == typeof(int) && x.IsLiteral)
                .ToDictionary(k => k.Name, v => (int)v.GetRawConstantValue());

            if (log != null)
            {
                log.Debug(() => "DrawableList. Got " + result.Count + " items.");
                foreach (var keyValuePair in result)
                {
                    log.Debug(() => "DrawableList Item: " + keyValuePair.Key);
                }
            }

            return result;
        }

        internal static Type[] GetTypesFromAssembly(
            Assembly assembly,
            IFullLogger log)
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
                if (log != null)
                {
                    log.Warn(e, "Exception while detecting drawing types.");

                    foreach (var loaderException in e.LoaderExceptions)
                    {
                        log.Warn(loaderException, "Inner Exception for detecting drawing types.");
                    }
                }

                // null check here because mono doesn't appear to follow the MSDN documentation
                // as of July 2019.
                return e.Types != null
                    ? e.Types.Where(x => x != null).ToArray()
                    : Array.Empty<Type>();
            }
        }

        private Dictionary<string, int> GetDrawableList()
        {
            return GetDrawableList(Locator.Current.GetService<ILogManager>().GetLogger(typeof(PlatformBitmapLoader)));
        }
    }
}
