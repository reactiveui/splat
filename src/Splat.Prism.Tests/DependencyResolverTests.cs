// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Shouldly;
using Splat.Common.Test;
using Splat.Prism;
using Xunit;

namespace Splat.Prism.Tests
{
    /// <summary>
    /// Tests to show the <see cref="PrismDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Should resolve the views.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_Resolve_Views()
        {
            var container = new SplatContainerExtension();
            container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));
            container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo));

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
        public void PrismDependencyResolver_Should_Resolve_Named_View()
        {
            var container = new SplatContainerExtension();
            container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo), "Other");

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Should resolve the view models.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_Resolve_View_Models()
        {
            var container = new SplatContainerExtension();

            container.Register(typeof(ViewModelOne), typeof(ViewModelOne));
            container.Register(typeof(ViewModelTwo), typeof(ViewModelTwo));

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.ShouldNotBeNull();
            vmTwo.ShouldNotBeNull();
        }

        /// <summary>
        /// Should resolve the screen.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_Resolve_Screen()
        {
            var builder = new SplatContainerExtension();
            builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_UnregisterCurrent_Screen()
        {
            var builder = new SplatContainerExtension();
            builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

            Locator.Current.GetService<IScreen>().ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

            Locator.Current.GetService<IScreen>().ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
        {
            var builder = new SplatContainerExtension();
            builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_UnregisterAll_Screen()
        {
            var builder = new SplatContainerExtension();
            builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

            Locator.Current.GetService<IScreen>().ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

            Locator.Current.GetService<IScreen>().ShouldBeNull();
        }

        /// <summary>
        /// Should unregister the screen.
        /// </summary>
        [Fact]
        public void PrismDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
        {
            var builder = new SplatContainerExtension();
            builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldNotBeNull();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

            Locator.Current.GetService<IScreen>(nameof(MockScreen)).ShouldBeNull();
        }

        /// <summary>
        /// Check to ensure the correct logger is returned.
        /// </summary>
        /// <remarks>
        /// Introduced for Splat #331.
        /// </remarks>
        [Fact]
        public void PrismDependencyResolver_Should_ReturnRegisteredLogger()
        {
            var c = new SplatContainerExtension();
            c.Register(typeof(ILogger), typeof(ConsoleLogger));
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
        public void PrismDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
        {
            var c = new SplatContainerExtension();
            c.RegisterInstance(typeof(ILogManager), new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

            var d = Splat.Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(d);
        }
    }
}
