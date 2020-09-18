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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
            containerBuilder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(containerBuilder.Build());

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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(containerBuilder.Build());

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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ViewModelOne>().AsSelf();
            containerBuilder.RegisterType<ViewModelTwo>().AsSelf();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(containerBuilder.Build());

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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<MockScreen>().As<IScreen>().SingleInstance();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(containerBuilder.Build());

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
            var containerBuilder = new ContainerBuilder();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(containerBuilder.Build());

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
            var containerBuilder = new ContainerBuilder();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();

            Locator.CurrentMutable.RegisterConstant(
                new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())),
                typeof(ILogManager));

            autofacResolver.SetLifetimeScope(containerBuilder.Build());

            var logManager = Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(logManager);
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
            var containerBuilder = new ContainerBuilder();

            var autofacResolver = containerBuilder.UseAutofacDependencyResolver();

            containerBuilder.Register(_ => new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger()))).As(typeof(ILogManager))
                .AsImplementedInterfaces();

            autofacResolver.SetLifetimeScope(containerBuilder.Build());

            var logManager = Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(logManager);
        }

        /// <summary>
        ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
        ///     UnregisterAll method is not available anymore.
        /// </summary>
        [Fact]
        public override void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
        ///     <inheritdoc cref="BaseDependencyResolverTests{T}.HasRegistration"/>
        /// </summary>
        [Fact]
        public override void HasRegistration()
        {
            var type = typeof(string);
            const string contractOne = "ContractOne";
            const string contractTwo = "ContractTwo";
            var resolver = GetDependencyResolver();

            Assert.False(resolver.HasRegistration(type));
            Assert.False(resolver.HasRegistration(type, contractOne));
            Assert.False(resolver.HasRegistration(type, contractTwo));

            resolver.Register(() => "unnamed", type);
            Assert.True(resolver.HasRegistration(type));
            Assert.False(resolver.HasRegistration(type, contractOne));
            Assert.False(resolver.HasRegistration(type, contractTwo));

            resolver.Register(() => contractOne, type, contractOne);
            Assert.True(resolver.HasRegistration(type));
            Assert.True(resolver.HasRegistration(type, contractOne));
            Assert.False(resolver.HasRegistration(type, contractTwo));

            resolver.Register(() => contractTwo, type, contractTwo);
            Assert.True(resolver.HasRegistration(type));
            Assert.True(resolver.HasRegistration(type, contractOne));
            Assert.True(resolver.HasRegistration(type, contractTwo));
        }

        /// <inheritdoc />
        protected override AutofacDependencyResolver GetDependencyResolver()
        {
            var containerBuilder = new ContainerBuilder();

            return containerBuilder.UseAutofacDependencyResolver();
        }
    }
}
