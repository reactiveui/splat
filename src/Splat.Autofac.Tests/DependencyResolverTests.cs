// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Autofac;

using FluentAssertions;

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
        /// Shoulds the resolve nulls.
        /// </summary>
        [Fact]
        public void Can_Register_And_Resolve_Null_Types()
        {
            var builder = new ContainerBuilder();
            var autofacResolver = builder.UseAutofacDependencyResolver();

            var foo = 5;
            Locator.CurrentMutable.Register(() => foo, null);

            var bar = 4;
            var contract = "foo";
            Locator.CurrentMutable.Register(() => bar, null, contract);
            autofacResolver.SetLifetimeScope(builder.Build());

            var value = Locator.Current.GetService(null);
            Assert.Equal(foo, value);

            value = Locator.Current.GetService(null, contract);
            Assert.Equal(bar, value);
        }

        /// <summary>
        /// Shoulds the resolve views.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_Views()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
            builder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();

            var autofacResolver = builder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(builder.Build());

            var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewOne.Should().NotBeNull();
            viewOne.Should().BeOfType<ViewOne>();
            viewTwo.Should().NotBeNull();
            viewTwo.Should().BeOfType<ViewTwo>();
        }

        /// <summary>
        /// Shoulds the resolve views.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_Named_View()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");

            var autofacResolver = builder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(builder.Build());

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

            viewTwo.Should().NotBeNull();
            viewTwo.Should().BeOfType<ViewTwo>();
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

            var autofacResolver = builder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(builder.Build());

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.Should().NotBeNull();
            vmTwo.Should().NotBeNull();
        }

        /// <summary>
        /// Shoulds the resolve screen.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Resolve_Screen()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MockScreen>().As<IScreen>().SingleInstance();

            var autofacResolver = builder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(builder.Build());

            var screen = Locator.Current.GetService<IScreen>();

            screen.Should().NotBeNull();
            screen.Should().BeOfType<MockScreen>();
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void AutofacDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
        {
            var builder = new ContainerBuilder();

            var autofacResolver = builder.UseAutofacDependencyResolver();
            autofacResolver.SetLifetimeScope(builder.Build());

            var result = Record.Exception(() =>
                Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

            result.Should().BeOfType<NotImplementedException>();
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
            var builder = new ContainerBuilder();

            var autofacResolver = builder.UseAutofacDependencyResolver();

            Locator.CurrentMutable.RegisterConstant(
                new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())),
                typeof(ILogManager));

            autofacResolver.SetLifetimeScope(builder.Build());

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
            var builder = new ContainerBuilder();

            var autofacResolver = builder.UseAutofacDependencyResolver();

            builder.Register(_ => new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger()))).As(typeof(ILogManager))
                .AsImplementedInterfaces();

            autofacResolver.SetLifetimeScope(builder.Build());

            var logManager = Locator.Current.GetService<ILogManager>();
            Assert.IsType<FuncLogManager>(logManager);
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
        /// </summary>
        [Fact]
        public override void UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
        /// </summary>
        [Fact]
        public override void UnregisterCurrent_Remove_Last()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
        /// </summary>
        [Fact]
        public override void UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
        /// </summary>
        [Fact]
        public override void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
        /// </summary>
        [Fact]
        public override void UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
        }

        /// <summary>
        ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
        ///     <inheritdoc cref="BaseDependencyResolverTests{T}.HasRegistration"/>
        /// </summary>
        [Fact]
        public override void HasRegistration()
        {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <inheritdoc />
        protected override AutofacDependencyResolver GetDependencyResolver()
        {
            var builder = new ContainerBuilder();

            return builder.UseAutofacDependencyResolver();
        }
    }
}
