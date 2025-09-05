// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;



using Splat.Common.Test;
using Splat.Tests.ServiceLocation;

namespace Splat.Autofac.Tests;

/// <summary>
/// Tests to show the <see cref="AutofacDependencyResolver"/> works correctly.
/// </summary>
[TestFixture]
public class DependencyResolverTests : BaseDependencyResolverTests<AutofacDependencyResolver>
{
    /// <summary>
    /// Shoulds the resolve nulls.
    /// </summary>
    [Test]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();

        var foo = 5;
        Locator.CurrentMutable.Register(() => foo, null);

        var bar = 4;
        var contract = "foo";
        Locator.CurrentMutable.Register(() => bar, null, contract);
        autofacResolver.SetLifetimeScope(builder.Build());

        Assert.That(Locator.CurrentMutable.HasRegistration(null, Is.True));
        var value = Locator.Current.GetService(null);
        Assert.That(value, Is.EqualTo(foo));

        Assert.That(Locator.CurrentMutable.HasRegistration(null, contract, Is.True));
        value = Locator.Current.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = Locator.Current.GetServices(null);
        Assert.That((int, Is.EqualTo(foo))values.First());
        Assert.That(values.Count(, Is.EqualTo(1)));

        Assert.Throws<NotImplementedException>(() => Locator.CurrentMutable.UnregisterCurrent(null));
        var valuesNC = Locator.Current.GetServices(null);
        Assert.That(valuesNC.Count(, Is.EqualTo(1)));
        Assert.That((int, Is.EqualTo(foo))valuesNC.First());
        var valuesC = Locator.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(, Is.EqualTo(1)));
        Assert.That((int, Is.EqualTo(bar))valuesC.First());
    }

    /// <summary>
    /// Shoulds the resolve views.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Resolve_Views()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
        builder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        Assert.That(viewOne, Is.Not.Null);
        Assert.That(viewOne, Is.TypeOf<ViewOne>());
        Assert.That(viewTwo, Is.Not.Null);
        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Shoulds the resolve views.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Resolve_Named_View()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        Assert.That(viewTwo, Is.Not.Null);
        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Shoulds the resolve view models.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Resolve_View_Models()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewModelOne>().AsSelf();
        builder.RegisterType<ViewModelTwo>().AsSelf();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var vmOne = Locator.Current.GetService<ViewModelOne>();
        var vmTwo = Locator.Current.GetService<ViewModelTwo>();

        Assert.That(vmOne, Is.Not.Null);
        Assert.That(vmTwo, Is.Not.Null);
    }

    /// <summary>
    /// Shoulds the resolve screen.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Resolve_Screen()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<MockScreen>().As<IScreen>().SingleInstance();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var screen = Locator.Current.GetService<IScreen>();

        Assert.That(screen, Is.Not.Null);
        Assert.That(screen, Is.TypeOf<MockScreen>());
    }

    /// <summary>
    /// Should throw an exception if service registration call back called.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
    {
        var builder = new ContainerBuilder();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var result = Record.Exception(() =>
            Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

        Assert.That(result, Is.TypeOf<NotImplementedException>());
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void AutofacDependencyResolver_Should_ReturnRegisteredLogger()
    {
        var builder = new ContainerBuilder();

        var autofacResolver = builder.UseAutofacDependencyResolver();

        Locator.CurrentMutable.RegisterConstant<ILogManager>(new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

        autofacResolver.SetLifetimeScope(builder.Build());

        var logManager = Locator.Current.GetService<ILogManager>();
        Assert.That(logManager, Is.TypeOf<FuncLogManager>());
    }

    /// <summary>
    /// Test that a pre-init logger isn't overriden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void AutofacDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        var builder = new ContainerBuilder();

        var autofacResolver = builder.UseAutofacDependencyResolver();

        builder.Register(_ => new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger()))).As(typeof(ILogManager))
            .AsImplementedInterfaces();

        autofacResolver.SetLifetimeScope(builder.Build());

        var logManager = Locator.Current.GetService<ILogManager>();
        Assert.That(logManager, Is.TypeOf<FuncLogManager>());
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
    /// </summary>
    [Test]
    public override void UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
    /// </summary>
    [Test]
    public override void UnregisterCurrent_Remove_Last()
    {
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
    /// </summary>
    [Test]
    public override void UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    /// </summary>
    [Test]
    public override void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    /// </summary>
    [Test]
    public override void UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    ///     <inheritdoc cref="BaseDependencyResolverTests{T}.HasRegistration"/>
    /// </summary>
    [Test]
    public override void HasRegistration()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var type = typeof(string);
        const string contractOne = "ContractOne";
        const string contractTwo = "ContractTwo";
        var resolver = GetDependencyResolver();

        Assert.That(resolver.HasRegistration(type, Is.False));
        Assert.That(resolver.HasRegistration(type, contractOne, Is.False));
        Assert.That(resolver.HasRegistration(type, contractTwo, Is.False));

        resolver.Register(() => "unnamed", type);
        Assert.That(resolver.HasRegistration(type, Is.True));
        Assert.That(resolver.HasRegistration(type, contractOne, Is.False));
        Assert.That(resolver.HasRegistration(type, contractTwo, Is.False));

        resolver.Register(() => contractOne, type, contractOne);
        Assert.That(resolver.HasRegistration(type, Is.True));
        Assert.That(resolver.HasRegistration(type, contractOne, Is.True));
        Assert.That(resolver.HasRegistration(type, contractTwo, Is.False));

        resolver.Register(() => contractTwo, type, contractTwo);
        Assert.That(resolver.HasRegistration(type, Is.True));
        Assert.That(resolver.HasRegistration(type, contractOne, Is.True));
        Assert.That(resolver.HasRegistration(type, contractTwo, Is.True));
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <inheritdoc />
    protected override AutofacDependencyResolver GetDependencyResolver()
    {
        var builder = new ContainerBuilder();

        return builder.UseAutofacDependencyResolver();
    }
}
