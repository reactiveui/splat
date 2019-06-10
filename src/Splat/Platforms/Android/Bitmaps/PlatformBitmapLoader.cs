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
        private static readonly Dictionary<string, int> _drawableList;
        private static readonly IFullLogger _log;

        /// <summary>
        /// Initializes static members of the <see cref="PlatformBitmapLoader"/> class.
        /// </summary>
        static PlatformBitmapLoader()
        {
            _log = Locator.Current.GetService<ILogManager>().GetLogger(typeof(PlatformBitmapLoader));
            _drawableList = GetDrawableList();
        }

        /// <inheritdoc />
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
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
        public IBitmap Create(float width, float height)
        {
            return Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb8888).FromNative();
        }

        internal static Dictionary<string, int> GetDrawableList()
        {
            // VS2019 onward
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetTypesFromAssembly)
                .Where(x => x.Name == "Resource" && x.GetNestedType("Drawable") != null)
                .Select(x => x.GetNestedType("Drawable"))
                .ToArray();

            _log.Debug(() => "DrawableList. Got " + assemblies.Length + " assemblies.");
            foreach (var assembly in assemblies)
            {
                _log.Debug(() => "DrawableList Assembly: " + assembly.Name);
            }

            var result = assemblies
                .SelectMany(x => x.GetFields())
                .Where(x => x.FieldType == typeof(int) && x.IsLiteral)
                .ToDictionary(k => k.Name, v => (int)v.GetRawConstantValue());

            _log.Debug(() => "DrawableList. Got " + result.Count + " items.");
            foreach (var keyValuePair in result)
            {
                _log.Debug(() => "DrawableList Item: " + keyValuePair.Key);
            }

            return result;
        }

        internal static Type[] GetTypesFromAssembly(Assembly assembly)
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
                _log.Warn(e, "Exception while detecting drawing types.");

                foreach (var loaderException in e.LoaderExceptions)
                {
                    _log.Warn(loaderException, "Inner Exception for detecting drawing types.");
                }

                return e.Types.Where(x => x != null).ToArray();
            }
        }
    }
}
