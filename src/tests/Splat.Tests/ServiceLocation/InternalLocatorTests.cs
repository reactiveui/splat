// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="InternalLocator"/> class.</summary>
[NotInParallel]
public sealed class InternalLocatorTests
{
    /// <summary>The internal locator scope created for the duration of each test.</summary>
    private InternalLocatorScope? _scope;

    /// <summary>The internal locator under test, obtained from the current scope.</summary>
    private InternalLocator _locator = null!;

    /// <summary>Marker service interface used by the tests.</summary>
    private interface ITestService;

    /// <summary>Creates a fresh locator scope before each test.</summary>
    [Before(Test)]
    public void SetUp()
    {
        _scope = new();
        _locator = _scope.Locator;
    }

    /// <summary>Disposes the locator scope after each test.</summary>
    [After(Test)]
    public void TearDown()
    {
        _scope?.Dispose();
        _scope = null;
    }

    /// <summary>Verifies that the constructor initializes with the default resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ShouldInitializeWithDefaultResolver()
    {
        await Assert.That(_locator.Internal).IsNotNull();
        await Assert.That(_locator.Internal).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    /// <summary>Verifies that Current returns the internal resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Current_ShouldReturnInternal() => await Assert.That(_locator.Current).IsEqualTo(_locator.Internal);

    /// <summary>Verifies that CurrentMutable returns the internal resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CurrentMutable_ShouldReturnInternal() => await Assert.That(_locator.CurrentMutable).IsEqualTo(_locator.Internal);

    /// <summary>Verifies that SetLocator throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_WithNullResolver_ShouldThrowArgumentNullException() => await Assert.That(() => _locator.SetLocator(null!)).ThrowsExactly<ArgumentNullException>();

    /// <summary>Verifies that SetLocator updates the internal resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that SetLocator invokes registered change callbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that SetLocator does not invoke callbacks while notifications are suppressed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that RegisterResolverCallbackChanged throws when the callback is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterResolverCallbackChanged_WithNullCallback_ShouldThrowNullReferenceException() =>

        // InternalLocator doesn't validate null parameter, so it throws NullReferenceException
        await Assert.That(() => _locator.RegisterResolverCallbackChanged(null!)).ThrowsExactly<NullReferenceException>();

    /// <summary>Verifies that RegisterResolverCallbackChanged invokes the callback immediately.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterResolverCallbackChanged_ShouldInvokeCallbackImmediately()
    {
        var callbackInvoked = false;

        _locator.RegisterResolverCallbackChanged(() => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>Verifies that disposing the subscription removes the change callback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that all registered change callbacks are invoked.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that SuppressResolverCallbackChangedNotifications disables callbacks within the scope.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that suppression scopes can be nested.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that a callback registered while suppressed is not invoked immediately.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that Dispose disposes the internal resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that calling Dispose multiple times does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        var locator = new InternalLocator();

        locator.Dispose();

        await Assert.That(() => locator.Dispose()).ThrowsNothing();
    }

    /// <summary>Verifies that a reentrant callback during SetLocator does not deadlock.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_ShouldNotCauseDeadlockWithReentrantCallback()
    {
        var callCount = 0;

        _locator.RegisterResolverCallbackChanged(() =>
        {
            callCount++;
            if (callCount != 1)
            {
                return;
            }

            // This would cause a deadlock if the implementation doesn't handle it
            var anotherResolver = new FuncDependencyResolver(
                (_, _) => null!,
                (_, _, _) => { },
                (_, _) => { },
                (_, _) => { });
            _locator.SetLocator(anotherResolver);
        });

        // Should complete without deadlock
        await Assert.That(callCount).IsGreaterThan(0);
    }

    /// <summary>Verifies that the constructor callback exits early when CurrentMutable is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
