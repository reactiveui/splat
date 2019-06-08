// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Shouldly;
using Splat.Common.Test;
using Xunit;

namespace Splat.Ninject.Tests
{
    /// <summary>
    /// Tests to show the <see cref="NinjectDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Should resolve views.
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
        /// Should resolve views.
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
        /// Should resolve view models.
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
        /// Should resolve screen.
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
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Throw_If_UnregisterCurrent_Called()
        {
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen)));

            result.ShouldBeOfType<NotImplementedException>();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_UnregisterAll()
        {
            var container = new StandardKernel();
            container.Bind<IScreen>().ToConstant(new MockScreen());
            container.UseNinjectDependencyResolver();

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

            var result = Record.Exception(() => Locator.Current.GetService<IScreen>());

            result.ShouldBeOfType<ActivationException>();
            result.Message.ShouldStartWith("Error activating IScreen");
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void NinjectDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
        {
            var container = new StandardKernel();
            container.UseNinjectDependencyResolver();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

            result.ShouldBeOfType<NotImplementedException>();
        }
    }
}
