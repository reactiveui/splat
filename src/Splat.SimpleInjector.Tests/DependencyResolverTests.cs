// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using Shouldly;
using SimpleInjector;
using Splat.SimpleInjector;
using Xunit;

namespace Splat.Simplnjector
{
    /// <summary>
    /// Tests to show the <see cref="SimpleInjectorDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Simples the injector dependency resolver should resolve a view model.
        /// </summary>
        [Fact]
        public void SimpleInjectorDependencyResolver_Should_Resolve_View_Model()
        {
            var container = new Container();
            container.Register<ViewModel>();
            container.UseSimpleInjectorDependencyResolver();

            var viewModel = Locator.Current.GetService(typeof(ViewModel));

            viewModel.ShouldNotBeNull();
            viewModel.ShouldBeOfType<ViewModel>();
        }

        /// <summary>
        /// Simples the injector dependency resolver should resolve a view.
        /// </summary>
        [Fact]
        public void SimpleInjectorDependencyResolver_Should_Resolve_View()
        {
            var container = new Container();
            container.Register<IViewFor<ViewModel>, View>();
            container.UseSimpleInjectorDependencyResolver();

            var view = Locator.Current.GetService(typeof(IViewFor<ViewModel>));

            view.ShouldNotBeNull();
            view.ShouldBeOfType<View>();
        }

        /// <summary>
        /// Simples the injector dependency resolver should resolve the screen.
        /// </summary>
        [Fact]
        public void SimpleInjectorDependencyResolver_Should_Resolve_Screen()
        {
            var container = new Container();
            container.RegisterSingleton<IScreen, MockScreen>();
            container.UseSimpleInjectorDependencyResolver();

            var screen = Locator.Current.GetService(typeof(IScreen));

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();
        }
    }
}
