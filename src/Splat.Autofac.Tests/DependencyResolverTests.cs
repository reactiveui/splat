// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Shouldly;
using Splat.Common.Testing;
using Xunit;

namespace Splat.Autofac.Tests
{
    /// <summary>
    /// Tests to show the <see cref="AutofacDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
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
    }
}
