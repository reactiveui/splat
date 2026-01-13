// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq.Expressions;

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>
/// Represents a transient registration for a service type in a Simple Injector container.
/// </summary>
/// <remarks>This registration ensures that a new instance of the implementation type is created each time the
/// service is requested from the container. Use this class when you want transient lifestyle behavior for a service in
/// Simple Injector.</remarks>
/// <param name="container">The container in which the registration is made. Cannot be null.</param>
/// <param name="implementationType">The concrete type that will be instantiated for each request. Cannot be null.</param>
/// <param name="instanceCreator">An optional delegate used to create instances of the implementation type. If null, the container will use its
/// default instantiation mechanism.</param>
internal sealed class TransientSimpleInjectorRegistration(Container container, Type implementationType, Func<object?>? instanceCreator = null) : Registration(Lifestyle.Transient, container, implementationType, instanceCreator!)
{
    public override Expression BuildExpression() => BuildTransientExpression();
}
