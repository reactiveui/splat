// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !WINDOWS_UWP && !ANDROID

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;
#pragma warning disable SA1615 // Element return value should be documented

namespace Splat.Tests
{
    /// <summary>
    /// Tests to make sure that the API matches the approved ones.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [UsesVerify]
    public class ApiApprovalTests
    {
        private static readonly Regex _removeCoverletSectionRegex = new(@"^namespace Coverlet\.Core\.Instrumentation\.Tracker.*?^}", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Tests to make sure the splat project is approved.
        /// </summary>
        [Fact]
        public Task SplatProject()
        {
            var assembly = typeof(AssemblyFinder).Assembly;
            var generatorOptions = new ApiGeneratorOptions { WhitelistedNamespacePrefixes = new[] { "Splat" } };
            var apiText = assembly.GeneratePublicApi(generatorOptions);
            apiText = _removeCoverletSectionRegex.Replace(apiText, string.Empty);
            return Verifier.Verify(apiText)
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

#endif
