// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;
using Ninject.Planning.Bindings;

namespace Splat.Ninject.Tests;

/// <summary>
/// Tests for the Ninject resolver behavior around activation failures and binding lifecycle: bindings that throw
/// during resolution are skipped, a kernel that fails to enumerate bindings yields an empty result, unregistering
/// unknown or unmatched bindings is a no-op, and the finalizer disposal path leaves the kernel intact.
/// </summary>
[NotInParallel]
public class NinjectDependencyResolverActivationTests
{
    /// <summary>The contract name reused across the keyed tests.</summary>
    private const string Contract = "contract";

    /// <summary>A marker service used by the activation tests.</summary>
    private interface IActivatable;

    /// <summary>Verifies that a binding which throws during activation is skipped by the non-keyed and generic enumerations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WhenBindingFailsToActivate_SkipsBinding()
    {
        var kernel = new StandardKernel();
        _ = kernel.Bind<IActivatable>().ToMethod(static _ => throw new ActivationException("cannot activate"));
        using var resolver = new NinjectDependencyResolver(kernel);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetServices(typeof(IActivatable))).IsEmpty();
            await Assert.That(resolver.GetServices<IActivatable>()).IsEmpty();
        }
    }

    /// <summary>Verifies that a named binding which throws during activation is skipped by the keyed enumerations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithContract_WhenBindingFailsToActivate_SkipsBinding()
    {
        var kernel = new StandardKernel();
        _ = kernel.Bind<IActivatable>().ToMethod(static _ => throw new ActivationException("cannot activate")).Named(Contract);
        using var resolver = new NinjectDependencyResolver(kernel);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetServices(typeof(IActivatable), Contract)).IsEmpty();
            await Assert.That(resolver.GetServices<IActivatable>(Contract)).IsEmpty();
        }
    }

    /// <summary>Verifies that when the kernel fails to enumerate bindings with an activation error, every enumeration returns empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WhenKernelFailsToEnumerateBindings_ReturnsEmpty()
    {
        var kernel = new ThrowingBindingsKernel();
        using var resolver = new NinjectDependencyResolver(kernel);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetServices(typeof(IActivatable))).IsEmpty();
            await Assert.That(resolver.GetServices(typeof(IActivatable), Contract)).IsEmpty();
            await Assert.That(resolver.GetServices<IActivatable>()).IsEmpty();
            await Assert.That(resolver.GetServices<IActivatable>(Contract)).IsEmpty();
        }
    }

    /// <summary>Verifies that unregistering a type with no bindings is a no-op across all four unregister methods.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Unregister_WithNoBindings_DoesNotThrow()
    {
        var kernel = new StandardKernel();
        using var resolver = new NinjectDependencyResolver(kernel);

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(typeof(IActivatable));
            resolver.UnregisterCurrent(typeof(IActivatable), Contract);
            resolver.UnregisterAll(typeof(IActivatable));
            resolver.UnregisterAll(typeof(IActivatable), Contract);
        }).ThrowsNothing();
    }

    /// <summary>Verifies that unregistering all for a contract leaves an existing non-matching binding in place.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_WithContract_WhenNoBindingMatchesContract_LeavesBinding()
    {
        var kernel = new StandardKernel();
        _ = kernel.Bind<IActivatable>().ToConstant(new SimpleActivatable());
        using var resolver = new NinjectDependencyResolver(kernel);

        resolver.UnregisterAll(typeof(IActivatable), Contract);

        // The only binding is unnamed, so a contract-scoped unregister matches nothing and removes nothing.
        await Assert.That(resolver.HasRegistration(typeof(IActivatable))).IsTrue();
    }

    /// <summary>Verifies that the finalizer disposal path leaves the kernel usable, while the deterministic path disposes it.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_FinalizerPath_DoesNotDisposeKernel()
    {
        var kernel = new StandardKernel();
        _ = kernel.Bind<IActivatable>().ToConstant(new SimpleActivatable());
        using var resolver = new DisposeSpyResolver(kernel);

        // Finalizer path: managed cleanup is skipped, so the kernel stays alive and resolvable.
        resolver.InvokeDispose(false);

        await Assert.That(resolver.GetService(typeof(IActivatable))).IsNotNull();
    }

    /// <summary>A trivially constructible implementation of <see cref="IActivatable"/>.</summary>
    private sealed class SimpleActivatable : IActivatable;

    /// <summary>A kernel whose binding enumeration always fails with an <see cref="ActivationException"/>.</summary>
    private sealed class ThrowingBindingsKernel : StandardKernel
    {
        /// <inheritdoc/>
        public override IEnumerable<IBinding> GetBindings(Type service) =>
            throw new ActivationException("cannot enumerate bindings");
    }

    /// <summary>A resolver that exposes the protected <c>Dispose(bool)</c> method so the finalizer path can be exercised.</summary>
    /// <param name="kernel">The kernel the resolver manages.</param>
    private sealed class DisposeSpyResolver(IKernel kernel) : NinjectDependencyResolver(kernel)
    {
        /// <summary>Invokes the protected disposal method.</summary>
        /// <param name="disposing">Whether the deterministic (<see langword="true"/>) or finalizer (<see langword="false"/>) path is taken.</param>
        public void InvokeDispose(bool disposing) => Dispose(disposing);
    }
}
