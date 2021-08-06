// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using FluentAssertions;
using SimpleInjector;
using Splat;
using Splat.SimpleInjector;
using Xunit;

namespace ReactiveUI.DI.Tests
{
    /// <summary>
    /// SimpleInjector ReactiveUI Dependency Tests.
    /// </summary>
    public class SimpleInjectorReactiveUIDependencyTests
    {
        /// <summary>
        /// Should register ReactiveUI binding type converters.
        /// </summary>
        [Fact]
        public void SimpleInjectorDependencyResolverShouldRegisterReactiveUIBindingTypeConverters()
        {
            // Invoke RxApp which initializes the ReactiveUI platform.
            Container container = new();
            SimpleInjectorInitializer initializer = new();

            Locator.SetLocator(initializer);
            Locator.CurrentMutable.InitializeReactiveUI();
            container.UseSimpleInjectorDependencyResolver(initializer);
            var converters = Locator.Current.GetServices<IBindingTypeConverter>().ToList();

            converters.Should().NotBeNull();
            converters.Should().Contain(x => x.GetType() == typeof(StringConverter));
            converters.Should().Contain(x => x.GetType() == typeof(EqualityTypeConverter));
        }

        /// <summary>
        /// Should register ReactiveUI creates command bindings.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolverShouldRegisterReactiveUICreatesCommandBinding()
        {
            // Invoke RxApp which initializes the ReactiveUI platform.
            Container container = new();
            SimpleInjectorInitializer initializer = new();

            Locator.SetLocator(initializer);
            Locator.CurrentMutable.InitializeReactiveUI();
            container.UseSimpleInjectorDependencyResolver(initializer);

            var converters = Locator.Current.GetServices<ICreatesCommandBinding>().ToList();

            converters.Should().NotBeNull();
            converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaEvent));
            converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaCommandParameter));
        }
    }
}
