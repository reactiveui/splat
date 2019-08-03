// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;

namespace Splat.SimpleInjector
{
    /// <summary>
    /// Simple Injector implementation for <see cref="IMutableDependencyResolver"/>.
    /// </summary>
    /// <seealso cref="Splat.IDependencyResolver" />
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            return _container.GetInstance(serviceType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null) =>
            _container.GetAllInstances(serviceType);

        /// <inheritdoc />
        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return _container.GetCurrentRegistrations().Any(x => x.ServiceType == serviceType);
        }

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null) =>
            _container.Register(serviceType, factory);

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        /// <param name="disposing">Whether or not the instance is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container?.Dispose();
                _container = null;
            }
        }
    }
}
