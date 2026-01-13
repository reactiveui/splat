// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.NLog;

namespace Splat.Builder;

/// <summary>
/// Provides an Autofac module that configures NLog as the logging backend for Splat-based applications.
/// </summary>
/// <remarks>This module enables integration of NLog with Splat by registering the necessary logging services. Add
/// this module to your dependency injection container to ensure that Splat uses NLog for logging throughout the
/// application.</remarks>
public sealed class NLogSplatModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseNLogWithWrappingFullLogger();
}
