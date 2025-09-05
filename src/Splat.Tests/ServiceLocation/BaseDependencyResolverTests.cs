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
public abstract class BaseDependencyResolverTests<T>
    where T : IDependencyResolver
{
    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    [Test]
    public virtual void UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        Assert.DoesNotThrow(() =>
        {
            resolver.UnregisterCurrent(type);
            resolver.UnregisterCurrent(type);
        });
    }

    /// <summary>
    /// Test to ensure UnregisterCurrent removes last entry.
    /// </summary>
    [Test]
    public virtual void UnregisterCurrent_Remove_Last()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new FuncLogManager(_ => new WrappingFullLogger(new DebugLogger())), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        var service = resolver.GetService(type);
        Assert.That(service, Is.TypeOf<FuncLogManager>());

        resolver.UnregisterCurrent(type);

        service = resolver.GetService(type);
        Assert.That(service, Is.TypeOf<DefaultLogManager>());
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    [Test]
    public virtual void UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        Assert.DoesNotThrow(() =>
        {
            resolver.UnregisterCurrent(type, contract);
            resolver.UnregisterCurrent(type, contract);
        });
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    [Test]
    public virtual void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        Assert.DoesNotThrow(() =>
        {
            resolver.UnregisterAll(type);
            resolver.UnregisterCurrent(type);
        });
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    [Test]
    public virtual void UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        Assert.DoesNotThrow(() =>
        {
            resolver.UnregisterAll(type, contract);
            resolver.UnregisterCurrent(type, contract);
        });
    }

    /// <summary>
    /// Ensures <see cref="IReadonlyDependencyResolver.GetServices(Type, string)"/> never returns null.
    /// </summary>
    [Test]
    public void GetServices_Should_Never_Return_Null()
    {
        var resolver = GetDependencyResolver();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.GetServices<string>(), Is.Not.Null);
            Assert.That(resolver.GetServices<string>("Landscape"), Is.Not.Null);
        }
    }

    /// <summary>
    /// Tests for ensuring hasregistration behaves when using contracts.
    /// </summary>
    [Test]
    public virtual void HasRegistration()
    {
        var type = typeof(string);
        const string contractOne = "ContractOne";
        const string contractTwo = "ContractTwo";
        var resolver = GetDependencyResolver();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.False);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.False);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.False);
        }

        resolver.Register(() => "unnamed", type);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.True);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.False);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.False);
        }

        resolver.UnregisterAll(type);

        resolver.Register(() => contractOne, type, contractOne);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.False);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.True);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.False);
        }

        resolver.UnregisterAll(type, contractOne);

        resolver.Register(() => contractTwo, type, contractTwo);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.False);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.False);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.True);
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
            Assert.That(lm, Is.Not.Null);

            var mgr = lm!.GetLogger<NLogLogger>();
            Assert.That(mgr, Is.Not.Null);
        }
    }

    /// <summary>
    /// Nulls the resolver tests.
    /// </summary>
    [Test]
    public void NullResolverTests()
    {
        IReadonlyDependencyResolver? resolver = null;
        IMutableDependencyResolver? resolver1 = null;
        IDependencyResolver? resolver2 = null;

        Assert.Throws<ArgumentNullException>(() => resolver!.GetService<ILogManager>());
        Assert.Throws<ArgumentNullException>(() => resolver!.GetServices<ILogManager>());
        Assert.Throws<ArgumentNullException>(() => resolver1!.ServiceRegistrationCallback(typeof(ILogManager), d => d.Dispose()));
        Assert.Throws<ArgumentNullException>(() => resolver2!.WithResolver().Dispose());
        Assert.Throws<ArgumentNullException>(() => resolver1!.Register<ILogManager>(() => new DefaultLogManager(AppLocator.Current)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterConstant<ILogManager>(new DefaultLogManager(AppLocator.Current)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterLazySingleton(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterLazySingleton(() => new DefaultLogManager(AppLocator.Current)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterLazySingletonAnd<ViewModelOne>("eight"));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterLazySingletonAnd<DefaultLogManager>(() => new(AppLocator.Current), "seven"));
        Assert.Throws<ArgumentNullException>(() => resolver1!.UnregisterCurrent<ILogManager>());
        Assert.Throws<ArgumentNullException>(() => resolver1!.UnregisterAll<ILogManager>());
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterAnd<ViewModelOne>());
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterAnd(() => new DefaultLogManager(AppLocator.Current)));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterAnd<IViewModelOne>(() => new ViewModelOne()));
        Assert.Throws<ArgumentNullException>(() => resolver1!.Register<IViewModelOne, ViewModelOne>());
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterConstantAnd(new ViewModelOne()));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterConstantAnd(new ViewModelOne()));
        Assert.Throws<ArgumentNullException>(() => resolver1!.RegisterConstantAnd<ViewModelOne>());
    }

    /// <summary>
    /// Registers the and tests.
    /// </summary>
    [Test]
    public void RegisterAndTests()
    {
        var resolver = GetDependencyResolver();

        Assert.Throws<ArgumentNullException>(() => resolver.RegisterAnd<IViewModelOne>(null!));

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
