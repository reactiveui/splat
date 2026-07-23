// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests;

/// <summary>Test helper that wraps a service collection and its built service provider.</summary>
internal sealed class ContainerWrapper
{
    /// <summary>The lazily built service provider.</summary>
    private IServiceProvider? _serviceProvider;

    /// <summary>Initializes a new instance of the <see cref="ContainerWrapper"/> class.</summary>
    public ContainerWrapper() => ServiceCollection.UseMicrosoftDependencyResolver();

    /// <summary>Gets the underlying service collection.</summary>
    internal IServiceCollection ServiceCollection { get; } = new ServiceCollection();

    /// <summary>Gets the service provider, building it on first access.</summary>
    internal IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider();

    /// <summary>Builds the service provider and registers it as the Microsoft dependency resolver.</summary>
    internal void BuildAndUse() => ServiceProvider.UseMicrosoftDependencyResolver();
}
