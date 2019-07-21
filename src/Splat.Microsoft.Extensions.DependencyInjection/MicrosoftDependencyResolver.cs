// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Microsoft DI implementation for <see cref="IMutableDependencyResolver"/>.
    /// </summary>
    /// <seealso cref="IDependencyResolver" />
    /// <seealso cref="IMicrosoftDependencyResolver"/>
    public class MicrosoftDependencyResolver : IMicrosoftDependencyResolver
    {
        private const string LockedExceptionMessage = "The provider has already been set.";

        private readonly IServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;
        private bool _locked;

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
            UpdateContainer(serviceProvider);
        }

        private IServiceProvider ServiceProvider
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
            ServiceProvider.GetRequiredService(serviceType);

        /// <inheritdoc />
        public virtual IEnumerable<object> GetServices(Type serviceType, string contract = null) =>
          ServiceProvider.GetServices(serviceType);

        /// <inheritdoc />
        public virtual void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (_locked)
            {
                throw new InvalidOperationException(LockedExceptionMessage);
            }

            _serviceCollection.AddSingleton(serviceType, (provider) => factory());
        }

        /// <inheritdoc />
        public virtual void UnregisterCurrent(Type serviceType, string contract = null) =>
            UnregisterAll(serviceType);

        /// <inheritdoc />
        public virtual void UnregisterAll(Type serviceType, string contract = null)
        {
            if (_locked)
            {
                throw new InvalidOperationException(LockedExceptionMessage);
            }

            var remove = _serviceCollection
                .Where(sd => sd.ServiceType == serviceType)
                .ToList();

            foreach (var item in remove)
            {
                _serviceCollection.Remove(item);
            }
        }

        /// <inheritdoc />
        public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void UpdateContainer(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
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
    }
}
