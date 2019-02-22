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
        /// Shoulds the resolve views.
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
        /// Shoulds the resolve views.
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
        /// Shoulds the resolve view models.
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
        /// Shoulds the resolve screen.
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
    }
}
