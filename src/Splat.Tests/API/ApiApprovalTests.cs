﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !WINDOWS_UWP && !ANDROID

using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Splat.Tests
{
    /// <summary>
    /// Tests to make sure that the API matches the approved ones.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiApprovalTests
    {
        /// <summary>
        /// Tests to make sure the splat project is approved.
        /// </summary>
        [Fact]
        public void SplatProject()
        {
            typeof(AssemblyFinder).Assembly.CheckApproval();
        }
    }
}

#endif
