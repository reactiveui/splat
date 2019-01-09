// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#if NETFX_CORE
using Windows.ApplicationModel;
#endif

namespace Splat
{
    /// <summary>
    /// Detects if we are in design mode or unit test mode based on the current platform.
    /// </summary>
    public class PlatformModeDetector : IModeDetector
    {
        /// <inheritdoc />
        public bool? InUnitTestRunner()
        {
            var testAssemblies = new[]
            {
                "CSUNIT",
                "NUNIT",
                "XUNIT",
                "MBUNIT",
                "NBEHAVE",
                "VISUALSTUDIO.QUALITYTOOLS",
                "FIXIE",
                "NCRUNCH",
            };

            try
            {
                return SearchForAssembly(testAssemblies);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public bool? InDesignMode()
        {
#if NETFX_CORE
            return DesignMode.DesignModeEnabled;
#else
            var designEnvironments = new[]
            {
                "BLEND.EXE",
                "XDESPROC.EXE",
            };

            var entry = Assembly.GetEntryAssembly();
            if (entry != null)
            {
                var exeName = new FileInfo(entry.Location).Name;

                if (designEnvironments.Any(x => x.IndexOf(exeName, StringComparison.InvariantCultureIgnoreCase) != -1))
                {
                    return true;
                }
            }

            return false;
#endif
        }

        private static bool SearchForAssembly(IEnumerable<string> assemblyList)
        {
#if NETFX_CORE
            var depPackages = Package.Current.Dependencies.Select(x => x.Id.FullName.ToUpperInvariant());
            if (depPackages.Any(x => assemblyList.Any(name => x.Contains(name)))) return true;

            var fileTask = Task.Factory.StartNew(async () => {
                var files = await Package.Current.InstalledLocation.GetFilesAsync();
                return files.Select(x => x.Path).ToArray();
            }, TaskCreationOptions.HideScheduler).Unwrap();

            return fileTask.Result.Any(x => assemblyList.Any(name => x.Contains(name, StringComparison.InvariantCulture)));
#else
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(x => x.FullName.ToUpperInvariant())
                .Any(x => assemblyList.Any(name => x.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) != -1));
#endif
        }
    }
}
