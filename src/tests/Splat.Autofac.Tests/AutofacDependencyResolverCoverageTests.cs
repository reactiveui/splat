// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

using Splat.Common.Test;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Splat.Autofac.Tests;

/// <summary>Additional coverage tests for <see cref="AutofacDependencyResolver"/> edge cases.</summary>
public sealed class AutofacDependencyResolverCoverageTests
{
    /// <summary>A contract name used when exercising the contract-based registration APIs.</summary>
    private const string ContractName = "contract";

    /// <summary>Verifies that resolving the null service type without a registration yields <see langword="null"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithNullType_AndNoRegistration_ReturnsNull()
    {
        var resolver = BuiltResolver();

        await Assert.That(resolver.GetService(null)).IsNull();
    }

    /// <summary>Verifies that resolving the null service type by contract without a registration yields <see langword="null"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithNullTypeAndContract_AndNoRegistration_ReturnsNull()
    {
        var resolver = BuiltResolver();

        await Assert.That(resolver.GetService(null, ContractName)).IsNull();
    }

    /// <summary>Verifies that setting the lifetime scope twice throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLifetimeScope_WhenAlreadySet_Throws()
    {
        var builder = new ContainerBuilder();
        var resolver = new AutofacDependencyResolver(builder);
        var scope = builder.Build();
        resolver.SetLifetimeScope(scope);

        await Assert.That(() => resolver.SetLifetimeScope(scope)).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the factory register throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_Factory_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.Register(static () => new object(), typeof(object))).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the contract factory register throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_FactoryWithContract_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.Register(static () => new object(), typeof(object), ContractName)).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the generic type register throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_GenericType_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.Register<ViewModelOne, ViewModelOne>()).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the generic type register with contract throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_GenericTypeWithContract_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.Register<ViewModelOne, ViewModelOne>(ContractName)).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the constant register throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.RegisterConstant(new ViewModelOne())).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the constant register with contract throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantWithContract_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.RegisterConstant(new ViewModelOne(), ContractName)).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the lazy singleton register throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.RegisterLazySingleton(static () => new ViewModelOne())).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the lazy singleton register with contract throws once the lifetime scope has been set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonWithContract_AfterLifetimeScopeSet_Throws()
    {
        var resolver = BuiltResolver();

        await Assert.That(() => resolver.RegisterLazySingleton(static () => new ViewModelOne(), ContractName)).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that a generic type registered before the lifetime scope is set resolves from the built container.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_GenericType_BeforeLifetimeScopeSet_ResolvesFromBuiltContainer()
    {
        var builder = new ContainerBuilder();
        using var resolver = new AutofacDependencyResolver(builder);
        resolver.Register<ViewModelOne, ViewModelOne>();
        resolver.SetLifetimeScope(builder.Build());

        await Assert.That(resolver.GetService<ViewModelOne>()).IsNotNull();
    }

    /// <summary>Verifies that disposing with a user-supplied lifetime scope set completes without throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WithLifetimeScopeSet_DoesNotThrow()
    {
        var resolver = BuiltResolver();

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>Verifies that the finalizer-path dispose (disposing is <see langword="false"/>) leaves the container usable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenNotDisposing_LeavesContainerUsable()
    {
        var builder = new ContainerBuilder();
        using var resolver = new DisposeProbeResolver(builder);
        resolver.RegisterConstant(new ViewModelOne());

        resolver.InvokeDispose(false);

        await Assert.That(resolver.GetService<ViewModelOne>()).IsNotNull();
    }

    /// <summary>Creates a resolver whose lifetime scope has already been set from a freshly built container.</summary>
    /// <returns>A built <see cref="AutofacDependencyResolver"/>.</returns>
    private static AutofacDependencyResolver BuiltResolver()
    {
        var builder = new ContainerBuilder();
        var resolver = new AutofacDependencyResolver(builder);
        resolver.SetLifetimeScope(builder.Build());
        return resolver;
    }

    /// <summary>A resolver subclass exposing the protected dispose overload so the finalizer path can be exercised.</summary>
    /// <param name="builder">The Autofac container builder.</param>
    private sealed class DisposeProbeResolver(ContainerBuilder builder) : AutofacDependencyResolver(builder)
    {
        /// <summary>Invokes the protected dispose overload.</summary>
        /// <param name="disposing">Whether the instance is disposing.</param>
        public void InvokeDispose(bool disposing) => Dispose(disposing);
    }
}
