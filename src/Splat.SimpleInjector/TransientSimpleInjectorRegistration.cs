// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq.Expressions;

using SimpleInjector;

namespace Splat.SimpleInjector;

internal sealed class TransientSimpleInjectorRegistration(Container container, Type implementationType, Func<object?>? instanceCreator = null) : Registration(Lifestyle.Transient, container, implementationType, instanceCreator!)
{
    public override Expression BuildExpression() => BuildTransientExpression();
}
