// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Ninject;

namespace Splat.Ninject
{
    /// <summary>
    /// Ninject implementation for <see cref="IMutableDependencyResolver"/>.
    /// </summary>
    /// <seealso cref="IMutableDependencyResolver" />
    public class NinjectDependencyResolver : IMutableDependencyResolver
    {
        private IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? _kernel.Get(serviceType)
                : _kernel.Get(serviceType, contract);

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? _kernel.GetAll(serviceType)
                : _kernel.GetAll(serviceType, contract);

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null) =>
            _kernel.Bind(serviceType).ToMethod(_ => factory());

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null) => _kernel.Unbind(serviceType);

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
                _kernel?.Dispose();
                _kernel = null;
            }
        }
    }
}
