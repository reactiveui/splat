// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

using Splat.Common.Test;

namespace Splat.DryIoc.Tests;

[NotInParallel]
public class DependencyResolverTests
{
    /// <summary>
    /// Shoulds the resolve nulls.
    /// </summary>
    [Test] //// (Ignore("Further investigation required"))]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var builder = new Container();
        builder.UseDryIocDependencyResolver();

        const int foo = 5;
        Assert.Throws<ArgumentNullException>(() => AppLocator.CurrentMutable.Register(() => foo, serviceType: null, contract: null));

        // Tests skipped as functionality removed.
#if SKIP_TEST
        var bar = 4;
        var contract = "foo";
        AppLocator.CurrentMutable.Register(() => bar, null, contract);

        Assert.That(AppLocator.CurrentMutable.HasRegistration(null), Is.True);
        var value = AppLocator.Current.GetService(null);
        Assert.That(value, Is.EqualTo(foo));

        Assert.That(Locator.CurrentMutable.HasRegistration(null, contract), Is.True);
        value = AppLocator.Current.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = AppLocator.Current.GetServices(null);
        Assert.That((int)values.First(), Is.EqualTo(foo));
        Assert.That(values.Count(), Is.EqualTo(1));

        AppLocator.CurrentMutable.UnregisterCurrent(null);
        var valuesNC = AppLocator.Current.GetServices(null);
        Assert.That(valuesNC.Count(), Is.EqualTo(0));
        var valuesC = AppLocator.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.EqualTo(1));

        AppLocator.CurrentMutable.UnregisterAll(null);
        valuesNC = AppLocator.Current.GetServices(null);
        Assert.That(valuesNC.Count(), Is.EqualTo(0));

        AppLocator.CurrentMutable.UnregisterAll(null, contract);
        valuesC = AppLocator.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.EqualTo(0));
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Resolve_Views()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelOne>, ViewOne>();
        container.Register<IViewFor<ViewModelTwo>, ViewTwo>();
        container.UseDryIocDependencyResolver();

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
    /// Should resolve the views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Resolve_Named_View()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelTwo>, ViewTwo>(serviceKey: "Other");
        container.UseDryIocDependencyResolver();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve the view models.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Resolve_View_Models()
    {
        var container = new Container();
        container.Register<ViewModelOne>();
        container.Register<ViewModelTwo>();
        container.UseDryIocDependencyResolver();

        AppLocator.CurrentMutable.Register(() => new ViewThatShouldNotLoad(), typeof(IViewFor<ViewModelOne>), "name");

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.Multiple())
        {
            await Assert.That(vmOne).IsNotNull();
            await Assert.That(vmTwo).IsNotNull();
        }
    }

    /// <summary>
    /// Should resolve the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Resolve_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_UnregisterCurrent_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
        builder.UseDryIocDependencyResolver();

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNotNull();

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_UnregisterAll_Screen()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton);
        builder.UseDryIocDependencyResolver();

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
    {
        var builder = new Container();
        builder.Register<IScreen, MockScreen>(Reuse.Singleton, serviceKey: nameof(MockScreen));
        builder.UseDryIocDependencyResolver();

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNotNull();

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNull();
    }

    /// <summary>
    /// Should throw an exception if service registration call back called.
    /// </summary>
    [Test]
    public void DryIocDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();

        Assert.Throws<NotImplementedException>(() =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), _ => { }));
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_ReturnRegisteredLogger()
    {
        var c = new Container();
        c.UseDryIocDependencyResolver();
        c.Register<ILogger, ConsoleLogger>(ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        AppLocator.CurrentMutable.RegisterConstant<ILogManager>(
            new FuncLogManager(_ => new WrappingFullLogger(new ConsoleLogger())));

        var d = AppLocator.Current.GetService<ILogManager>();

        await Assert.That(d).IsTypeOf<FuncLogManager>();
    }

    /// <summary>
    /// Test that a pre-init logger isn't overriden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        var c = new Container();
        c.RegisterInstance<ILogManager>(
            new FuncLogManager(_ => new WrappingFullLogger(new ConsoleLogger())));
        c.UseDryIocDependencyResolver();

        var d = AppLocator.Current.GetService<ILogManager>();

        await Assert.That(d).IsTypeOf<FuncLogManager>();
    }

    /// <summary>
    /// DryIoc dependency resolver should resolve after duplicate keyed registratoion.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Resolve_AfterDuplicateKeyedRegistration()
    {
        var container = new Container();
        container.UseDryIocDependencyResolver();
        AppLocator.CurrentMutable.Register(() => new ViewModelOne(), typeof(ViewModelOne), "ViewModelOne");
        AppLocator.CurrentMutable.Register(() => new ViewModelOne(), typeof(ViewModelOne), "ViewModelOne");

        var vmOne = AppLocator.Current.GetService<ViewModelOne>("ViewModelOne");

        await Assert.That(vmOne).IsNotNull();
    }

    /// <summary>
    /// DryIoc dependency resolver should create a resolved object only once when resolving.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DryIocDependencyResolver_Should_Create_Once_When_Resolving()
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
        await Assert.That(count).IsEqualTo(1);

        var vmOne = vms.LastOrDefault();
        using (Assert.Multiple())
        {
            await Assert.That(vmOne).IsNotNull();
            await Assert.That(count).IsEqualTo(1);
        }
    }
}
