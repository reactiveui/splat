// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

using Splat.Common.Test;
using Splat.NLog;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests;

[NotInParallel]
public class DependencyResolverTests
{
/// <summary>
    /// Should resolve views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Views()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        services.AddTransient<IViewFor<ViewModelOne>, ViewOne>();
        services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

        wrapper.BuildAndUse();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Named_View()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        services.AddTransient<IViewFor<ViewModelTwo>, ViewTwo>();

        wrapper.BuildAndUse();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve view models.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_View_Models()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        services.AddTransient<ViewModelOne>();
        services.AddTransient<ViewModelTwo>();

        wrapper.BuildAndUse();

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.Multiple())
        {
            await Assert.That(vmOne).IsNotNull();
            await Assert.That(vmTwo).IsNotNull();
        }
    }

    /// <summary>
    /// Should resolve screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_Resolve_Screen()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;
        services.AddSingleton<IScreen>(new MockScreen());

        wrapper.BuildAndUse();

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>
    /// Should unregister all.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MicrosoftDependencyResolver_Should_UnregisterAll()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;

        services.AddSingleton<IScreen>(new MockScreen());

        await Assert.That(AppLocator.CurrentMutable.HasRegistration(typeof(IScreen))).IsTrue();

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        var result = AppLocator.Current.GetService<IScreen>();
        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Should throw an exception if service registration callback is called.
    /// </summary>
    [Test]
    public void MicrosoftDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
    {
        var wrapper = new ContainerWrapper();
        wrapper.BuildAndUse();

        Assert.Throws<NotImplementedException>(() =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), _ => { }));
    }

    /// <summary>
    /// Should throw an exception if trying to register services when the container is registered as immutable.
    /// </summary>
    [Test]
    public void MicrosoftDependencyResolver_Should_Throw_If_Attempt_Registration_After_Build()
    {
        var wrapper = new ContainerWrapper();
        wrapper.BuildAndUse();

        Assert.Throws<InvalidOperationException>(() =>
            AppLocator.CurrentMutable.Register(() => new ViewOne()));
    }

    /// <summary>
    /// Tests to ensure NLog registers correctly with different service locators.
    /// Based on issue reported in #553.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ILogManager_Resolvable()
    {
        var wrapper = new ContainerWrapper();
        var services = wrapper.ServiceCollection;

        // Setup NLog for Logging (doesn't matter if I actually configure NLog or not)
        var funcLogManager = new FuncLogManager(type => new NLogLogger(LogResolver.Resolve(type)));
        services.AddSingleton<ILogManager>(funcLogManager);

        wrapper.BuildAndUse();

        // Get the ILogManager instance.
        var lm = AppLocator.Current.GetService<ILogManager>();
        await Assert.That(lm).IsNotNull();

        var mgr = lm!.GetLogger<NLogLogger>();
        await Assert.That(mgr).IsNotNull();
    }
}
