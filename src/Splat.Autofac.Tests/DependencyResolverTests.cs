// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    /// Should resolve nulls.
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

        Assert.That(Locator.CurrentMutable.HasRegistration(null), Is.True);

        var value = Locator.Current.GetService(null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.EqualTo(foo));

            Assert.That(Locator.CurrentMutable.HasRegistration(null, contract), Is.True);
        }

        value = Locator.Current.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = Locator.Current.GetServices(null).ToList();
        using (Assert.EnterMultipleScope())
        {
            Assert.That((int)values[0], Is.EqualTo(foo));
            Assert.That(values, Has.Count.EqualTo(1));
        }

        Assert.Throws<NotImplementedException>(() => Locator.CurrentMutable.UnregisterCurrent(null));

        var valuesNc = Locator.Current.GetServices(null).ToList();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuesNc, Has.Count.EqualTo(1));
            Assert.That((int)valuesNc[0], Is.EqualTo(foo));
        }

        var valuesC = Locator.Current.GetServices(null, contract).ToList();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuesC, Has.Count.EqualTo(1));
            Assert.That((int)valuesC[0], Is.EqualTo(bar));
        }
    }

    /// <summary>
    /// Should resolve views.
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
        using (Assert.EnterMultipleScope())
        {
            Assert.That(viewOne, Is.TypeOf<ViewOne>());
            Assert.That(viewTwo, Is.Not.Null);
        }

        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Should resolve named view.
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
    /// Should resolve view models.
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vmOne, Is.Not.Null);
            Assert.That(vmTwo, Is.Not.Null);
        }
    }

    /// <summary>
    /// Should resolve screen.
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
    /// Should throw an exception if service registration callback is called.
    /// </summary>
    [Test]
    public void AutofacDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        Assert.Throws<NotImplementedException>(() =>
            Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), _ => { }));
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
    [Obsolete("Obsolete")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void HasRegistration()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
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

        resolver.Register(() => contractOne, type, contractOne);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.True);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.True);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.False);
        }

        resolver.Register(() => contractTwo, type, contractTwo);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(type), Is.True);
            Assert.That(resolver.HasRegistration(type, contractOne), Is.True);
            Assert.That(resolver.HasRegistration(type, contractTwo), Is.True);
        }
    }

    /// <inheritdoc />
    protected override AutofacDependencyResolver GetDependencyResolver()
    {
        var builder = new ContainerBuilder();
        return builder.UseAutofacDependencyResolver();
    }
}
