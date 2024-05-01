// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using SimpleInjector;

namespace Splat.SimpleInjector;

internal sealed class TransientSimpleInjectorRegistration(Container container, Type implementationType, Func<object?>? instanceCreator = null) : Registration(Lifestyle.Transient, container, implementationType, instanceCreator!)
{
    public override Expression BuildExpression() => BuildTransientExpression();
}
