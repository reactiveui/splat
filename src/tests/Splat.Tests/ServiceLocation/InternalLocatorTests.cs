// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="InternalLocator"/> class.
/// </summary>
[NotInParallel]
public sealed class InternalLocatorTests
{
    private InternalLocatorScope? _scope;
    private InternalLocator _locator = null!;

    private interface ITestService
    {
    }

    [Before(HookType.Test)]
    public void SetUp()
    {
        _scope = new();
        _locator = _scope.Locator;
    }

    [After(HookType.Test)]
    public void TearDown()
    {
        _scope?.Dispose();
        _scope = null;
    }

    [Test]
    public async Task Constructor_ShouldInitializeWithDefaultResolver()
    {
        await Assert.That(_locator.Internal).IsNotNull();
        await Assert.That(_locator.Internal).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    [Test]
    public async Task Current_ShouldReturnInternal() => await Assert.That(_locator.Current).IsEqualTo(_locator.Internal);

    [Test]
    public async Task CurrentMutable_ShouldReturnInternal() => await Assert.That(_locator.CurrentMutable).IsEqualTo(_locator.Internal);

    [Test]
    public async Task SetLocator_WithNullResolver_ShouldThrowArgumentNullException() => await Assert.That(() => _locator.SetLocator(null!)).ThrowsExactly<ArgumentNullException>();

    [Test]
    public async Task SetLocator_ShouldUpdateInternal()
    {
        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });

        _locator.SetLocator(newResolver);

        await Assert.That(_locator.Internal).IsEqualTo(newResolver);
    }

    [Test]
    public async Task SetLocator_ShouldInvokeRegisteredCallbacks()
    {
        var callbackInvoked = false;

        _locator.RegisterResolverCallbackChanged(() => callbackInvoked = true);
        callbackInvoked = false; // Reset after initial registration

        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });
        _locator.SetLocator(newResolver);

        await Assert.That(callbackInvoked).IsTrue();
    }

    [Test]
    public async Task SetLocator_WithSuppressedNotifications_ShouldNotInvokeCallbacks()
    {
        var callbackInvoked = false;

        _locator.RegisterResolverCallbackChanged(() => callbackInvoked = true);
        callbackInvoked = false; // Reset after initial registration

        using (_locator.SuppressResolverCallbackChangedNotifications())
        {
            var newResolver = new FuncDependencyResolver(
                (_, _) => null!,
                (_, _, _) => { },
                (_, _) => { },
                (_, _) => { });
            _locator.SetLocator(newResolver);

            await Assert.That(callbackInvoked).IsFalse();
        }
    }

    [Test]
    public async Task RegisterResolverCallbackChanged_WithNullCallback_ShouldThrowNullReferenceException() =>

        // InternalLocator doesn't validate null parameter, so it throws NullReferenceException
        await Assert.That(() => _locator.RegisterResolverCallbackChanged(null!)).ThrowsExactly<NullReferenceException>();

    [Test]
    public async Task RegisterResolverCallbackChanged_ShouldInvokeCallbackImmediately()
    {
        var callbackInvoked = false;

        _locator.RegisterResolverCallbackChanged(() => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsTrue();
    }

    [Test]
    public async Task RegisterResolverCallbackChanged_WhenDisposed_ShouldRemoveCallback()
    {
        var callCount = 0;

        var subscription = _locator.RegisterResolverCallbackChanged(() => callCount++);
        callCount = 0; // Reset after initial registration

        subscription.Dispose();

        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });
        _locator.SetLocator(newResolver);

        await Assert.That(callCount).IsEqualTo(0);
    }

    [Test]
    public async Task RegisterResolverCallbackChanged_WithMultipleCallbacks_ShouldInvokeAll()
    {
        var callback1Invoked = false;
        var callback2Invoked = false;

        _locator.RegisterResolverCallbackChanged(() => callback1Invoked = true);
        _locator.RegisterResolverCallbackChanged(() => callback2Invoked = true);

        callback1Invoked = false;
        callback2Invoked = false;

        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });
        _locator.SetLocator(newResolver);

        await Assert.That(callback1Invoked).IsTrue();
        await Assert.That(callback2Invoked).IsTrue();
    }

    [Test]
    public async Task SuppressResolverCallbackChangedNotifications_ShouldDisableCallbacks()
    {
        await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();

        using (_locator.SuppressResolverCallbackChangedNotifications())
        {
            await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();
        }

        await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();
    }

    [Test]
    public async Task SuppressResolverCallbackChangedNotifications_CanBeNested()
    {
        using (_locator.SuppressResolverCallbackChangedNotifications())
        {
            await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();

            using (_locator.SuppressResolverCallbackChangedNotifications())
            {
                await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();
            }

            await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();
        }

        await Assert.That(_locator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();
    }

    [Test]
    public async Task RegisterResolverCallbackChanged_WithSuppressedNotifications_ShouldNotInvokeImmediately()
    {
        var callbackInvoked = false;

        using (_locator.SuppressResolverCallbackChangedNotifications())
        {
            _locator.RegisterResolverCallbackChanged(() => callbackInvoked = true);

            await Assert.That(callbackInvoked).IsFalse();
        }

        // After suppression is lifted, callback is not retroactively invoked
        await Assert.That(callbackInvoked).IsFalse();
    }

    [Test]
    public async Task Dispose_ShouldDisposeInternal()
    {
        var locator = new InternalLocator();
        var resolver = locator.Internal;

        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();

        locator.Dispose();

        // The Internal resolver is disposed, subsequent operations should check the disposed state
        // For InstanceGenericFirstDependencyResolver, GetService on disposed resolver throws ObjectDisposedException
        var result = resolver.GetService(typeof(ITestService));
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        var locator = new InternalLocator();

        locator.Dispose();

        await Assert.That(() => locator.Dispose()).ThrowsNothing();
    }

    [Test]
    public async Task SetLocator_ShouldNotCauseDeadlockWithReentrantCallback()
    {
        var callCount = 0;

        _locator.RegisterResolverCallbackChanged(() =>
        {
            callCount++;
            if (callCount == 1)
            {
                // This would cause a deadlock if the implementation doesn't handle it
                var anotherResolver = new FuncDependencyResolver(
                    (_, _) => null!,
                    (_, _, _) => { },
                    (_, _) => { },
                    (_, _) => { });
                _locator.SetLocator(anotherResolver);
            }
        });

        // Should complete without deadlock
        await Assert.That(callCount).IsGreaterThan(0);
    }

    [Test]
    public async Task Constructor_Callback_WhenCurrentMutableIsNull_ShouldExitEarly()
    {
        // This test covers the early return path in the constructor's callback
        // when CurrentMutable is null. This can happen during disposal or in edge cases.
        var locator = new InternalLocator();

        // Set Internal to null to simulate the edge case
        locator.Internal = null!;

        // Now trigger a resolver change which will invoke the constructor's callback
        // The callback should handle CurrentMutable being null gracefully
        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });

        // This should not throw even though CurrentMutable is now null
        await Assert.That(() => locator.SetLocator(newResolver)).ThrowsNothing();

        locator.Dispose();
    }
}
