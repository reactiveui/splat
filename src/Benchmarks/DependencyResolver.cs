// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Splat.Benchmarks
{
#pragma warning disable CA1001, CA1063

    /// <summary>
    /// <see cref="IDependencyResolver"/> implementation for benchmarking the Locator.
    /// </summary>
    /// <seealso cref="Splat.IDependencyResolver" />
    public class DependencyResolver : IDependencyResolver
    {
        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null) => default(object);

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null) => Enumerable.Empty<object>();

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback) => Disposable.Empty;

        /// <inheritdoc />
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public bool HasRegistration(Type serviceType, string contract = null) => true;
    }

#pragma warning restore CA1001, CA1063
}
