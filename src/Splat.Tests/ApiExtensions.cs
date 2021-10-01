// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PublicApiGenerator;
using VerifyXunit;

#pragma warning disable SA1615 // Element return value should be documented

namespace Splat.Tests
{
    /// <summary>
    /// A helper for doing API approvals.
    /// </summary>
    public static class ApiExtensions
    {
        private static readonly Regex _removeCoverletSectionRegex = new(@"^namespace Coverlet\.Core\.Instrumentation\.Tracker.*?^}", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Checks to make sure the API is approved.
        /// </summary>
        /// <param name="assembly">The assembly that is being checked.</param>
        /// <param name="filePath">The caller file path.</param>
        public static Task CheckApproval(this Assembly assembly, [CallerFilePath] string filePath = "")
        {
            var generatorOptions = new ApiGeneratorOptions { WhitelistedNamespacePrefixes = new[] { "Splat" } };
            var apiText = assembly.GeneratePublicApi(generatorOptions);
            apiText = _removeCoverletSectionRegex.Replace(apiText, string.Empty);
            return Verifier.Verify(apiText, null, filePath)
                .UniqueForRuntimeAndVersion()
                .ScrubEmptyLines()
                .ScrubLines(l =>
                    l.StartsWith("[assembly: AssemblyVersion(", StringComparison.InvariantCulture) ||
                    l.StartsWith("[assembly: AssemblyFileVersion(", StringComparison.InvariantCulture) ||
                    l.StartsWith("[assembly: AssemblyInformationalVersion(", StringComparison.InvariantCulture) ||
                    l.StartsWith("[assembly: System.Reflection.AssemblyMetadata(", StringComparison.InvariantCulture));
        }
    }
}
