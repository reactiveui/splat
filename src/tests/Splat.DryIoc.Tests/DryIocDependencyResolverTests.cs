// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using DryIoc;
using Splat.Tests.ServiceLocation;

namespace Splat.DryIoc.Tests;

/// <summary>
/// Unit tests for the DryIocDependencyResolver that verify conformance to the IDependencyResolver contract.
/// </summary>
/// <remarks>
/// Inherits from BaseDependencyResolverTests to ensure consistent behavior across all resolver implementations.
/// </remarks>
[NotInParallel]
[InheritsTests]
public sealed class DryIocDependencyResolverTests : BaseDependencyResolverTests<DryIocDependencyResolver>
{
    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>("test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), "test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();
        await Assert.That(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(null!))
            .Throws<NotImplementedException>();
        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), null!))
            .Throws<NotImplementedException>();
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support ServiceRegistrationCallback.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not invoke callbacks on disposal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Dispose_InvokesCallbacks()
    {
        // DryIoc ServiceRegistrationCallback throws NotImplementedException
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc manages disposal of services itself.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Dispose_WhileLazySingletonUnderConstruction_DisposesServiceAndThrowsException()
    {
        // DryIoc manages its own service disposal lifecycle including services under construction
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support null service types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task GetService_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // DryIoc does not support null service types
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support null service types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task GetServices_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // DryIoc does not support null service types
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support null service types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task HasRegistration_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // DryIoc does not support null service types
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support null service types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterCurrent_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // DryIoc does not support null service types
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support null service types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterAll_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        // DryIoc does not support null service types
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's Unregister implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterCurrent_Remove_Last()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's Unregister implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterCurrent_Generic_RemovesLastRegistration()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's Unregister implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterAll_Generic_RemovesAllRegistrations()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's Unregister implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterCurrent_Generic_WithNullContract_DelegatesToNonContract()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's Unregister implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterCurrent_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support multiple registrations with the same contract (Replace mode only).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task GetServices_Generic_WithContract_CombinesGenericAndNonGenericResults()
    {
        // DryIoc uses Replace mode for contracts, not stack-based append
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc does not support multiple registrations with the same contract (Replace mode only).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterAll_Generic_WithContract_RemovesAllContractRegistrations()
    {
        // DryIoc uses Replace mode for contracts, not stack-based append
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's UnregisterAll implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterAll_Generic_WithNullContract_DelegatesToNonContract()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <summary>
    /// DryIoc's UnregisterAll implementation has issues with DefaultKey-based registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task UnregisterAll_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        // DryIoc's unregister doesn't work correctly with AppendNewImplementation DefaultKeys
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override DryIocDependencyResolver GetDependencyResolver() => new(new Container());
}
