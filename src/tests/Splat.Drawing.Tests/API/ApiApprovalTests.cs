// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Tests;

/// <summary>
/// Tests to make sure that the API matches the approved ones.
/// </summary>
[ExcludeFromCodeCoverage]
public class ApiApprovalTests
{
    /// <summary>
    /// Tests to make sure the splat project is approved.
    /// </summary>
    /// <returns>The task.</returns>
    [Test]
    public Task SplatUIProject() =>
#if WINDOWS
        typeof(IPlatformModeDetector).Assembly.CheckApproval(["Splat"]);
#else
        Task.CompletedTask;
#endif

}
