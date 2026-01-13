// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Serilog;

namespace Splat.Builder;

/// <summary>
/// Provides a ReactiveUI module that configures Serilog as the logging implementation for Splat, optionally using a
/// specified Serilog logger instance.
/// </summary>
/// <remarks>Use this module to integrate Serilog with Splat's logging infrastructure in ReactiveUI applications.
/// If an explicit Serilog logger is provided, it will be used for all Splat logging; otherwise, the default Serilog
/// logger configuration is applied.</remarks>
/// <param name="actualLogger">The Serilog logger instance to use for logging. If null, a default logger will be configured.</param>
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
