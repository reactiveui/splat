// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder.Tests
{
    /// <summary>
    /// Unit tests for SplatBuilderExtensions.
    /// </summary>
    public class SplatBuilderExtensionsTests
    {
        /// <summary>
        /// Applies the throws on null module.
        /// </summary>
        [Fact]
        public void ApplyThrowsOnNullModule()
        {
            IModule module = null!;
            Assert.Throws<ArgumentNullException>(() => module.Apply());
        }

        /// <summary>
        /// Creates the splat builder throws on null resolver.
        /// </summary>
        [Fact]
        public void CreateSplatBuilderThrowsOnNullResolver()
        {
            IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
            Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder());
        }

        /// <summary>
        /// Creates the splat builder returns application builder.
        /// </summary>
        [Fact]
        public void CreateSplatBuilderReturnsAppBuilder()
        {
            var resolver = new InternalLocator();
            var builder = resolver.CurrentMutable.CreateSplatBuilder();
            Assert.NotNull(builder);
        }

        /// <summary>
        /// Creates the splat builder with configure action throws on null resolver.
        /// </summary>
        [Fact]
        public void CreateSplatBuilderWithConfigureActionThrowsOnNullResolver()
        {
            IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
            Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder(r => { }));
        }

        /// <summary>
        /// Creates the splat builder with configure action returns application builder.
        /// </summary>
        [Fact]
        public void CreateSplatBuilderWithConfigureActionReturnsAppBuilder()
        {
            var resolver = new InternalLocator();
            var builder = resolver.CurrentMutable.CreateSplatBuilder(r => { });
            Assert.NotNull(builder);
        }
    }
}
