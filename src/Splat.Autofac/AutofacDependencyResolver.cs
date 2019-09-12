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
using Autofac.Core.Registration;

#pragma warning disable CS0618 // Obsolete values.

namespace Splat.Autofac
{
    /// <summary>
    /// Autofac implementation for <see cref="IDependencyResolver"/>.
    /// </summary>
    public class AutofacDependencyResolver : IDependencyResolver
    {
        private readonly object _lockObject = new object();
        private IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver" /> class.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        public AutofacDependencyResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        /// <inheritdoc />
        public virtual object GetService(Type serviceType, string contract = null)
        {
            lock (_lockObject)
            {
                try
                {
                    return string.IsNullOrEmpty(contract)
                        ? _componentContext.Resolve(serviceType)
                        : _componentContext.ResolveNamed(contract, serviceType);
                }
                catch (DependencyResolutionException)
                {
                    return null;
                }
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            lock (_lockObject)
            {
                try
                {
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                    object instance = string.IsNullOrEmpty(contract)
                        ? _componentContext.Resolve(enumerableType)
                        : _componentContext.ResolveNamed(contract, enumerableType);
                    return ((IEnumerable)instance).Cast<object>();
                }
                catch (DependencyResolutionException)
                {
                    return null;
                }
            }
        }

        /// <inheritdoc />
        public bool HasRegistration(Type serviceType, string contract = null)
        {
            lock (_lockObject)
            {
                return _componentContext.ComponentRegistry.Registrations.Any(x => GetWhetherServiceRegistrationMatchesSearch(
                    x.Services,
                    serviceType,
                    contract));
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
        public virtual void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            lock (_lockObject)
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

                builder.Update(_componentContext.ComponentRegistry);
            }
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
            lock (_lockObject)
            {
                var registrations = _componentContext.ComponentRegistry.Registrations.ToList();
                var registrationCount = registrations.Count;
                if (registrationCount < 1)
                {
                    return;
                }

                var candidatesForRemoval = new List<IComponentRegistration>(registrationCount);
                var registrationIndex = 0;
                while (registrationIndex < registrationCount)
                {
                    var componentRegistration = registrations[registrationIndex];

                    var isCandidateForRemoval = GetWhetherServiceRegistrationMatchesSearch(
                        componentRegistration.Services,
                        serviceType,
                        contract);
                    if (isCandidateForRemoval)
                    {
                        registrations.RemoveAt(registrationIndex);
                        candidatesForRemoval.Add(componentRegistration);
                        registrationCount--;
                    }
                    else
                    {
                        registrationIndex++;
                    }
                }

                if (candidatesForRemoval.Count == 0)
                {
                    // nothing to remove
                    return;
                }

                if (candidatesForRemoval.Count > 1)
                {
                    // need to re-add some registrations
                    var reAdd = candidatesForRemoval.Take(candidatesForRemoval.Count - 1);
                    registrations.AddRange(reAdd);

                    /*
                    // check for multi service registration
                    // in future might want to just remove a single service from a component
                    // rather than do the whole component.
                    var lastCandidate = candidatesForRemoval.Last();
                    var lastCandidateRegisteredServices = lastCandidate.Services.ToArray();
                    if (lastCandidateRegisteredServices.Length > 1)
                    {
                        //
                        // builder.RegisterType<CallLogger>()
                        //    .AsSelf()
                        //    .As<ILogger>()
                        //    .As<ICallInterceptor>();
                        var survivingServices = lastCandidateRegisteredServices.Where(s => s.GetType() != serviceType);
                        var newRegistration = new ComponentRegistration(
                            lastCandidate.Id,
                            lastCandidate.Activator,
                            lastCandidate.Lifetime,
                            lastCandidate.Sharing,
                            lastCandidate.Ownership,
                            survivingServices,
                            lastCandidate.Metadata);
                        registrations.Add(newRegistration);
                    }
                    */
                }

                RemoveAndRebuild(registrations);
            }
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
            lock (_lockObject)
            {
                // prevent multiple enumerations
                var registrations = _componentContext.ComponentRegistry.Registrations.ToList();
                var registrationCount = registrations.Count;
                if (registrationCount < 1)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(contract))
                {
                    RemoveAndRebuild(
                        registrationCount,
                        registrations,
                        x => x.Services.All(s =>
                        {
                            if (!(s is TypedService typedService))
                            {
                                return false;
                            }

                            return typedService.ServiceType != serviceType || !HasMatchingContract(s, contract);
                        }));
                    return;
                }

                RemoveAndRebuild(
                    registrationCount,
                    registrations,
                    x => x.Services.All(s =>
                    {
                        if (!(s is TypedService typedService))
                        {
                            return false;
                        }

                        return typedService.ServiceType != serviceType;
                    }));
            }
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
            lock (_lockObject)
            {
                if (disposing)
                {
                    _componentContext.ComponentRegistry?.Dispose();
                }
            }
        }

        private static bool GetWhetherServiceRegistrationMatchesSearch(
            IEnumerable<Service> componentRegistrationServices,
            Type serviceType,
            string contract)
        {
            foreach (var componentRegistrationService in componentRegistrationServices)
            {
                if (!(componentRegistrationService is IServiceWithType keyedService))
                {
                    continue;
                }

                if (keyedService.ServiceType != serviceType)
                {
                    continue;
                }

                // right type
                if (string.IsNullOrEmpty(contract))
                {
                    if (!HasNoContract(componentRegistrationService))
                    {
                        continue;
                    }

                    // candidate for removal
                    return true;
                }

                if (!HasMatchingContract(componentRegistrationService, contract))
                {
                    continue;
                }

                // candidate for removal
                return true;
            }

            return false;
        }

        private static bool HasMatchingContract(Service service, string contract)
        {
            if (!(service is KeyedService keyedService))
            {
                return false;
            }

            if (!(keyedService.ServiceKey is string stringServiceKey))
            {
                return false;
            }

            return stringServiceKey.Equals(contract, StringComparison.Ordinal);
        }

        private static bool HasNoContract(Service service)
        {
            return !(service is KeyedService);
        }

        private void RemoveAndRebuild(
            int registrationCount,
            IList<IComponentRegistration> registrations,
            Func<IComponentRegistration, bool> predicate)
        {
            var survivingComponents = registrations.Where(predicate).ToArray();

            if (survivingComponents.Length == registrationCount)
            {
                // not removing anything
                // drop out
                return;
            }

            RemoveAndRebuild(survivingComponents);
        }

        private void RemoveAndRebuild(IEnumerable<IComponentRegistration> survivingComponents)
        {
            var builder = new ContainerBuilder();
            foreach (var c in survivingComponents)
            {
                builder.RegisterComponent(c);
            }

            foreach (var source in _componentContext.ComponentRegistry.Sources)
            {
                builder.RegisterSource(source);
            }

            _componentContext = builder.Build();
        }
    }
}
