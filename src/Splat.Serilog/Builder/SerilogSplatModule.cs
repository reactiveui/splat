// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Serilog;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Serilog logger.
/// </summary>
/// <seealso cref="IModule" />
/// <remarks>
/// Initializes a new instance of the <see cref="SerilogSplatModule" /> class.
/// </remarks>
public sealed class SerilogSplatModule(global::Serilog.ILogger? actualLogger) : IModule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SerilogSplatModule"/> class.
    /// </summary>
    public SerilogSplatModule()
        : this(null)
    {
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        if (actualLogger is not null)
        {
            resolver.UseSerilogFullLogger(actualLogger);
            return;
        }

        resolver.UseSerilogFullLogger();
    }
}
