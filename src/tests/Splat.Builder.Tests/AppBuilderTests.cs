// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Builder.Tests;

[NotInParallel]
public class AppBuilderTests
{
    private AppBuilderScope? _appBuilderScope;

    /// <summary>
    /// Setup method to initialize AppBuilderScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpAppBuilderScope() => _appBuilderScope = new();

    /// <summary>
    /// Teardown method to dispose AppBuilderScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownAppBuilderScope()
    {
        _appBuilderScope?.Dispose();
        _appBuilderScope = null;
    }

    /// <summary>
    /// Constructors the throws on null resolver.
    /// </summary>
    [Test]
    public void ConstructorThrowsOnNullResolver() => Assert.Throws<ArgumentNullException>(() => _ = new AppBuilder(null!));

    /// <summary>
    /// Constructors the sets using builder true.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ConstructorSetsUsingBuilderTrue()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        await Assert.That(AppBuilder.UsingBuilder).IsTrue();
        resolver.Dispose();
    }

    /// <summary>
    /// Creates the splat builder returns builder.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderReturnsBuilder()
    {
        var builder = AppBuilder.CreateSplatBuilder();
        await Assert.That(builder).IsNotNull();
    }

    /// <summary>
    /// Resets the state of the builder state for tests resets static.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResetBuilderStateForTestsResetsStaticState()
    {
        AppBuilder.ResetBuilderStateForTests();
        using (Assert.Multiple())
        {
            await Assert.That(AppBuilder.HasBeenBuilt).IsFalse();
            await Assert.That(AppBuilder.UsingBuilder).IsFalse();
        }
    }

    /// <summary>
    /// Uses the current splat locator changes resolver provider.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseCurrentSplatLocatorChangesResolverProvider()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        var result = builder.UseCurrentSplatLocator();
        await Assert.That(result).IsSameReferenceAs(builder);
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UsingModuleAddsModule()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        var result = builder.UsingModule(new MockModule());
        await Assert.That(result).IsSameReferenceAs(builder);
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
        Assert.Throws<ArgumentNullException>(() =>
            builder.WithCustomRegistration(null!));
        resolver.Dispose();
    }

    /// <summary>
    /// Withes the custom registration adds action.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithCustomRegistrationAddsAction()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        var result = builder.WithCustomRegistration(_ => { });
        await Assert.That(result).IsSameReferenceAs(builder);
        resolver.Dispose();
    }

    /// <summary>
    /// Withes the core services returns self.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithCoreServicesReturnsSelf()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        var result = builder.WithCoreServices();
        await Assert.That(result).IsSameReferenceAs(builder);
        resolver.Dispose();
    }

    /// <summary>
    /// Builds the applies registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BuildAppliesRegistrations()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        bool called = false;
        builder.WithCustomRegistration(_ => called = true);
        builder.Build();
        await Assert.That(called).IsTrue();
        resolver.Dispose();
    }

    /// <summary>
    /// Builds the does nothing if already built.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BuildDoesNothingIfAlreadyBuilt()
    {
        var resolver = new InternalLocator();
        var builder = new AppBuilder(resolver.CurrentMutable);
        builder.Build(); // sets HasBeenBuilt
        bool called = false;
        builder.WithCustomRegistration(_ => called = true);
        builder.Build(); // should not call registration again
        await Assert.That(called).IsFalse();
        resolver.Dispose();
    }
}
