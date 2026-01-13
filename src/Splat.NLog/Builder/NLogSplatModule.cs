// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.NLog;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the NLog logger.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NLogSplatModule"/> class.
/// </remarks>
public sealed class NLogSplatModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseNLogWithWrappingFullLogger();
}
