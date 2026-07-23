// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

using Splat.Common.Test;
using Splat.NLog;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests;

/// <summary>Tests for the Microsoft.Extensions.DependencyInjection dependency resolver.</summary>
[NotInParallel]
public class DependencyResolverTests
{
    /// <summary>Should resolve views.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Views()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        _ = services.AddTransient<IViewFor<ViewModelOne>, ViewOne>();
        _ = services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

        wrapper.BuildAndUse();

        var viewOne = AppLocator.Current.GetService<IViewFor<ViewModelOne>>();
        var viewTwo = AppLocator.Current.GetService<IViewFor<ViewModelTwo>>();

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>Should resolve views.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Named_View()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        _ = services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

        wrapper.BuildAndUse();

        var viewTwo = AppLocator.Current.GetService<IViewFor<ViewModelTwo>>();

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>Should resolve view models.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_View_Models()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        _ = services.AddTransient<ViewModelOne>();
        _ = services.AddTransient<ViewModelTwo>();

        wrapper.BuildAndUse();

        var viewModelOne = AppLocator.Current.GetService<ViewModelOne>();
        var viewModelTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.Multiple())
        {
            await Assert.That(viewModelOne).IsNotNull();
            await Assert.That(viewModelTwo).IsNotNull();
        }
    }

    /// <summary>Should resolve screen.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Screen()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        _ = services.AddSingleton<IScreen>(new MockScreen());

        wrapper.BuildAndUse();

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>Should unregister all.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_UnregisterAll()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;

        _ = services.AddSingleton<IScreen>(new MockScreen());

        await Assert.That(AppLocator.CurrentMutable.HasRegistration<IScreen>()).IsTrue();

        AppLocator.CurrentMutable.UnregisterAll<IScreen>();

        var result = AppLocator.Current.GetService<IScreen>();
        await Assert.That(result).IsNull();
    }

    /// <summary>Should throw an exception if service registration callback is called.</summary>
    [Test]
    public void MicrosoftDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
    {
        var wrapper = new ContainerWrapper();
        wrapper.BuildAndUse();

        _ = Assert.Throws<NotSupportedException>(static () =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), static _ => { }));
    }

    /// <summary>Should throw an exception if trying to register services when the container is registered as immutable.</summary>
    [Test]
    public void MicrosoftDependencyResolver_Should_Throw_If_Attempt_Registration_After_Build()
    {
        var wrapper = new ContainerWrapper();
        wrapper.BuildAndUse();

        _ = Assert.Throws<InvalidOperationException>(static () =>
            AppLocator.CurrentMutable.Register(static () => new ViewOne()));
    }

    /// <summary>Using the provider extension when the current locator is not a Microsoft resolver installs a new resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseMicrosoftDependencyResolver_FromProvider_WhenCurrentIsNotMicrosoftResolver_SetsNewResolver()
    {
        using var scope = new AppLocatorScope();

        await Assert.That(AppLocator.Current).IsNotTypeOf<MicrosoftDependencyResolver>();

        var services = new ServiceCollection();
        _ = services.AddSingleton<IScreen>(new MockScreen());
        services.BuildServiceProvider().UseMicrosoftDependencyResolver();

        await Assert.That(AppLocator.Current).IsTypeOf<MicrosoftDependencyResolver>();
        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsTypeOf<MockScreen>();
    }

    /// <summary>Tests to ensure NLog registers correctly with different service locators. Based on issue reported in #553.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ILogManager_Resolvable()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;

        // Setup NLog for Logging (doesn't matter if I actually configure NLog or not)
        var funcLogManager = new FuncLogManager(static type => new NLogLogger(LogResolver.Resolve(type)));
        _ = services.AddSingleton<ILogManager>(funcLogManager);

        wrapper.BuildAndUse();

        // Get the ILogManager instance.
        var lm = AppLocator.Current.GetService<ILogManager>();
        await Assert.That(lm).IsNotNull();

        var mgr = lm!.GetLogger<NLogLogger>();
        await Assert.That(mgr).IsNotNull();
    }
}
