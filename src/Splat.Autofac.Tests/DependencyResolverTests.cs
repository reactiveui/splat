// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Autofac;
using Shouldly;
using Splat.Common.Test;
using Splat.Tests.ServiceLocation;
using Xunit;

namespace Splat.Autofac.Tests
{
    /// <summary>
    /// Tests to show the <see cref="AutofacDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests : BaseDependencyResolverTests<AutofacDependencyResolver>
    {
        /// <summary>
        /// Shoulds the resolve views.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_Views()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
            builder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();
            builder.Build().UseAutofacDependencyResolver();

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
        public void AutofacDependencyResolver_Should_Resolve_Named_View()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");
            builder.Build().UseAutofacDependencyResolver();

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Shoulds the resolve view models.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_View_Models()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewModelOne>().AsSelf();
            builder.RegisterType<ViewModelTwo>().AsSelf();
            builder.Build().UseAutofacDependencyResolver();

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.ShouldNotBeNull();
            vmTwo.ShouldNotBeNull();
        }

        /// <summary>
        /// Shoulds the resolve screen.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_Screen()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MockScreen>().As<IScreen>().SingleInstance();
            builder.Build().UseAutofacDependencyResolver();

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
        {
            var container = new ContainerBuilder();
            container.UseAutofacDependencyResolver();

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
        public void AutofacDependencyResolver_Should_ReturnRegisteredLogger()
        {
            var container = new ContainerBuilder();
            container.UseAutofacDependencyResolver();

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
        public void AutofacDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
        {
            var builder = new ContainerBuilder();
            builder.Register(_ => new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger()))).As(typeof(ILogManager))
                .AsImplementedInterfaces();

            builder.UseAutofacDependencyResolver();

            var d = Splat.Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(d);
        }

        /// <inheritdoc />
        protected override AutofacDependencyResolver GetDependencyResolver()
        {
            var container = new ContainerBuilder();
            return new AutofacDependencyResolver(container.Build());
        }
    }
}
