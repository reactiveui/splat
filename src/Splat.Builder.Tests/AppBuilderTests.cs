// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder.Tests
{
    /// <summary>
    /// Unit tests for AppBuilder.
    /// </summary>
[TestFixture]
    public class AppBuilderTests
    {
        /// <summary>
        /// Constructors the throws on null resolver.
        /// </summary>
        [Test]
        public void ConstructorThrowsOnNullResolver()
        {
            Assert.Throws<ArgumentNullException>(() => new AppBuilder((IMutableDependencyResolver)null!));
        }

        /// <summary>
        /// Constructors the sets using builder true.
        /// </summary>
        [Test]
        public void ConstructorSetsUsingBuilderTrue()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            Assert.That(AppBuilder.UsingBuilder, Is.True);
            resolver.Dispose();
        }

        /// <summary>
        /// Creates the splat builder returns builder.
        /// </summary>
        [Test]
        public void CreateSplatBuilderReturnsBuilder()
        {
            var builder = AppBuilder.CreateSplatBuilder();
            Assert.That(builder, Is.Not.Null);
        }

        /// <summary>
        /// Resets the state of the builder state for tests resets static.
        /// </summary>
        [Test]
        public void ResetBuilderStateForTestsResetsStaticState()
        {
            AppBuilder.ResetBuilderStateForTests();
            Assert.That(AppBuilder.HasBeenBuilt, Is.False);
            Assert.That(AppBuilder.UsingBuilder, Is.False);
        }

        /// <summary>
        /// Uses the current splat locator changes resolver provider.
        /// </summary>
        [Test]
        public void UseCurrentSplatLocatorChangesResolverProvider()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            var result = builder.UseCurrentSplatLocator();
            Assert.That(result, Is.SameAs(builder));
            resolver.Dispose();
        }

        /// <summary>
        /// Usings the module throws on null module.
        /// </summary>
        [Test]
        public void UsingModuleThrowsOnNullModule()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            Assert.Throws<ArgumentNullException>(() => builder.UsingModule((IModule)null!));
            resolver.Dispose();
        }

        /// <summary>
        /// Usings the module adds module.
        /// </summary>
        [Test]
        public void UsingModuleAddsModule()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            var result = builder.UsingModule(new MokModule());
            Assert.That(result, Is.SameAs(builder));
            resolver.Dispose();
        }

        /// <summary>
        /// Withes the custom registration throws on null action.
        /// </summary>
        [Test]
        public void WithCustomRegistrationThrowsOnNullAction()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            Assert.Throws<ArgumentNullException>(() => builder.WithCustomRegistration((Action<IMutableDependencyResolver>)null!));
            resolver.Dispose();
        }

        /// <summary>
        /// Withes the custom registration adds action.
        /// </summary>
        [Test]
        public void WithCustomRegistrationAddsAction()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            var result = builder.WithCustomRegistration(r => { });
            Assert.That(result, Is.SameAs(builder));
            resolver.Dispose();
        }

        /// <summary>
        /// Withes the core services returns self.
        /// </summary>
        [Test]
        public void WithCoreServicesReturnsSelf()
        {
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            var result = builder.WithCoreServices();
            Assert.That(result, Is.SameAs(builder));
            resolver.Dispose();
        }

        /// <summary>
        /// Builds the applies registrations.
        /// </summary>
        [Test]
        public void BuildAppliesRegistrations()
        {
            AppBuilder.ResetBuilderStateForTests();
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            bool called = false;
            builder.WithCustomRegistration(r => called = true);
            builder.Build();
            Assert.That(called, Is.True);
            resolver.Dispose();
        }

        /// <summary>
        /// Builds the does nothing if already built.
        /// </summary>
        [Test]
        public void BuildDoesNothingIfAlreadyBuilt()
        {
            AppBuilder.ResetBuilderStateForTests();
            var resolver = new InternalLocator();
            var builder = new AppBuilder(resolver.CurrentMutable);
            builder.Build(); // sets HasBeenBuilt
            bool called = false;
            builder.WithCustomRegistration(r => called = true);
            builder.Build(); // should not call registration again
            Assert.That(called, Is.False);
            resolver.Dispose();
        }
    }

    internal sealed class MokModule : IModule
    {
        public void Configure(IMutableDependencyResolver resolver)
        {
            // This is a mock module for testing purposes.
            // It does not need to do anything specific.
            // In a real scenario, you would register services here.}
        }
    }
}
