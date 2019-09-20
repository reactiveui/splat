// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Splat
{
    /// <summary>
    /// A simple dependency resolver which takes Funcs for all its actions.
    /// GetService is always implemented via GetServices().LastOrDefault().
    /// This container is not thread safe.
    /// </summary>
    public class FuncDependencyResolver : IDependencyResolver
    {
        private readonly Func<Type, string, IEnumerable<object>> _innerGetServices;
        private readonly Action<Func<object>, Type, string> _innerRegister;
        private readonly Action<Type, string> _unregisterCurrent;
        private readonly Action<Type, string> _unregisterAll;
        private readonly Dictionary<Tuple<Type, string>, List<Action<IDisposable>>> _callbackRegistry = new Dictionary<Tuple<Type, string>, List<Action<IDisposable>>>();

        private IDisposable _inner;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncDependencyResolver"/> class.
        /// </summary>
        /// <param name="getAllServices">A func which will return all the services contained for the specified service type and contract.</param>
        /// <param name="register">A func which will be called when a service type and contract are registered.</param>
        /// <param name="unregisterCurrent">A func which will unregister the current registered element for a service type and contract.</param>
        /// <param name="unregisterAll">A func which will unregister all the registered elements for a service type and contract.</param>
        /// <param name="toDispose">A optional disposable which is called when this resolver is disposed.</param>
        public FuncDependencyResolver(
            Func<Type, string, IEnumerable<object>> getAllServices,
            Action<Func<object>, Type, string> register = null,
            Action<Type, string> unregisterCurrent = null,
            Action<Type, string> unregisterAll = null,
            IDisposable toDispose = null)
        {
            _innerGetServices = getAllServices;
            _innerRegister = register;
            _unregisterCurrent = unregisterCurrent;
            _unregisterAll = unregisterAll;
            _inner = toDispose ?? ActionDisposable.Empty;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            return (GetServices(serviceType, contract) ?? Enumerable.Empty<object>()).LastOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _innerGetServices(serviceType, contract);
        }

        /// <inheritdoc />
        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return _innerGetServices(serviceType, contract) != null;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "disp is Disposed in callback.")]
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (_innerRegister == null)
            {
                throw new NotImplementedException();
            }

            _innerRegister(factory, serviceType, contract);

            var pair = Tuple.Create(serviceType, contract ?? string.Empty);

            if (_callbackRegistry.ContainsKey(pair))
            {
                List<Action<IDisposable>> toRemove = null;

                foreach (var callback in _callbackRegistry[pair])
                {
                    var disp = new BooleanDisposable();

                    callback(disp);

                    if (disp.IsDisposed)
                    {
                        if (toRemove == null)
                        {
                            toRemove = new List<Action<IDisposable>>();
                        }

                        toRemove.Add(callback);
                    }
                }

                if (toRemove != null)
                {
                    foreach (var c in toRemove)
                    {
                        _callbackRegistry[pair].Remove(c);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            if (_unregisterCurrent == null)
            {
                throw new NotImplementedException();
            }

            _unregisterCurrent.Invoke(serviceType, contract);
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            if (_unregisterAll == null)
            {
                throw new NotImplementedException();
            }

            _unregisterAll.Invoke(serviceType, contract);
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            var pair = Tuple.Create(serviceType, contract ?? string.Empty);

            if (!_callbackRegistry.ContainsKey(pair))
            {
                _callbackRegistry[pair] = new List<Action<IDisposable>>();
            }

            _callbackRegistry[pair].Add(callback);

            return new ActionDisposable(() => _callbackRegistry[pair].Remove(callback));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of all managed memory from this class.
        /// </summary>
        /// <param name="isDisposing">If we are currently disposing managed resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                Interlocked.Exchange(ref _inner, ActionDisposable.Empty).Dispose();
            }

            _isDisposed = true;
        }
    }
}
