// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Application builder interface for configuring dependency injection and services.
/// </summary>
public interface IAppBuilder
{
    /// <summary>
    /// Builds this instance.
    /// </summary>
    /// <returns>An App Instance.</returns>
    IAppInstance Build();

    /// <summary>
    /// Uses the current splat locator.
    /// </summary>
    /// <returns>The builder instance for chaining.</returns>
    IAppBuilder UseCurrentSplatLocator();

    /// <summary>
    /// Uses the module.
    /// </summary>
    /// <typeparam name="T">The Splat Module Type.</typeparam>
    /// <param name="registrationModule">The registration module.</param>
    /// <returns>The builder instance for chaining.</returns>
    IAppBuilder UsingModule<T>(T registrationModule)
        where T : IModule;

    /// <summary>
    /// Configures the core services.
    /// </summary>
    /// <returns>The builder instance for chaining.</returns>
    IAppBuilder WithCoreServices();

    /// <summary>
    /// Configures with custom registration.
    /// </summary>
    /// <param name="configureAction">The configure action.</param>
    /// <returns>The builder instance for chaining.</returns>
    IAppBuilder WithCustomRegistration(Action<IMutableDependencyResolver> configureAction);
}
