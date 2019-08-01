// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
        private const string MutableExceptionMessage = "The container has not yet been built.";
        private const string ImmutableExceptionMessage = "This container has already been built and cannot be modified.";
        private static readonly Type _dictionaryType = typeof(ContractDictionary<>);
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
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
            _isImmutable = true;
        }

        /// <summary>
        /// Gets the internal Microsoft conainer,
        /// or build new if this instance was not initialized with one.
        /// </summary>
        protected virtual IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = _serviceCollection.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }

        /// <inheritdoc />
        public virtual object GetService(Type serviceType, string contract = null) =>
            GetServices(serviceType, contract).LastOrDefault();

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                return ServiceProvider.GetServices(serviceType);
            }

            var dic = GetContractDictionary(serviceType, false);
            return dic?.GetFactories(contract);
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

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;

            if (string.IsNullOrWhiteSpace(contract))
            {
                _serviceCollection.AddTransient(serviceType, _ => factory());
            }
            else
            {
                var dic = GetContractDictionary(serviceType, true);

                dic.AddFactory(contract, factory);
            }
        }

        /// <inheritdoc/>
        public virtual void UnregisterCurrent(Type serviceType, string contract = null)
        {
            if (_isImmutable)
            {
                throw new InvalidOperationException(ImmutableExceptionMessage);
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;

            if (contract == null)
            {
                var sd = _serviceCollection.LastOrDefault(s => s.ServiceType == serviceType);
                _serviceCollection.Remove(sd);
                return;
            }

            var dic = GetContractDictionary(serviceType, false);
            if (dic != null)
            {
                dic.RemoveLastFactory(contract);
                if (dic.Count == 0)
                {
                    RemoveContractService(serviceType);
                }
            }
        }

        /// <summary>
        /// Unregisters all the values associated with the specified type and contract - or -
        /// If the container has already been built, removes the specified contract (scope) entirely,
        /// ignoring the <paramref name="serviceType"/> argument.
        /// </summary>
        /// <param name="serviceType">The service type to unregister.</param>
        /// <param name="contract">This parameter is ignored. Service will be removed from all contracts.</param>
        public virtual void UnregisterAll(Type serviceType, string contract = null)
        {
            if (_isImmutable)
            {
                throw new InvalidOperationException(ImmutableExceptionMessage);
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;

            if (contract == null)
            {
                var sds = _serviceCollection
                    .Where(s => s.ServiceType == serviceType)
                    .ToList();

                foreach (var sd in sds)
                {
                    _serviceCollection.Remove(sd);
                }
            }

            var dic = GetContractDictionary(serviceType, false);
            if (dic != null && dic.TryRemove(contract, out var _) && dic.Count == 0)
            {
                RemoveContractService(serviceType);
            }
        }

        /// <inheritdoc />
        public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            // this method is not used by RxUI
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual bool HasRegistration(Type serviceType)
        {
            if (!_isImmutable)
            {
                return _serviceCollection.Any(sd => sd.ServiceType == serviceType);
            }

            var service = _serviceProvider.GetService(serviceType);
            return service != null;
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
        }

        private static Type GetDictionaryType(Type serviceType) => _dictionaryType.MakeGenericType(serviceType);

        private void RemoveContractService(Type serviceType)
        {
            var dicType = GetDictionaryType(serviceType);
            var sd = _serviceCollection.SingleOrDefault(s => s.ServiceType == serviceType);
            _serviceCollection.Remove(sd);
        }

        private ContractDictionary GetContractDictionary(Type serviceType, bool createIfNotExists)
        {
            var dicType = GetDictionaryType(serviceType);

            if (_isImmutable)
            {
                return (ContractDictionary)ServiceProvider.GetService(dicType);
            }

            var dic = _serviceCollection
                .Where(sd => sd.ServiceType == dicType)
                .Select(sd => sd.ImplementationInstance)
                .Cast<ContractDictionary>()
                .SingleOrDefault();

            if (createIfNotExists && dic == null)
            {
                dic = (ContractDictionary)Activator.CreateInstance(dicType);

                _serviceCollection.AddSingleton(dic);
            }

            return dic;
        }

        private class ContractDictionary : ConcurrentDictionary<string, IList<Func<object>>>
        {
            public Func<object> GetFactorý(string contract)
            {
                return GetFactories(contract)
                    .LastOrDefault();
            }

            public IEnumerable<Func<object>> GetFactories(string contract)
            {
                if (TryGetValue(contract, out var collection))
                {
                    return collection;
                }

                return Enumerable.Empty<Func<object>>();
            }

            public void AddFactory(string contract, Func<object> factory)
            {
                if (!TryGetValue(contract, out var collection))
                {
                    this[contract] = collection = new List<Func<object>>();
                }

                collection.Add(factory);
            }

            public void RemoveLastFactory(string contract)
            {
                if (TryGetValue(contract, out var collection))
                {
                    var lastIndex = collection.Count - 1;
                    if (lastIndex > 0)
                    {
                        collection.RemoveAt(lastIndex);
                        if (collection.Count == 0)
                        {
                            TryRemove(contract, out var _);
                        }
                    }
                }
            }
        }

        private class ContractDictionary<T> : ContractDictionary
        {
        }
    }
}
