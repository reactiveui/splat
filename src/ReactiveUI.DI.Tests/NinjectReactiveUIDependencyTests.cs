// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using FluentAssertions;
using Ninject;
using Splat;
using Splat.Ninject;
using Xunit;

namespace ReactiveUI.DI.Tests
{
    /// <summary>
    /// Ninject ReactiveUI Dependency Tests.
    /// </summary>
    public class NinjectReactiveUIDependencyTests
    {
        /// <summary>
        /// Should register ReactiveUI binding type converters.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolverShouldRegisterReactiveUIBindingTypeConverters()
        {
            // Invoke RxApp which initializes the ReactiveUI platform.
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();
            Locator.CurrentMutable.InitializeReactiveUI();

            var converters = container.GetAll<IBindingTypeConverter>().ToList();

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
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();
            Locator.CurrentMutable.InitializeReactiveUI();

            var converters = container.GetAll<ICreatesCommandBinding>().ToList();

            converters.Should().NotBeNull();
            converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaEvent));
            converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaCommandParameter));
        }
    }
}
