// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using ReactiveUI;
using Shouldly;
using Xunit;

namespace Splat.Ninject.Tests
{
    /// <summary>
    /// Tests to show the <see cref="NinjectDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Shoulds the resolve views.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Resolve_Views()
        {
            var container = new StandardKernel();
            container.Bind<IViewFor<ViewModelOne>>().To<ViewOne>();
            container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
            container.UseNinjectDependencyResolver();

            var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewOne.ShouldNotBeNull();
            viewOne.ShouldBeOfType<ViewOne>();
            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Shoulds the resolve views.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Resolve_Named_View()
        {
            var container = new StandardKernel();
            container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
            container.UseNinjectDependencyResolver();

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Shoulds the resolve view models.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Resolve_View_Models()
        {
            var container = new StandardKernel();
            container.Bind<ViewModelOne>().ToSelf();
            container.Bind<ViewModelTwo>().ToSelf();
            container.UseNinjectDependencyResolver();

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.ShouldNotBeNull();
            vmTwo.ShouldNotBeNull();
        }

        /// <summary>
        /// Shoulds the resolve screen.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Resolve_Screen()
        {
            var container = new StandardKernel();
            container.Bind<IScreen>().ToConstant(new MockScreen());
            container.UseNinjectDependencyResolver();

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }

        /// <summary>
        /// Shoulds register ReactiveUI binding type converters.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Register_ReactiveUI_BindingTypeConverters()
        {
            // Invoke RxApp which initializes the ReactiveUI platform.
            var scheduler = RxApp.MainThreadScheduler;
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();

            var converters = container.GetAll<IBindingTypeConverter>().ToList();

            converters.ShouldNotBeNull();
            converters.ShouldContain(x => x.GetType() == typeof(StringConverter));
            converters.ShouldContain(x => x.GetType() == typeof(EqualityTypeConverter));
        }

        /// <summary>
        /// Shoulds register ReactiveUI creates command bindings.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Register_ReactiveUI_CreatesCommandBinding()
        {
            // Invoke RxApp which initializes the ReactiveUI platform.
            var scheduler = RxApp.MainThreadScheduler;
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();

            var converters = container.GetAll<ICreatesCommandBinding>().ToList();

            converters.ShouldNotBeNull();
            converters.ShouldContain(x => x.GetType() == typeof(CreatesCommandBindingViaEvent));
            converters.ShouldContain(x => x.GetType() == typeof(CreatesCommandBindingViaCommandParameter));
        }
    }
}
