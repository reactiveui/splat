// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Microsoft DI implementation for <see cref="IDependencyResolver"/>.
    /// </summary>
    /// <seealso cref="IDependencyResolver" />
    [SuppressMessage("Globalization", "CA1303", Justification = "Unnecessary warning")]
    public class MicrosoftDependencyResolver : IDependencyResolver
    {
        private const string ImmutableExceptionMessage = "This container has already been built and cannot be modified.";
        private readonly Lazy<IDictionary<string, IServiceScope>> _serviceScopes = new Lazy<IDictionary<string, IServiceScope>>(() => new Dictionary<string, IServiceScope>());
        private IServiceCollection _serviceCollection;
        private bool _isImmutable;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with an <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
        public MicrosoftDependencyResolver(IServiceCollection services = null)
        {
            _serviceCollection = services ?? new ServiceCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with a configured service Provider.
        /// </summary>
        /// <param name="serviceProvider">A ready to use service provider.</param>
        public MicrosoftDependencyResolver(IServiceProvider serviceProvider)
            : this()
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
            _isImmutable = true;
        }

        /// <inheritdoc />
        public virtual object GetService(Type serviceType, string contract = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var sp = GetServiceProvider(contract);
            return sp.GetService(serviceType);
        }

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var sp = GetServiceProvider(contract);
            return sp.GetServices(serviceType);
        }

        /// <inheritdoc />
        public virtual void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (_isImmutable)
            {
                throw new InvalidOperationException(ImmutableExceptionMessage);
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (contract != null)
            {
                _serviceCollection.AddScoped(serviceType, _ => factory());
            }
            else
            {
                _serviceCollection.AddTransient(serviceType);
            }
        }

        /// <summary>
        /// Unregisters the current item based on the specified type and contract.
        /// </summary>
        /// <param name="serviceType">The service type to unregister.</param>
        /// <param name="contract">This parameter is ignored. Service will be removed from all contracts.</param>
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            if (_isImmutable)
            {
                throw new InvalidOperationException(ImmutableExceptionMessage);
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var last = _serviceCollection.LastOrDefault(sd => sd.ServiceType == serviceType);
            if (last != null)
            {
                _serviceCollection.Remove(last);
            }
        }

        /// <summary>
        /// Unregisters all the values associated with the specified type and contract.
        /// </summary>
        /// <param name="serviceType">The service type to unregister.</param>
        /// <param name="contract">This parameter is ignored. Service will be removed from all contracts.</param>
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            if (_isImmutable)
            {
                throw new InvalidOperationException(ImmutableExceptionMessage);
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var registrations = _serviceCollection
                .Where(sd => sd.ServiceType == serviceType)
                .ToList();

            foreach (var sd in registrations)
            {
                _serviceCollection.Remove(sd);
            }
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            // this method is not used by RxUI
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool HasRegistration(Type serviceType) => _serviceCollection.Any(sd => sd.ServiceType == serviceType);

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
        }

        private IServiceProvider GetServiceProvider(string contract = null, bool createIfNotExists = false)
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            }

            var dic = _serviceScopes.Value;

            if (contract == null)
            {
                return _serviceProvider;
            }

            if (!dic.TryGetValue(contract, out var scope) && createIfNotExists)
            {
                dic[contract] = scope = _serviceProvider.CreateScope();
            }

            return scope?.ServiceProvider ?? _serviceProvider;
        }
    }
}
