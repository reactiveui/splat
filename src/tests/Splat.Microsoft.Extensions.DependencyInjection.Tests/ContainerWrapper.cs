// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests;

internal sealed class ContainerWrapper
{
    private IServiceProvider _serviceProvider;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ContainerWrapper() => ServiceCollection.UseMicrosoftDependencyResolver();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

    public IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider();

    public void BuildAndUse() => ServiceProvider.UseMicrosoftDependencyResolver();
}
