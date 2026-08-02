// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests that a resolver installed with <see cref="DependencyResolverMixins.WithResolver(IDependencyResolver, bool)"/>
/// is scoped to the async flow that installed it, so overlapping flows do not observe each other's resolver or
/// notification suppression.
/// </summary>
[NotInParallel]
public sealed class WithResolverFlowIsolationTests
{
    /// <summary>The locator scope created for the duration of each test.</summary>
    private AppLocatorScope? _scope;

    /// <summary>Identifies which flow registered the resolved instance.</summary>
    private interface IFlowMarker
    {
        /// <summary>Gets the name of the flow that registered this marker.</summary>
        string Flow { get; }
    }

    /// <summary>Creates a fresh locator scope before each test.</summary>
    [Before(Test)]
    public void SetUp() => _scope = new();

    /// <summary>Disposes the locator scope after each test.</summary>
    [After(Test)]
    public void TearDown()
    {
        _scope?.Dispose();
        _scope = null;
    }

    /// <summary>Verifies that two flows whose <c>WithResolver</c> scopes overlap in time each resolve from their own resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_WhenScopesOverlap_EachFlowResolvesFromItsOwnResolver()
    {
        using var firstResolver = new InstanceGenericFirstDependencyResolver();
        firstResolver.RegisterConstant<IFlowMarker>(new FlowMarker("first"));

        using var secondResolver = new InstanceGenericFirstDependencyResolver();
        secondResolver.RegisterConstant<IFlowMarker>(new FlowMarker("second"));

        using var firstScopeEntered = new SemaphoreSlim(0, 1);
        using var secondScopeEntered = new SemaphoreSlim(0, 1);
        using var firstFlowResolved = new SemaphoreSlim(0, 1);

        async Task<string?> ResolveInFirstFlowAsync()
        {
            using (firstResolver.WithResolver())
            {
                _ = firstScopeEntered.Release();
                await secondScopeEntered.WaitAsync();

                var flow = AppLocator.Current.GetService<IFlowMarker>()?.Flow;
                _ = firstFlowResolved.Release();
                return flow;
            }
        }

        async Task<string?> ResolveInSecondFlowAsync()
        {
            await firstScopeEntered.WaitAsync();

            using (secondResolver.WithResolver())
            {
                _ = secondScopeEntered.Release();
                await firstFlowResolved.WaitAsync();

                return AppLocator.Current.GetService<IFlowMarker>()?.Flow;
            }
        }

        var resolved = await Task.WhenAll(ResolveInFirstFlowAsync(), ResolveInSecondFlowAsync());

        using (Assert.Multiple())
        {
            await Assert.That(resolved[0]).IsEqualTo("first");
            await Assert.That(resolved[1]).IsEqualTo("second");
        }
    }

    /// <summary>
    /// Verifies that a flow suppressing resolver-changed notifications does not suppress them for an overlapping flow
    /// that asked for notifications to keep flowing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_WhenScopesOverlap_SuppressionDoesNotLeakIntoAnotherFlow()
    {
        using var suppressingResolver = new InstanceGenericFirstDependencyResolver();
        using var notifyingResolver = new InstanceGenericFirstDependencyResolver();

        using var suppressingScopeEntered = new SemaphoreSlim(0, 1);
        using var notifyingFlowObserved = new SemaphoreSlim(0, 1);

        async Task<bool> ObserveWhileSuppressingAsync()
        {
            using (suppressingResolver.WithResolver(suppressResolverCallback: true))
            {
                _ = suppressingScopeEntered.Release();
                await notifyingFlowObserved.WaitAsync();

                return AppLocator.AreResolverCallbackChangedNotificationsEnabled();
            }
        }

        async Task<bool> ObserveWhileNotifyingAsync()
        {
            await suppressingScopeEntered.WaitAsync();

            using (notifyingResolver.WithResolver(suppressResolverCallback: false))
            {
                var enabled = AppLocator.AreResolverCallbackChangedNotificationsEnabled();
                _ = notifyingFlowObserved.Release();
                return enabled;
            }
        }

        var observed = await Task.WhenAll(ObserveWhileSuppressingAsync(), ObserveWhileNotifyingAsync());

        using (Assert.Multiple())
        {
            await Assert.That(observed[0]).IsFalse();
            await Assert.That(observed[1]).IsTrue();
        }
    }

    /// <summary>
    /// Verifies that a resolver installed inside a flow is not observable from an unrelated flow, which continues to
    /// see the process-wide resolver.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_WhileScopeIsActive_UnrelatedFlowSeesTheProcessWideResolver()
    {
        var processWideResolver = AppLocator.GetLocator();

        using var flowResolver = new InstanceGenericFirstDependencyResolver();

        using var scopeEntered = new SemaphoreSlim(0, 1);
        using var unrelatedFlowObserved = new SemaphoreSlim(0, 1);

        async Task HoldScopeAsync()
        {
            using (flowResolver.WithResolver())
            {
                _ = scopeEntered.Release();
                await unrelatedFlowObserved.WaitAsync();
            }
        }

        async Task<IDependencyResolver> ObserveFromUnrelatedFlowAsync()
        {
            await scopeEntered.WaitAsync();

            var observed = AppLocator.GetLocator();
            _ = unrelatedFlowObserved.Release();
            return observed;
        }

        var holding = HoldScopeAsync();
        var observedResolver = await ObserveFromUnrelatedFlowAsync();
        await holding;

        using (Assert.Multiple())
        {
            await Assert.That(observedResolver).IsSameReferenceAs(processWideResolver);
            await Assert.That(AppLocator.GetLocator()).IsSameReferenceAs(processWideResolver);
        }
    }

    /// <summary>Verifies that replacing the locator inside a scope only affects that flow and is discarded with the scope.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_InsideAScope_IsConfinedToTheFlow()
    {
        var processWideResolver = AppLocator.GetLocator();

        using var flowResolver = new InstanceGenericFirstDependencyResolver();
        using var replacementResolver = new InstanceGenericFirstDependencyResolver();

        using (flowResolver.WithResolver())
        {
            AppLocator.SetLocator(replacementResolver);

            await Assert.That(AppLocator.GetLocator()).IsSameReferenceAs(replacementResolver);
        }

        await Assert.That(AppLocator.GetLocator()).IsSameReferenceAs(processWideResolver);
    }

    /// <summary>A marker naming the flow that registered it.</summary>
    /// <param name="flow">The name of the flow that registered this marker.</param>
    private sealed class FlowMarker(string flow) : IFlowMarker
    {
        /// <inheritdoc/>
        public string Flow { get; } = flow;
    }
}
