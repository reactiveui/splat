// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;


using Splat.Common.Test;

namespace Splat.DryIoc.Tests;

/// <summary>
/// Tests to show the <see cref="DryIocDependencyResolver"/> works correctly.
/// </summary>
[TestFixture]
public class DependencyResolverTests
{
    /// <summary>
    /// Shoulds the resolve nulls.
    /// </summary>
    [Test] //// (Skip = "Further investigation required")]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var builder = new Container();
        builder.UseDryIocDependencyResolver();

        var foo = 5;
        Assert.Throws<ArgumentNullException>(() => AppLocator.CurrentMutable.Register(() => foo, null));

        // Tests skipped as functionality removed.
#if SKIP_TEST
        var bar = 4;
        var contract = "foo";
        AppLocator.CurrentMutable.Register(() => bar, null, contract);

        Assert.That(AppLocator.CurrentMutable.HasRegistration(null, Is.True));
        var value = AppLocator.Current.GetService(null);
        Assert.That(value, Is.EqualTo(foo));

        Assert.That(Locator.CurrentMutable.HasRegistration(null, contract, Is.True));
        value = AppLocator.Current.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = AppLocator.Current.GetServices(null);
        Assert.That((int, Is.EqualTo(foo))values.First());
        Assert.That(values.Count(, Is.EqualTo(1)));

        AppLocator.CurrentMutable.UnregisterCurrent(null);
        var valuesNC = AppLocator.Current.GetServices(null);
        Assert.That(valuesNC.Count(, Is.EqualTo(0)));
        var valuesC = AppLocator.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(, Is.EqualTo(1)));

        AppLocator.CurrentMutable.UnregisterAll(null);
        valuesNC = AppLocator.Current.GetServices(null);
        Assert.That(valuesNC.Count(, Is.EqualTo(0)));

        AppLocator.CurrentMutable.UnregisterAll(null, contract);
        valuesC = AppLocator.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(, Is.EqualTo(0)));
#endif
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Register_But_Not_Create_Views()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();

        AppLocator.CurrentMutable.Register(() => new ViewThatShouldNotLoad(), typeof(IViewFor<ViewModelOne>));
        Assert.Throws<InvalidOperationException>(() => AppLocator.Current.GetService<IViewFor<ViewModelOne>>());
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Register_With_Contract_But_Not_Create_Views()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();

        AppLocator.CurrentMutable.Register(() => new ViewThatShouldNotLoad(), typeof(IViewFor<ViewModelOne>), "name");
        Assert.Throws<InvalidOperationException>(() => AppLocator.Current.GetService<IViewFor<ViewModelOne>>("name"));
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Resolve_Views()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelOne>, ViewOne>();
        container.Register<IViewFor<ViewModelTwo>, ViewTwo>();
        container.UseDryIocDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        Assert.That(viewOne, Is.Not.Null);
        Assert.That(viewOne, Is.TypeOf<ViewOne>());
        Assert.That(viewTwo, Is.Not.Null);
        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Resolve_Named_View()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelTwo>, ViewTwo>(serviceKey: "Other");
        container.UseDryIocDependencyResolver();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        Assert.That(viewTwo, Is.Not.Null);
        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Should resolve the view models.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Resolve_View_Models()
    {
        var container = new Container();
        container.Register<ViewModelOne>();
        container.Register<ViewModelTwo>();
        container.UseDryIocDependencyResolver();

        AppLocator.CurrentMutable.Register(() => new ViewThatShouldNotLoad(), typeof(IViewFor<ViewModelOne>), "name");

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        Assert.That(vmOne, Is.Not.Null);
        Assert.That(vmTwo, Is.Not.Null);
    }

    /// <summary>
    /// Should resolve the screen.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Resolve_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        Assert.That(screen, Is.Not.Null);
        Assert.That(screen, Is.TypeOf<MockScreen>());
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_UnregisterCurrent_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
        builder.UseDryIocDependencyResolver();

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_UnregisterAll_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
        builder.UseDryIocDependencyResolver();

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Null);
    }

    /// <summary>
    /// Should throw an exception if service registration call back called.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();

        var result = Record.Exception(() =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

        Assert.That(result, Is.TypeOf<NotImplementedException>());
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void DryIocDependencyResolver_Should_ReturnRegisteredLogger()
    {
        var c = new Container();
        c.UseDryIocDependencyResolver();
        c.Register<ILogger, ConsoleLogger>(ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        AppLocator.CurrentMutable.RegisterConstant<ILogManager>(new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

        var d = AppLocator.Current.GetService<ILogManager>();
        Assert.That(d, Is.TypeOf<FuncLogManager>());
    }

    /// <summary>
    /// Test that a pre-init logger isn't overriden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void DryIocDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        var c = new Container();
        c.RegisterInstance<ILogManager>(new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));
        c.UseDryIocDependencyResolver();

        var d = AppLocator.Current.GetService<ILogManager>();
        Assert.That(d, Is.TypeOf<FuncLogManager>());
    }

    /// <summary>
    /// DryIoc dependency resolver should resolve after duplicate keyed registratoion.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Resolve_AfterDuplicateKeyedRegistration()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();
        AppLocator.CurrentMutable.Register(() => new ViewModelOne(), typeof(ViewModelOne), "ViewModelOne");
        AppLocator.CurrentMutable.Register(() => new ViewModelOne(), typeof(ViewModelOne), "ViewModelOne");

        var vmOne = AppLocator.Current.GetService<ViewModelOne>("ViewModelOne");

        Assert.That(vmOne, Is.Not.Null);
    }

    /// <summary>
    /// DryIoc dependency resolver should create a resolved object only once when resolving.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Create_Once_When_Resolving()
    {
        var container = new Container();
        var count = 0;
        container.RegisterDelegate(() =>
        {
            count++;
            return new ViewModelOne();
        });

        var resolver = new DryIocDependencyResolver(container);

        // Imitate a call to Locator.Current.GetService<ViewModelOne>()
        var vms = resolver.GetServices(typeof(ViewModelOne));
        Assert.That(count, Is.EqualTo(1));
        var vmOne = vms.LastOrDefault();
        Assert.That(vmOne, Is.Not.Null);
        Assert.That(count, Is.EqualTo(1));
    }
}
