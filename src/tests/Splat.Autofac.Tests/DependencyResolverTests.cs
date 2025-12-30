// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

using Splat.Common.Test;
using Splat.Tests.ServiceLocation;

namespace Splat.Autofac.Tests;

[InheritsTests]
[NotInParallel]
public class DependencyResolverTests : BaseDependencyResolverTests<AutofacDependencyResolver>
{
    /// <summary>
    /// Should resolve nulls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Can_Register_And_Resolve_Null_Types()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();

        const int foo = 5;
        Locator.CurrentMutable.Register(() => foo, null);

        const int bar = 4;
        const string contract = "foo";
        Locator.CurrentMutable.Register(() => bar, null, contract);

        autofacResolver.SetLifetimeScope(builder.Build());

        await Assert.That(Locator.CurrentMutable.HasRegistration(null)).IsTrue();

        var value = Locator.Current.GetService(null);
        using (Assert.Multiple())
        {
            await Assert.That(value).IsEqualTo(foo);

            await Assert.That(Locator.CurrentMutable.HasRegistration(null, contract)).IsTrue();
        }

        value = Locator.Current.GetService(null, contract);
        await Assert.That(value).IsEqualTo(bar);

        var values = Locator.Current.GetServices(null).ToList();
        using (Assert.Multiple())
        {
            await Assert.That((int)values[0]).IsEqualTo(foo);
            await Assert.That(values).Count().IsEqualTo(1);
        }

        Assert.Throws<NotImplementedException>(() => Locator.CurrentMutable.UnregisterCurrent(null));

        var valuesNc = Locator.Current.GetServices(null).ToList();
        using (Assert.Multiple())
        {
            await Assert.That(valuesNc).Count().IsEqualTo(1);
            await Assert.That((int)valuesNc[0]).IsEqualTo(foo);
        }

        var valuesC = Locator.Current.GetServices(null, contract).ToList();
        using (Assert.Multiple())
        {
            await Assert.That(valuesC).Count().IsEqualTo(1);
            await Assert.That((int)valuesC[0]).IsEqualTo(bar);
        }
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AutofacDependencyResolver_Should_Resolve_Views()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
        builder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve named view.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AutofacDependencyResolver_Should_Resolve_Named_View()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve view models.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AutofacDependencyResolver_Should_Resolve_View_Models()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewModelOne>().AsSelf();
        builder.RegisterType<ViewModelTwo>().AsSelf();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var vmOne = Locator.Current.GetService<ViewModelOne>();
        var vmTwo = Locator.Current.GetService<ViewModelTwo>();

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
    public async Task AutofacDependencyResolver_Should_Resolve_Screen()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<MockScreen>().As<IScreen>().SingleInstance();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var screen = Locator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrent_Remove_Last()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterCurrent"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     <inheritdoc cref="AutofacDependencyResolver.UnregisterAll"/>
    ///     <inheritdoc cref="BaseDependencyResolverTests{T}.HasRegistration"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Obsolete("Obsolete")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override async Task HasRegistration()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
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

        resolver.Register(() => contractOne, type, contractOne);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.Register(() => contractTwo, type, contractTwo);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsTrue();
        }
    }

    /// <inheritdoc />
    protected override AutofacDependencyResolver GetDependencyResolver()
    {
        var builder = new ContainerBuilder();
        return builder.UseAutofacDependencyResolver();
    }
}
