// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Moq;

namespace Splat.Builder.Tests
{
    /// <summary>
    /// Unit tests for AppBuilder.
    /// </summary>
    public class AppBuilderTests
    {
        /// <summary>
        /// Constructors the throws on null resolver.
        /// </summary>
        [Fact]
        public void ConstructorThrowsOnNullResolver()
        {
            Assert.Throws<ArgumentNullException>(() => new AppBuilder((IMutableDependencyResolver)null!));
        }

        /// <summary>
        /// Constructors the sets using builder true.
        /// </summary>
        [Fact]
        public void ConstructorSetsUsingBuilderTrue()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            Assert.True(AppBuilder.UsingBuilder);
        }

        /// <summary>
        /// Creates the splat builder returns builder.
        /// </summary>
        [Fact]
        public void CreateSplatBuilderReturnsBuilder()
        {
            var builder = AppBuilder.CreateSplatBuilder();
            Assert.NotNull(builder);
        }

        /// <summary>
        /// Resets the state of the builder state for tests resets static.
        /// </summary>
        [Fact]
        public void ResetBuilderStateForTestsResetsStaticState()
        {
            AppBuilder.ResetBuilderStateForTests();
            Assert.False(AppBuilder.HasBeenBuilt);
            Assert.False(AppBuilder.UsingBuilder);
        }

        /// <summary>
        /// Uses the current splat locator changes resolver provider.
        /// </summary>
        [Fact]
        public void UseCurrentSplatLocatorChangesResolverProvider()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            var result = builder.UseCurrentSplatLocator();
            Assert.Same(builder, result);
        }

        /// <summary>
        /// Usings the module throws on null module.
        /// </summary>
        [Fact]
        public void UsingModuleThrowsOnNullModule()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            Assert.Throws<ArgumentNullException>(() => builder.UsingModule<IModule>((IModule)null!));
        }

        /// <summary>
        /// Usings the module adds module.
        /// </summary>
        [Fact]
        public void UsingModuleAddsModule()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var module = new Mock<IModule>().Object;
            var builder = new AppBuilder(resolver);
            var result = builder.UsingModule(module);
            Assert.Same(builder, result);
        }

        /// <summary>
        /// Withes the custom registration throws on null action.
        /// </summary>
        [Fact]
        public void WithCustomRegistrationThrowsOnNullAction()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            Assert.Throws<ArgumentNullException>(() => builder.WithCustomRegistration((Action<IMutableDependencyResolver>)null!));
        }

        /// <summary>
        /// Withes the custom registration adds action.
        /// </summary>
        [Fact]
        public void WithCustomRegistrationAddsAction()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            var result = builder.WithCustomRegistration(r => { });
            Assert.Same(builder, result);
        }

        /// <summary>
        /// Withes the core services returns self.
        /// </summary>
        [Fact]
        public void WithCoreServicesReturnsSelf()
        {
            var resolver = new Mock<IMutableDependencyResolver>().Object;
            var builder = new AppBuilder(resolver);
            var result = builder.WithCoreServices();
            Assert.Same(builder, result);
        }

        /// <summary>
        /// Builds the applies registrations.
        /// </summary>
        [Fact]
        public void BuildAppliesRegistrations()
        {
            var resolverMock = new Mock<IMutableDependencyResolver>();
            var builder = new AppBuilder(resolverMock.Object);
            bool called = false;
            builder.WithCustomRegistration(r => called = true);
            builder.Build();
            Assert.True(called);
        }

        /// <summary>
        /// Builds the does nothing if already built.
        /// </summary>
        [Fact]
        public void BuildDoesNothingIfAlreadyBuilt()
        {
            var resolverMock = new Mock<IMutableDependencyResolver>();
            var builder = new AppBuilder(resolverMock.Object);
            builder.Build(); // sets HasBeenBuilt
            bool called = false;
            builder.WithCustomRegistration(r => called = true);
            builder.Build(); // should not call registration again
            Assert.False(called);
        }
    }
}
