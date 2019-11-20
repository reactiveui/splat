// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Prism.Ioc;

namespace Splat.Prism
{
    /// <summary>
    /// A container for the Prism application.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1316:Tuple element names should use correct casing", Justification = "Match Prism naming scheme.")]
    public class SplatContainerExtension : IContainerExtension<IDependencyResolver>, IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplatContainerExtension"/> class.
        /// </summary>
        public SplatContainerExtension()
        {
            Locator.SetLocator(Instance);
        }

        /// <summary>
        /// Gets the dependency resolver.
        /// </summary>
        public IDependencyResolver Instance { get; } = new ModernDependencyResolver();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void FinalizeExtension()
        {
            Locator.SetLocator(new ModernDependencyResolver());
        }

        public object GetService(Type serviceType, string contract = null)
        {
            return Instance.GetService(serviceType, contract);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return Instance.GetServices(serviceType, contract);
        }

        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return Instance.HasRegistration(serviceType, contract);
        }

        /// <inheritdoc/>
        public bool IsRegistered(Type type)
        {
            return Instance.HasRegistration(type);
        }

        /// <inheritdoc/>
        public bool IsRegistered(Type type, string name)
        {
            return Instance.HasRegistration(type, name);
        }

        /// <inheritdoc/>
        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(() => Activator.CreateInstance(to), from);
            return this;
        }

        /// <inheritdoc/>
        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(() => Activator.CreateInstance(to), from, name);
            return this;
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            Instance.Register(factory, serviceType, contract);
        }

        /// <inheritdoc/>
        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterConstant(instance, type);
            return this;
        }

        /// <inheritdoc/>
        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterConstant(instance, type, name);
            return this;
        }

        /// <inheritdoc/>
        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.RegisterLazySingleton(() => Activator.CreateInstance(to), from);
            return this;
        }

        /// <inheritdoc/>
        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.RegisterLazySingleton(() => Activator.CreateInstance(to), from, name);
            return this;
        }

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            return Instance.GetService(type);
        }

        /// <inheritdoc/>
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object Resolve(Type type, string name)
        {
            return Instance.GetService(type, name);
        }

        /// <inheritdoc/>
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            throw new NotImplementedException();
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            return Instance.ServiceRegistrationCallback(serviceType, contract, callback);
        }

        public void UnregisterAll(Type serviceType, string contract = null)
        {
            Instance.UnregisterAll(serviceType, contract);
        }

        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            Instance.UnregisterCurrent(serviceType, contract);
        }
    }
}
