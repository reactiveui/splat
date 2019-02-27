// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Splat
{
    internal class InternalLocator : IDisposable
    {
        // this has been done to have a default single instance. but allow isolation in unit tests.B
        private readonly List<Action> _resolverChanged = new List<Action>();
        private volatile int _resolverChangedNotificationSuspendCount;
        private IDependencyResolver _dependencyResolver;

        internal InternalLocator()
        {
            _dependencyResolver = new ModernDependencyResolver();

            RegisterResolverCallbackChanged(() =>
            {
                if (CurrentMutable == null)
                {
                    return;
                }

                CurrentMutable.InitializeSplat();
            });
        }

        /// <summary>
        /// Gets the read only dependency resolver. This class is used throughout
        /// libraries for many internal operations as well as for general use
        /// by applications. If this isn't assigned on startup, a default, highly
        /// capable implementation will be used, and it is advised for most people
        /// to simply use the default implementation.
        /// </summary>
        /// <value>The dependency resolver.</value>
        public IReadonlyDependencyResolver Current => _dependencyResolver;

        /// <summary>
        /// Gets the mutable dependency resolver.
        /// The default resolver is also a mutable resolver, so this will be non-null.
        /// Use this to register new types on startup if you are using the default resolver.
        /// </summary>
        public IMutableDependencyResolver CurrentMutable => _dependencyResolver;

        internal IDependencyResolver Internal => _dependencyResolver;

        public void Dispose()
        {
            _dependencyResolver?.Dispose();
        }

        /// <summary>
        /// Allows setting the dependency resolver.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver to set.</param>
        public void SetLocator(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));

            // DV: is this needed if we're changing the behaviour of setlocator?
            /*
            if (ModeDetector.InUnitTestRunner())
            {
                _unitTestDependencyResolver = value;
                _dependencyResolver = _dependencyResolver ?? value;
            }
            else
            {
                _dependencyResolver = value;
            }
            */
            if (AreResolverCallbackChangedNotificationsEnabled())
            {
                var currentCallbacks = default(Action[]);
                lock (_resolverChanged)
                {
                    // NB: Prevent deadlocks should we reenter this setter from
                    // the callbacks
                    currentCallbacks = _resolverChanged.ToArray();
                }

                foreach (var block in currentCallbacks)
                {
                    block();
                }
            }
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
        public IDisposable RegisterResolverCallbackChanged(Action callback)
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
                    _resolverChanged.Remove(callback);
                }
            });
        }

        /// <summary>
        /// This method will prevent resolver changed notifications from happening until
        /// the returned <see cref="IDisposable"/> is disposed.
        /// </summary>
        /// <returns>A disposable which when disposed will indicate the change
        /// notification is no longer needed.</returns>
        public IDisposable SuppressResolverCallbackChangedNotifications()
        {
            Interlocked.Increment(ref _resolverChangedNotificationSuspendCount);

            return new ActionDisposable(() => Interlocked.Decrement(ref _resolverChangedNotificationSuspendCount));
        }

        /// <summary>
        /// Indicates if the we are notifying external classes of updates to the resolver being changed.
        /// </summary>
        /// <returns>A value indicating whether the notifications are happening.</returns>
        public bool AreResolverCallbackChangedNotificationsEnabled()
        {
            return _resolverChangedNotificationSuspendCount == 0;
        }
    }
}
