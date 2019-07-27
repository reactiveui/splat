// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using Shouldly;
using Splat.Common.Test;
using Xunit;

namespace Splat.DryIoc.Tests
{
    /// <summary>
    /// Tests to show the <see cref="DryIocDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Should resolve the views.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_Resolve_Views()
        {
            var container = new Container();
            container.Register<IViewFor<ViewModelOne>, ViewOne>();
            container.Register<IViewFor<ViewModelTwo>, ViewTwo>();
            container.UseDryIocDependencyResolver();

            var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewOne.ShouldNotBeNull();
            viewOne.ShouldBeOfType<ViewOne>();
            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Should resolve the views.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_Resolve_Named_View()
        {
            var container = new Container();
            container.Register<IViewFor<ViewModelTwo>, ViewTwo>(serviceKey: "Other");
            container.UseDryIocDependencyResolver();

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Should resolve the view models.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_Resolve_View_Models()
        {
            var container = new Container();
            container.Register<ViewModelOne>();
            container.Register<ViewModelTwo>();
            container.UseDryIocDependencyResolver();

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.ShouldNotBeNull();
            vmTwo.ShouldNotBeNull();
        }

        /// <summary>
        /// Should resolve the screen.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_Resolve_Screen()
        {
            var builder = new Container();
            builder.Register<IScreen, MockScreen>(Reuse.Singleton);
            builder.UseDryIocDependencyResolver();

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_UnregisterCurrent_Screen()
        {
            var builder = new Container();
            builder.Register<IScreen, MockScreen>(Reuse.Singleton);
            builder.UseDryIocDependencyResolver();

            Locator.Current.GetService<IScreen>().ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

            Locator.Current.GetService<IScreen>().ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
        {
            var builder = new Container();
            builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
            builder.UseDryIocDependencyResolver();

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_UnregisterAll_Screen()
        {
            var builder = new Container();
            builder.Register<IScreen, MockScreen>(Reuse.Singleton);
            builder.UseDryIocDependencyResolver();

            Locator.Current.GetService<IScreen>().ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

            Locator.Current.GetService<IScreen>().ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
        {
            var builder = new Container();
            builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
            builder.UseDryIocDependencyResolver();

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldBeNull();
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void DryIocDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
        {
            var container = new Container();
            container.UseDryIocDependencyResolver();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

            result.ShouldBeOfType<NotImplementedException>();
        }

        /// <summary>
        /// Check to ensure the correct logger is returned.
        /// </summary>
        /// <remarks>
        /// Introduced for Splat #331.
        /// </remarks>
        [Fact]
        public void DryIocDependencyResolver_Should_ReturnRegisteredLogger()
        {
            var c = new Container();
            c.UseDryIocDependencyResolver();
            c.Register<ILogger, ConsoleLogger>(ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            Locator.CurrentMutable.RegisterConstant(
                new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())),
                typeof(ILogManager));

            var d = Splat.Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(d);
        }

        /// <summary>
        /// Test that a pre-init logger isn't overriden.
        /// </summary>
        /// <remarks>
        /// Introduced for Splat #331.
        /// </remarks>
        [Fact]
        public void DryIocDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
        {
            var c = new Container();
            c.UseInstance(typeof(ILogManager), new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));
            c.UseDryIocDependencyResolver();

            var d = Splat.Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(d);
        }
    }
}
