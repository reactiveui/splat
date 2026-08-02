// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Primitives.Disposables;

namespace Splat;

/// <summary>
/// Provides an internal mechanism for managing dependency resolver instances and notifications within the application.
/// Supports registering callbacks for resolver changes and allows controlled suppression of change notifications.
/// </summary>
/// <remarks>This class is intended for internal use to coordinate dependency resolution and notification logic.
/// It enables libraries to react to resolver changes and supports isolation for testing scenarios. Alongside the
/// process-wide resolver an async flow can install its own resolver through <see cref="WithResolver"/>; that override
/// only exists for the flow that installed it, so overlapping flows never observe each other's resolver or
/// notification suppression. Thread safety is maintained for callback registration and notification suppression.
/// Dispose the instance to release associated resources when no longer needed.</remarks>
internal class InternalLocator : IDisposable
{
    /// <summary>Registration-change callbacks. A process-wide default instance is used, while still allowing isolation in unit tests.</summary>
    private readonly List<Action> _resolverChanged = new(4);

    /// <summary>Subscription that raises registration-change notifications; disposed with this instance.</summary>
    private readonly IDisposable _resolverChangedNotification;

    /// <summary>The resolver installed for the calling async flow, if that flow entered a <see cref="WithResolver"/> scope.</summary>
    private readonly AsyncLocal<ResolverOverride?> _flowOverride = new();

    /// <summary>
    /// Number of <see cref="WithResolver"/> scopes alive anywhere in the process; while zero, reads skip the flow
    /// lookup entirely. Either stale answer is still correct: a stale zero can only be seen by a flow that installed
    /// no scope of its own (a flow that installed one incremented before it reads), and a stale non-zero merely costs
    /// the flow lookup, which then finds nothing.
    /// </summary>
    private int _flowOverrideCount;

    /// <summary>Reentrancy counter; while greater than zero, change notifications are suppressed process wide.</summary>
    private int _resolverChangedNotificationSuspendCount;

    /// <summary>The resolver used by every flow that has not installed one of its own.</summary>
    private IDependencyResolver _processWide;

    /// <summary>Guards against running the dispose logic more than once.</summary>
    private bool _disposedValue;

    /// <summary>Initializes a new instance of the <see cref="InternalLocator"/> class.</summary>
    internal InternalLocator()
    {
        _processWide = new InstanceGenericFirstDependencyResolver();

        // CurrentMutable returns the non-nullable process-wide resolver (set above and only ever replaced via the null-guarded SetLocator), so it is never null here.
        _resolverChangedNotification = RegisterResolverCallbackChanged(() => AppLocator.ReInit(CurrentMutable));
    }

    /// <summary>
    /// Gets the read only dependency resolver. This class is used throughout
    /// libraries for many internal operations as well as for general use
    /// by applications. If this isn't assigned on startup, a default, highly
    /// capable implementation will be used, and it is advised for most people
    /// to simply use the default implementation.
    /// </summary>
    /// <value>The dependency resolver.</value>
    internal IReadonlyDependencyResolver Current => Internal;

    /// <summary>
    /// Gets the mutable dependency resolver.
    /// The default resolver is also a mutable resolver, so this will be non-null.
    /// Use this to register new types on startup if you are using the default resolver.
    /// </summary>
    internal IMutableDependencyResolver CurrentMutable => Internal;

    /// <summary>Gets or sets the dependency resolver used internally by the component.</summary>
    /// <value>
    /// Reads return the resolver installed by the calling flow's <see cref="WithResolver"/> scope when there is one,
    /// and the process-wide resolver otherwise. The setter always replaces the process-wide resolver; use
    /// <see cref="SetLocator"/> to write through to whichever resolver the calling flow is actually using.
    /// </value>
    internal IDependencyResolver Internal
    {
        get => Volatile.Read(ref _flowOverrideCount) == 0 ? _processWide : _flowOverride.Value?.Resolver ?? _processWide;
        set => _processWide = value;
    }

    /// <summary>Gets the resolver override installed by the calling async flow, or <c>null</c> when the flow uses the process-wide resolver.</summary>
    private ResolverOverride? FlowOverride => Volatile.Read(ref _flowOverrideCount) == 0 ? null : _flowOverride.Value;

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Allows setting the dependency resolver.</summary>
    /// <remarks>When the calling flow is inside a <see cref="WithResolver"/> scope the new resolver replaces that
    /// scope's resolver and is discarded when the scope ends; otherwise it replaces the process-wide resolver.</remarks>
    /// <param name="dependencyResolver">The dependency resolver to set.</param>
    internal void SetLocator(IDependencyResolver dependencyResolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(dependencyResolver);

        var flowOverride = FlowOverride;
        if (flowOverride is null)
        {
            _processWide = dependencyResolver;
        }
        else
        {
            flowOverride.Resolver = dependencyResolver;
        }

        NotifyResolverChanged();
    }

