// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;
using Splat.NLog;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Common tests for Dependency Resolver interaction with Splat.
/// </summary>
/// <typeparam name="T">The dependency resolver to test.</typeparam>
[NotInParallel]
public abstract class BaseDependencyResolverTests<T>
    where T : IDependencyResolver
{
    private AppLocatorScope? _appLocatorScope;

    /// <summary>
    /// Setup method to initialize AppLocatorScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpAppLocatorScope()
    {
        _appLocatorScope = new AppLocatorScope();
    }

    /// <summary>
    /// Teardown method to dispose AppLocatorScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownAppLocatorScope()
    {
        _appLocatorScope?.Dispose();
        _appLocatorScope = null;
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(type);
            resolver.UnregisterCurrent(type);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure UnregisterCurrent removes last entry.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Remove_Last()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new FuncLogManager(_ => new WrappingFullLogger(new DebugLogger())), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        var service = resolver.GetService(type);
        await Assert.That(service).IsTypeOf<FuncLogManager>();

        resolver.UnregisterCurrent(type);

        service = resolver.GetService(type);
        await Assert.That(service).IsTypeOf<DefaultLogManager>();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        await Assert.That(() =>
        {
            resolver.UnregisterAll(type);
            resolver.UnregisterCurrent(type);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        await Assert.That(() =>
        {
            resolver.UnregisterAll(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Ensures <see cref="IReadonlyDependencyResolver.GetServices(Type, string)"/> never returns null.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_Should_Never_Return_Null()
    {
        var resolver = GetDependencyResolver();

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetServices<string>()).IsNotNull();
            await Assert.That(resolver.GetServices<string>("Landscape")).IsNotNull();
        }
    }

    /// <summary>
    /// Tests for ensuring hasregistration behaves when using contracts.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration()
    {
        var type = typeof(string);
        const string contractOne = "ContractOne";
        const string contractTwo = "ContractTwo";
        var resolver = GetDependencyResolver();

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.Register(() => "unnamed", type);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.UnregisterAll(type);

        resolver.Register(() => contractOne, type, contractOne);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.UnregisterAll(type, contractOne);

        resolver.Register(() => contractTwo, type, contractTwo);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsTrue();
        }
    }

    /// <summary>
    /// Tests to ensure NLog registers correctly with different service locators.
    /// Based on issue reported in #553.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task ILogManager_Resolvable()
    {
        var resolver = GetDependencyResolver();

        // NOTE: MicrosoftDependencyResolver test for this functionality is in DependencyResolverTests
        if (resolver.GetType().Name != "MicrosoftDependencyResolver")
        {
            resolver.UseNLogWithWrappingFullLogger();
            AppLocator.SetLocator(resolver);
            await Task.Delay(10);
            AppLocator.CurrentMutable.InitializeSplat();
            await Task.Delay(10);

            var lm = AppLocator.Current.GetService<ILogManager>();
            await Assert.That(lm).IsNotNull();

            var mgr = lm!.GetLogger<NLogLogger>();
            await Assert.That(mgr).IsNotNull();
        }
    }

    /// <summary>
    /// Nulls the resolver tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NullResolverTests()
    {
        IReadonlyDependencyResolver? resolver = null;
        IMutableDependencyResolver? resolver1 = null;
        IDependencyResolver? resolver2 = null;

        await Assert.That(() => resolver!.GetService<ILogManager>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver!.GetServices<ILogManager>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.ServiceRegistrationCallback(typeof(ILogManager), d => d.Dispose())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver2!.WithResolver().Dispose()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.Register<ILogManager>(() => new DefaultLogManager(AppLocator.Current))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstant<ILogManager>(new DefaultLogManager(AppLocator.Current))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingleton(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingleton(() => new DefaultLogManager(AppLocator.Current))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd<ViewModelOne>("eight")).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd<DefaultLogManager>(() => new(AppLocator.Current), "seven")).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.UnregisterCurrent<ILogManager>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.UnregisterAll<ILogManager>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd<ViewModelOne>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd(() => new DefaultLogManager(AppLocator.Current))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd<IViewModelOne>(() => new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.Register<IViewModelOne, ViewModelOne>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd(new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd(new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd<ViewModelOne>()).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Registers the and tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndTests()
    {
        var resolver = GetDependencyResolver();

        await Assert.That(() => resolver.RegisterAnd<IViewModelOne>(null!)).Throws<ArgumentNullException>();

        resolver.RegisterAnd<ViewModelOne>("one")
                .RegisterAnd<IViewModelOne, ViewModelOne>("two")
                .RegisterAnd(() => new DefaultLogManager(AppLocator.Current), "three")
                .RegisterAnd<IViewModelOne>(() => new ViewModelOne(), "four")
                .RegisterConstantAnd<ViewModelOne>("five")
                .RegisterConstantAnd(new ViewModelOne(), "six")
                .RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager), "seven")
                .RegisterLazySingletonAnd<ViewModelOne>("eight")
                .RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), "seven")
                .Register<IViewModelOne, ViewModelOne>();
    }

    /// <summary>
    /// Gets an instance of a dependency resolver to test.
    /// </summary>
    /// <returns>Dependency Resolver.</returns>
    protected abstract T GetDependencyResolver();
}
