// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;

namespace Splat.DryIoc
{
    /// <summary>
    /// DryIoc implementation for <see cref="IMutableDependencyResolver"/>.
    /// https://bitbucket.org/dadhi/dryioc/wiki/Home.
    /// </summary>
    /// <seealso cref="Splat.IDependencyResolver" />
    public class DryIocDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DryIocDependencyResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DryIocDependencyResolver(IContainer? container = null)
        {
            _container = container ?? new Container();
        }

        /// <inheritdoc />
        public virtual object? GetService(Type? serviceType, string? contract = null)
        {
            var isNull = serviceType is null;
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var key = (serviceType, contract ?? string.Empty);
            var registeredinSplat = _container.ResolveMany(serviceType, serviceKey: key).Select(x => isNull ? ((NullServiceType)x).Factory()! : x);
            if (registeredinSplat.Any())
            {
                return registeredinSplat.LastOrDefault();
            }

            var registeredWithContract = _container.ResolveMany(serviceType, serviceKey: contract).Select(x => isNull ? ((NullServiceType)x).Factory()! : x);
            if (registeredWithContract.Any())
            {
                return registeredWithContract.LastOrDefault();
            }

            return _container.ResolveMany(serviceType).Select(x => isNull ? ((NullServiceType)x).Factory()! : x).LastOrDefault();
        }

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
        {
            var isNull = serviceType is null;
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var key = (serviceType, contract ?? string.Empty);
            var registeredinSplat = _container.ResolveMany(serviceType, serviceKey: key).Select(x => isNull ? ((NullServiceType)x).Factory()! : x);
            if (registeredinSplat.Any())
            {
                return registeredinSplat;
            }

            var registeredWithContract = _container.ResolveMany(serviceType, serviceKey: contract).Select(x => isNull ? ((NullServiceType)x).Factory()! : x);
            if (registeredWithContract.Any())
            {
                return registeredWithContract;
            }

            return _container.ResolveMany(serviceType).Select(x => isNull ? ((NullServiceType)x).Factory()! : x);
        }

        /// <inheritdoc />
        public bool HasRegistration(Type? serviceType, string? contract = null)
        {
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            return _container.GetServiceRegistrations().Any(x =>
            {
                if (x.ServiceType != serviceType)
                {
                    return false;
                }

                var key = (serviceType, contract ?? string.Empty);

                return key.Equals(x.OptionalServiceKey) ||
                (contract is null && x.OptionalServiceKey is null) ||
            (x.OptionalServiceKey is string serviceKeyAsString
                       && contract is not null && contract.Equals(serviceKeyAsString, StringComparison.Ordinal));
            });
        }

        /// <inheritdoc />
        public virtual void Register(Func<object?> factory, Type? serviceType, string? contract = null)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var isNull = serviceType is null;
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var key = (serviceType, contract ?? string.Empty);

            _container.UseInstance(
                serviceType,
                isNull ? new NullServiceType(factory) : factory(),
                serviceKey: key);
        }

        /// <inheritdoc />
        public virtual void UnregisterCurrent(Type? serviceType, string? contract = null)
        {
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var hadvalue = _container.GetServiceRegistrations().Any(x =>
            {
                if (x.ServiceType != serviceType)
                {
                    return false;
                }

                var key = (serviceType, contract ?? string.Empty);
                if (key.Equals(x.OptionalServiceKey))
                {
                    _container.Unregister(serviceType, key);
                    return true;
                }

                if (contract is null && x.OptionalServiceKey is null)
                {
                    _container.Unregister(serviceType);
                    return true;
                }

                if (x.OptionalServiceKey is string serviceKeyAsString
                       && contract is not null && contract.Equals(serviceKeyAsString, StringComparison.Ordinal))
                {
                    _container.Unregister(serviceType, contract);
                    return true;
                }

                return false;
            });
        }

        /// <inheritdoc />
        public virtual void UnregisterAll(Type? serviceType, string? contract = null)
        {
            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var key = (serviceType, contract ?? string.Empty);

            _container.Unregister(serviceType, key);
            _container.Unregister(serviceType, contract);
            _container.Unregister(serviceType);
        }

        /// <inheritdoc />
        public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
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
            }
        }
    }
}
