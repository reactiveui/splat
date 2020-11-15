// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splat.SimpleInjector
{
    /// <summary>
    /// sad.
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class SimpleInjectorInitializer : IDependencyResolver
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        /// <summary>
        /// Gets dictionary of registered factories.
        /// </summary>
        public Dictionary<Type, List<Func<object>>> RegisteredFactories { get; }
            = new Dictionary<Type, List<Func<object>>>();

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            Func<object> fact = RegisteredFactories[serviceType].LastOrDefault();
            return fact?.Invoke();
        }

        /// <inheritdoc/>
        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return RegisteredFactories[serviceType]
                .Select(n => n());
        }

        /// <inheritdoc />
        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return RegisteredFactories.TryGetValue(serviceType, out List<Func<object>> values)
                   && values.Any();
        }

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (!RegisteredFactories.ContainsKey(serviceType))
            {
                RegisteredFactories.Add(serviceType, new List<Func<object>>());
            }

            RegisteredFactories[serviceType].Add(factory);
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            if (RegisteredFactories.ContainsKey(serviceType))
            {
                RegisteredFactories.Remove(serviceType);
            }
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
        }
    }
}
