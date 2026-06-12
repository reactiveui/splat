// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Builder;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="NLogSplatModule"/> registers NLog logging.</summary>
public class NLogSplatModuleCoverageTests
{
    /// <summary>Verifies that <see cref="NLogSplatModule.Configure"/> registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Registers_ILogManager()
    {
        var resolver = new ModernDependencyResolver();
        var module = new NLogSplatModule();

        module.Configure(resolver);

        var logManager = resolver.GetService(typeof(ILogManager));

        await Assert.That(logManager).IsNotNull();
    }

    /// <summary>Verifies that the module's registered manager produces a usable logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Registered_Manager_Produces_Logger()
    {
        var resolver = new ModernDependencyResolver();
        var module = new NLogSplatModule();

        module.Configure(resolver);

        var logManager = (ILogManager)resolver.GetService(typeof(ILogManager))!;
        var logger = logManager.GetLogger(typeof(NLogSplatModuleCoverageTests));

        await Assert.That(logger).IsNotNull();
    }
}
