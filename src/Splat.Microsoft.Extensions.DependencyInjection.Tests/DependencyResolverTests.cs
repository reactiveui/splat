// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Splat.Common.Test;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests
{
    /// <summary>
    /// Tests to show the <see cref="MicrosoftDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Should resolve views.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Resolve_Views()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;
            services.AddTransient<IViewFor<ViewModelOne>, ViewOne>();
            services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

            wrapper.BuildAndUse();

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
        public void MicrosoftDependencyResolver_Should_Resolve_Named_View()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;
            services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

            wrapper.BuildAndUse();

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Should resolve view models.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Resolve_View_Models()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;
            services.AddTransient<ViewModelOne>();
            services.AddTransient<ViewModelTwo>();

            wrapper.BuildAndUse();

            var vmOne = Locator.Current.GetService<ViewModelOne>();
            var vmTwo = Locator.Current.GetService<ViewModelTwo>();

            vmOne.ShouldNotBeNull();
            vmTwo.ShouldNotBeNull();
        }

        /// <summary>
        /// Should resolve screen.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Resolve_Screen()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;
            services.AddSingleton<IScreen>(new MockScreen());

            wrapper.BuildAndUse();

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Throw_If_UnregisterCurrent_Called()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;
            wrapper.BuildAndUse();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen)));

            result.ShouldBeOfType<NotImplementedException>();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_UnregisterAll()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;

            services.AddSingleton<IScreen>(new MockScreen());

            Locator.CurrentMutable.HasRegistration(typeof(IScreen))
                .ShouldBeTrue();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

            var result = Record.Exception(() => Locator.Current.GetService<IScreen>());

            result.ShouldBeOfType<InvalidOperationException>();
            result.Message.ShouldStartWith("No service for type ");
        }

        /// <summary>
        /// Test that scopes can be registered and unregistered.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Enable_Scoped_Services()
        {
            var wrapper = new ServicesWrapper();
            var services = wrapper.ServiceCollection;

            services.AddScoped<ViewOne>();

            wrapper.BuildAndUse();

            var view1 = Locator.Current.GetService<ViewOne>("foo");
            var view2 = Locator.Current.GetService<ViewOne>("foo");
            var view3 = Locator.Current.GetService<ViewOne>("bar");

            Assert.True(ReferenceEquals(view1, view2));
            Assert.False(ReferenceEquals(view1, view3));

            Locator.CurrentMutable.UnregisterAll(null, "foo");
            view2 = Locator.Current.GetService<ViewOne>("foo");
            Assert.False(ReferenceEquals(view1, view2));
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
        {
            var wrapper = new ServicesWrapper();
            wrapper.BuildAndUse();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

            result.ShouldBeOfType<NotImplementedException>();
        }

        /// <summary>
        /// Should throw an exception if trying to register services when the container is registered as immutable.
        /// </summary>
        public void MicrosoftDependencyResolver_Should_Throw_If_Attempt_Registration_After_Build()
        {
            var wrapper = new ServicesWrapper();

            wrapper.BuildAndUse();

            var result = Record.Exception(() => Locator.CurrentMutable.Register(() => new ViewOne()));

            result.ShouldBeOfType<InvalidOperationException>();
        }

        private class ServicesWrapper
        {
            private IServiceProvider _serviceProvider;

            public ServicesWrapper()
            {
                ServiceCollection.UseMicrosoftDependencyResolver();
            }

            public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

            public IServiceProvider ServiceProvider
            {
                get
                {
                    if (_serviceProvider == null)
                    {
                        _serviceProvider = ServiceCollection.BuildServiceProvider();
                    }

                    return _serviceProvider;
                }
            }

            public void BuildAndUse() => ServiceProvider.UseMicrosoftDependencyResolver();
        }
    }
}
