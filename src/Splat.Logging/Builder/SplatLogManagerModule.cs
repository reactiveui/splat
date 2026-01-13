// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Provides a dependency injection module that registers a default implementation of the ILogManager interface using
/// the specified dependency resolver context.
/// </summary>
/// <param name="current">The dependency resolver used to resolve dependencies when creating the default ILogManager instance. Cannot be null.</param>
public sealed class SplatLogManagerModule(IReadonlyDependencyResolver current) : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        if (!resolver.HasRegistration<ILogManager>())
        {
            resolver.Register<ILogManager>(() => new DefaultLogManager(current));
        }
    }
}
