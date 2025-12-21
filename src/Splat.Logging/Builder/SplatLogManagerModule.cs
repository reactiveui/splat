// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Splat Log Manager.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SplatLogManagerModule"/> class.
/// </remarks>
public sealed class SplatLogManagerModule(IReadonlyDependencyResolver current) : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        if (!resolver.HasRegistration(typeof(ILogManager)))
        {
            resolver.Register<ILogManager>(() => new DefaultLogManager(current));
        }
    }
}
