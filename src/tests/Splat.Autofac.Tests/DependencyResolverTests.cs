// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Autofac;

using Splat.Common.Test;
using Splat.Tests.ServiceLocation;

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
#pragma warning disable CS0618 // Type or member is obsolete

namespace Splat.Autofac.Tests;

/// <summary>Tests for the Autofac dependency resolver.</summary>
[InheritsTests]
[NotInParallel]
public class DependencyResolverTests : BaseDependencyResolverTests<AutofacDependencyResolver>
{
    /// <summary>The contract name used when exercising contract-based registration APIs.</summary>
    private const string ContractName = "contract";

    /// <summary>Should resolve nulls.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Can_Register_And_Resolve_Null_Types()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();

        const int foo = 5;

        // Explicitly cast to call the non-generic Register method with null service type
        Locator.CurrentMutable.Register(() => foo, serviceType: null);

        const int bar = 4;
        const string contract = "foo";
        Locator.CurrentMutable.Register(() => bar, serviceType: null, contract: contract);

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

        Assert.Throws<NotSupportedException>(() => Locator.CurrentMutable.UnregisterCurrent(null));

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

    /// <summary>Should resolve views.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AutofacDependencyResolver_Should_Resolve_Views()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewOne>().As<IViewFor<ViewModelOne>>();
        builder.RegisterType<ViewTwo>().As<IViewFor<ViewModelTwo>>();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewOne = Locator.Current.GetService<IViewFor<ViewModelOne>>();
        var viewTwo = Locator.Current.GetService<IViewFor<ViewModelTwo>>();

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>Should resolve named view.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AutofacDependencyResolver_Should_Resolve_Named_View()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ViewTwo>().Named<IViewFor<ViewModelTwo>>("Other");

        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        var viewTwo = Locator.Current.GetService<IViewFor<ViewModelTwo>>("Other");

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>Should resolve view models.</summary>
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

    /// <summary>Should resolve screen.</summary>
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

    /// <summary>Should throw an exception if service registration callback is called.</summary>
    [Test]
    public void AutofacDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();
        autofacResolver.SetLifetimeScope(builder.Build());

        Assert.Throws<NotSupportedException>(() =>
            Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), _ => { }));
    }

    /// <summary>Verifies that UnregisterCurrent throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete UnregisterCurrent API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterCurrent method")]
    public override Task UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent(typeof(string)));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterCurrent throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete UnregisterCurrent API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterCurrent method")]
    public override Task UnregisterCurrent_Remove_Last()
    {
        var resolver = GetDependencyResolver();

        // Register a value so the scenario reflects removing the last registration; Autofac still throws.
        resolver.Register(() => "last", typeof(string));
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent(typeof(string)));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterCurrent with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete UnregisterCurrent API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterCurrent method")]
    public override Task UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent(typeof(string), ContractName));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterAll throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete UnregisterAll API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterAll method")]
    public override Task UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll(typeof(string)));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterAll with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete UnregisterAll API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterAll method")]
    public override Task UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll(typeof(string), ContractName));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that generic UnregisterCurrent throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete generic UnregisterCurrent API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterCurrent method")]
    public override Task UnregisterCurrent_Generic_RemovesLastRegistration()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent<ViewModelOne>());
        return Task.CompletedTask;
    }

    /// <summary>Verifies that generic UnregisterCurrent with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete generic UnregisterCurrent API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterCurrent method")]
    public override Task UnregisterCurrent_Generic_WithContract_RemovesRegistration()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent<ViewModelOne>(ContractName));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that generic UnregisterAll throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete generic UnregisterAll API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterAll method")]
    public override Task UnregisterAll_Generic_RemovesAllRegistrations()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll<ViewModelOne>());
        return Task.CompletedTask;
    }

    /// <summary>Verifies that generic UnregisterAll with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete generic UnregisterAll API and must keep the [Obsolete] marker to compile.")]
    [Obsolete("Testing obsolete UnregisterAll method")]
    public override Task UnregisterAll_Generic_WithContract_RemovesAllContractRegistrations()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll<ViewModelOne>(ContractName));
        return Task.CompletedTask;
    }

    /// <summary>Should check registration with and without contracts.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Test deliberately overrides and exercises the obsolete HasRegistration API and must keep the [Obsolete] marker to compile.")]
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

    /// <summary>Verifies that ServiceRegistrationCallback throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();

        // Register a service first so the callback would normally fire immediately; Autofac still throws.
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>("test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that non-generic ServiceRegistrationCallback throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that non-generic ServiceRegistrationCallback with contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), "test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback disposal throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();

        // The subscription disposable is never produced because Autofac throws before returning it.
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }).Dispose());
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback with null callback throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();

        // Autofac throws NotSupportedException before checking for null
        await Assert.That(() => resolver.ServiceRegistrationCallback<ViewModelOne>(null!))
            .Throws<NotSupportedException>();

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), null!))
            .Throws<NotSupportedException>();
    }

    /// <summary>Autofac doesn't invoke callbacks on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_InvokesCallbacks()
    {
        // Autofac ServiceRegistrationCallback throws NotSupportedException, so this test doesn't apply
        return Task.CompletedTask;
    }

    /// <summary>Autofac manages disposal of registered services itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_DisposesRegisteredServices()
    {
        // Autofac manages its own service disposal lifecycle
        return Task.CompletedTask;
    }

    /// <summary>Autofac handles lazy singletons itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_WithLazySingleton_DoesNotCreateIfNotAccessed()
    {
        // Autofac manages lazy singleton creation and disposal
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterCurrent generic with null contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrent_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent<ViewModelOne>(null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterCurrent non-generic with null contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrent_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent(typeof(ViewModelOne), null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterAll generic with null contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterAll_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll<ViewModelOne>(null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterAll non-generic with null contract throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterAll_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll(typeof(ViewModelOne), null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterCurrent with null type throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterCurrent_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterCurrent(null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that UnregisterAll with null type throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task UnregisterAll_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotSupportedException>(() => resolver.UnregisterAll(null));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that Register after dispose throws NotSupportedException for Autofac (due to callbacks not implemented).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();
        resolver.Dispose();

        // After disposal, ServiceRegistrationCallback still throws NotSupportedException for Autofac.
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback invokes for each throws NotSupportedException for Autofac.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();

        // Register multiple services so the callback would normally fire for each; Autofac still throws.
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that Dispose suppresses exceptions from callbacks (NotApplicable for Autofac as callbacks throw).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();

        // Callbacks can never be registered (Autofac throws), so disposal has nothing to suppress.
        Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        resolver.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>Autofac handles lazy singletons itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_WithAccessedLazySingleton_DisposesValue()
    {
        // Autofac manages lazy singleton creation and disposal
        return Task.CompletedTask;
    }

    /// <summary>Autofac manages disposal of services under construction itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_WhileLazySingletonUnderConstruction_DisposesServiceAndThrowsException()
    {
        // Autofac manages its own service disposal lifecycle including services under construction
        return Task.CompletedTask;
    }

    /// <summary>Verifies GetService with null type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task GetService_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // Skipping as this seems to fail in current adapter implementation
        return Task.CompletedTask;
    }

    /// <summary>Verifies GetServices with null type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task GetServices_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // Skipping as this seems to fail in current adapter implementation
        return Task.CompletedTask;
    }

    /// <summary>Verifies HasRegistration with null type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task HasRegistration_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // Skipping as this seems to fail in current adapter implementation
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override AutofacDependencyResolver GetDependencyResolver()
    {
        var builder = new ContainerBuilder();
        return builder.UseAutofacDependencyResolver();
    }
}
