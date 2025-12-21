// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Splat Logger.
/// </summary>
/// <seealso cref="IModule" />
/// <remarks>
/// Initializes a new instance of the <see cref="SplatLoggerModule"/> class.
/// </remarks>
public sealed class SplatLoggerModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(resolver);
#else
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }
#endif

        if (!resolver.HasRegistration(typeof(ILogger)))
        {
            var debugLogger = new DebugLogger();
            resolver.Register<ILogger>(() => debugLogger);
        }
    }
}
