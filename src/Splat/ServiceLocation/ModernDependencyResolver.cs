// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Splat
{
    /// <summary>
    /// <para>
    /// This class is a dependency resolver written for modern C# 5.0 times.
    /// It implements all registrations via a Factory method. With the power
    /// of Closures, you can actually implement most lifetime styles (i.e.
    /// construct per call, lazy construct, singleton) using this.
    /// </para>
    /// <para>
    /// Unless you have a very compelling reason not to, this is the only class
    /// you need in order to do dependency resolution, don't bother with using
    /// a full IoC container.
    /// </para>
    /// <para>This container is not thread safe.</para>
    /// </summary>
    public class ModernDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>> _callbackRegistry;
        private Dictionary<(Type serviceType, string? contract), List<Func<object?>>>? _registry;

        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernDependencyResolver"/> class.
        /// </summary>
        public ModernDependencyResolver()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernDependencyResolver"/> class.
        /// </summary>
        /// <param name="registry">A registry of services.</param>
        protected ModernDependencyResolver(Dictionary<(Type serviceType, string? contract), List<Func<object?>>>? registry)
        {
            _registry = registry is not null ?
                registry.ToDictionary(k => k.Key, v => v.Value.ToList()) :
                new Dictionary<(Type serviceType, string? contract), List<Func<object?>>>();

            _callbackRegistry = new Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>>();
        }

        /// <inheritdoc />
        public bool HasRegistration(Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return false;
            }

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);
            return _registry.TryGetValue(pair, out var registrations) && registrations.Count > 0;
        }

        /// <inheritdoc />
        public void Register(Func<object?> factory, Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return;
            }

            var isNull = serviceType is null;

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);

            if (!_registry.ContainsKey(pair))
            {
                _registry[pair] = new List<Func<object?>>();
            }

            _registry[pair].Add(() =>
                isNull
                    ? new NullServiceType(factory)
                    : factory());

            if (_callbackRegistry.ContainsKey(pair))
            {
                List<Action<IDisposable>>? toRemove = null;

                foreach (var callback in _callbackRegistry[pair])
                {
                    var disp = new BooleanDisposable();

                    callback(disp);

                    if (disp.IsDisposed)
                    {
                        (toRemove ??= new List<Action<IDisposable>>()).Add(callback);
                    }
                }

                if (toRemove is not null)
                {
                    foreach (var c in toRemove)
                    {
                        _callbackRegistry[pair].Remove(c);
                    }
                }
            }
        }

        /// <inheritdoc />
        public object? GetService(Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return default;
            }

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);
            if (!_registry.ContainsKey(pair))
            {
                return default;
            }

            var ret = _registry[pair].LastOrDefault();
            object? returnValue = default;
            if (ret != null)
            {
                returnValue = ret();
                if (returnValue is NullServiceType nullServiceType)
                {
                    return nullServiceType.Factory()!;
                }
            }

            return returnValue;
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return Array.Empty<object>();
            }

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);
            if (!_registry.ContainsKey(pair))
            {
                return Array.Empty<object>();
            }

            return _registry[pair].ConvertAll(x => x()!);
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return;
            }

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);

            if (!_registry.TryGetValue(pair, out var list))
            {
                return;
            }

            var position = list.Count - 1;
            if (position < 0)
            {
                return;
            }

            list.RemoveAt(position);
        }

        /// <inheritdoc />
        public void UnregisterAll(Type? serviceType, string? contract = null)
        {
            if (_registry is null)
            {
                return;
            }

            if (serviceType is null)
            {
                serviceType = typeof(NullServiceType);
            }

            var pair = GetKey(serviceType, contract);

            _registry[pair] = new List<Func<object?>>();
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (_registry is null)
            {
                return new ActionDisposable(() => { });
            }

            var pair = GetKey(serviceType, contract);

            if (!_callbackRegistry.ContainsKey(pair))
            {
                _callbackRegistry[pair] = new List<Action<IDisposable>>();
            }

            _callbackRegistry[pair].Add(callback);

            var disp = new ActionDisposable(() => _callbackRegistry[pair].Remove(callback));

            if (_registry.ContainsKey(pair))
            {
                foreach (var s in _registry[pair])
                {
                    callback(disp);
                }
            }

            return disp;
        }

        /// <summary>
        /// Generates a duplicate of the resolver with all the current registrations.
        /// Useful if you want to generate temporary resolver using the <see cref="DependencyResolverMixins.WithResolver(IDependencyResolver, bool)"/> method.
        /// </summary>
        /// <returns>The newly generated <see cref="ModernDependencyResolver"/> class with the current registrations.</returns>
        public ModernDependencyResolver Duplicate() => new(_registry);

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
                _registry = null;
            }

            _isDisposed = true;
        }

        private static (Type type, string contract) GetKey(
            Type? serviceType,
            string? contract = null) =>
            (serviceType!, contract ?? string.Empty);
    }
}
