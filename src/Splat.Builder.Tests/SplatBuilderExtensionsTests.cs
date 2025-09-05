// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder.Tests
{
    /// <summary>
    /// Unit tests for SplatBuilderExtensions.
    /// </summary>
    [TestFixture]
    public class SplatBuilderExtensionsTests
    {
        /// <summary>
        /// Applies the throws on null module.
        /// </summary>
        [Test]
        public void ApplyThrowsOnNullModule()
        {
            IModule module = null!;
            Assert.Throws<ArgumentNullException>(() => module.Apply());
        }

        /// <summary>
        /// Creates the splat builder throws on null resolver.
        /// </summary>
        [Test]
        public void CreateSplatBuilderThrowsOnNullResolver()
        {
            IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
            Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder());
        }

        /// <summary>
        /// Creates the splat builder returns application builder.
        /// </summary>
        [Test]
        public void CreateSplatBuilderReturnsAppBuilder()
        {
            var resolver = new InternalLocator();
            var builder = resolver.CurrentMutable.CreateSplatBuilder();
            Assert.That(builder, Is.Not.Null);
            resolver.Dispose();
        }

        /// <summary>
        /// Creates the splat builder with configure action throws on null resolver.
        /// </summary>
        [Test]
        public void CreateSplatBuilderWithConfigureActionThrowsOnNullResolver()
        {
            IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
            Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder(r => { }));
        }

        /// <summary>
        /// Creates the splat builder with configure action returns application builder.
        /// </summary>
        [Test]
        public void CreateSplatBuilderWithConfigureActionReturnsAppBuilder()
        {
            var resolver = new InternalLocator();
            var builder = resolver.CurrentMutable.CreateSplatBuilder(r => r.Register(() => "Hello", typeof(string)))
                .Build();
            Assert.That(builder, Is.Not.Null);
            var hello = resolver.Current.GetService<string>();
            Assert.That(hello, Is.EqualTo("Hello"));
            resolver.Dispose();
            AppBuilder.ResetBuilderStateForTests();
        }
    }
}
