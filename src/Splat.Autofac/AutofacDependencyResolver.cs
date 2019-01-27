// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Splat.Autofac
{
    /// <summary>
    /// Autofac implementation for <see cref="IMutableDependencyResolver"/>.
    /// </summary>
    public class AutofacDependencyResolver : IMutableDependencyResolver
    {
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AutofacDependencyResolver(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public virtual object GetService(Type serviceType, string contract = null)
        {
            try
            {
                return string.IsNullOrEmpty(contract)
                    ? _container.Resolve(serviceType)
                    : _container.ResolveNamed(contract, serviceType);
            }
            catch (DependencyResolutionException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            try
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                object instance = string.IsNullOrEmpty(contract)
                    ? _container.Resolve(enumerableType)
                    : _container.ResolveNamed(contract, enumerableType);
                return ((IEnumerable)instance).Cast<object>();
            }
            catch (DependencyResolutionException)
            {
                return null;
            }
        }

        /// <summary>
        /// Register a function with the resolver which will generate a object
        /// for the specified service type.
        /// Optionally a contract can be registered which will indicate
        /// that that registration will only work with that contract.
        /// Most implementations will use a stack based approach to allow for multile items to be registered.
        /// </summary>
        /// <param name="factory">The factory function which generates our object.</param>
        /// <param name="serviceType">The type which is used for the registration.</param>
        /// <param name="contract">A optional contract value which will indicates to only generate the value if this contract is specified.</param>
        [SuppressMessage("Design", "CS0168: Obsolete method call", Justification = "Needed for container generation")]
        public virtual void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            var builder = new ContainerBuilder();
            if (string.IsNullOrEmpty(contract))
            {
                builder.Register(x => factory()).As(serviceType).AsImplementedInterfaces();
            }
            else
            {
                builder.Register(x => factory()).Named(contract, serviceType).AsImplementedInterfaces();
            }

            builder.Update(_container);
        }

        /// <summary>
        /// Unregisters the current item based on the specified type and contract.
        /// https://stackoverflow.com/questions/5091101/is-it-possible-to-remove-an-existing-registration-from-autofac-container-builder.
        /// </summary>
        /// <param name="serviceType">The service type to unregister.</param>
        /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
        /// <exception cref="System.NotImplementedException">This is not implemented by default.</exception>
        /// <inheritdoc />
        public virtual void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unregisters all the values associated with the specified type and contract.
        /// https://stackoverflow.com/questions/5091101/is-it-possible-to-remove-an-existing-registration-from-autofac-container-builder.
        /// </summary>
        /// <param name="serviceType">The service type to unregister.</param>
        /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
        /// <exception cref="System.NotImplementedException">This is not implemented by default.</exception>
        /// <inheritdoc />
        public virtual void UnregisterAll(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            // this method is not used by RxUI
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