    /// <summary>Installs <paramref name="resolver"/> for the calling async flow until the returned scope is disposed.</summary>
    /// <remarks>The override travels with the async flow - across awaits and thread hops - and is invisible to every
    /// other flow, so tests running concurrently can each hold their own resolver. Scopes nest, and disposing one
    /// restores the resolver the flow was using beforehand.</remarks>
    /// <param name="resolver">The resolver the calling flow should resolve from. Must not be null.</param>
    /// <param name="suppressResolverCallback">
    /// <c>true</c> to keep resolver-changed notifications suppressed for this flow while the scope is open; otherwise
    /// <c>false</c>, which raises them as the scope is entered and again as it is left.
    /// </param>
    /// <returns>A scope which, when disposed, restores the flow's previous resolver.</returns>
    internal IDisposable WithResolver(IDependencyResolver resolver, bool suppressResolverCallback)
    {
        var previous = _flowOverride.Value;
        var notificationsSuppressed = suppressResolverCallback || previous?.NotificationsSuppressed == true;

        _ = Interlocked.Increment(ref _flowOverrideCount);
        _flowOverride.Value = new(resolver, notificationsSuppressed);

        if (!suppressResolverCallback)
        {
            NotifyResolverChanged();
        }

        return new ResolverOverrideScope(this, previous, suppressResolverCallback);
    }

    /// <summary>
    /// This method allows libraries to register themselves to be set up
    /// whenever the dependency resolver changes. Applications should avoid
    /// this method, it is usually used for libraries that depend on service
    /// location.
    /// </summary>
    /// <param name="callback">A callback that is invoked when the
    /// resolver is changed. This callback is also invoked immediately,
    /// to configure the current resolver.</param>
    /// <returns>When disposed, removes the callback. You probably can
    /// ignore this.</returns>
    internal IDisposable RegisterResolverCallbackChanged(Action callback)
    {
        lock (_resolverChanged)
        {
            _resolverChanged.Add(callback);
        }

        // NB: We always immediately invoke the callback to set up the
        // current resolver with whatever we've got
        if (AreResolverCallbackChangedNotificationsEnabled())
        {
            callback();
        }

        return new ActionDisposable(() =>
        {
            lock (_resolverChanged)
            {
                _ = _resolverChanged.Remove(callback);
            }
        });
    }

    /// <summary>This method will prevent resolver changed notifications from happening until the returned <see cref="IDisposable"/> is disposed.</summary>
    /// <remarks>Suppression requested here applies process wide; use <see cref="WithResolver"/> to confine it to the calling flow.</remarks>
    /// <returns>A disposable which when disposed will indicate the change
    /// notification is no longer needed.</returns>
    internal IDisposable SuppressResolverCallbackChangedNotifications()
    {
        _ = Interlocked.Increment(ref _resolverChangedNotificationSuspendCount);

        return new ActionDisposable(() => Interlocked.Decrement(ref _resolverChangedNotificationSuspendCount));
    }

    /// <summary>Indicates if the we are notifying external classes of updates to the resolver being changed.</summary>
    /// <returns>A value indicating whether the notifications are happening.</returns>
    internal bool AreResolverCallbackChangedNotificationsEnabled() =>
        Volatile.Read(ref _resolverChangedNotificationSuspendCount) == 0 && FlowOverride?.NotificationsSuppressed != true;

    /// <summary>Releases the unmanaged resources used by the object and optionally releases the managed resources.</summary>
    /// <remarks>This method is called by public Dispose methods and the finalizer. When disposing is true,
    /// this method releases all resources held by managed objects. When disposing is false, only unmanaged resources
    /// are released. Override this method to release additional resources in a derived class.</remarks>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _processWide.Dispose();
            _resolverChangedNotification.Dispose();
        }

        _disposedValue = true;
    }

    /// <summary>Runs the registered resolver-changed callbacks unless notifications are currently suppressed.</summary>
    private void NotifyResolverChanged()
    {
        if (!AreResolverCallbackChangedNotificationsEnabled())
        {
            return;
        }

        Action[] currentCallbacks;
        lock (_resolverChanged)
        {
            // NB: Prevent deadlocks should we reenter this setter from
            // the callbacks
            currentCallbacks = [.. _resolverChanged];
        }

        foreach (var block in currentCallbacks)
        {
            block();
        }
    }

    /// <summary>The resolver, and the notification state, that one async flow is using.</summary>
    /// <param name="resolver">The resolver the flow resolves from.</param>
    /// <param name="notificationsSuppressed">Whether resolver-changed notifications are suppressed for the flow.</param>
    private sealed class ResolverOverride(IDependencyResolver resolver, bool notificationsSuppressed)
    {
        /// <summary>Gets or sets the resolver the flow resolves from.</summary>
        public IDependencyResolver Resolver { get; set; } = resolver;

        /// <summary>Gets a value indicating whether resolver-changed notifications are suppressed for the flow.</summary>
        public bool NotificationsSuppressed { get; } = notificationsSuppressed;
    }

    /// <summary>Restores the flow's previous resolver when disposed.</summary>
    /// <param name="locator">The locator holding the override.</param>
    /// <param name="previous">The override the flow had before this scope was entered, if any.</param>
    /// <param name="suppressResolverCallback">Whether the scope was entered with resolver-changed notifications suppressed.</param>
    private sealed class ResolverOverrideScope(InternalLocator locator, ResolverOverride? previous, bool suppressResolverCallback) : IDisposable
    {
        /// <summary>Non-zero once the scope has been disposed, so a second dispose is a no-op.</summary>
        private int _disposed;

        /// <summary>Restores the flow's previous resolver.</summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }

            locator._flowOverride.Value = previous;
            _ = Interlocked.Decrement(ref locator._flowOverrideCount);

            if (suppressResolverCallback)
            {
                return;
            }

            locator.NotifyResolverChanged();
        }
    }
}
