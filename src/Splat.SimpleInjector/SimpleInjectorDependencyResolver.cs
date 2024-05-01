// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>
/// Simple Injector implementation for <see cref="IMutableDependencyResolver"/>.
/// </summary>
/// <seealso cref="Splat.IDependencyResolver" />
public class SimpleInjectorDependencyResolver : IDependencyResolver
{
    private readonly Container _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="initializer">The initializer.</param>
    public SimpleInjectorDependencyResolver(Container container, SimpleInjectorInitializer initializer)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _ = initializer ?? throw new ArgumentNullException(nameof(initializer));

        RegisterFactories(initializer);
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var registration = _container.GetRegistration(serviceType);
            if (registration is not null)
            {
                return registration.GetInstance();
            }

            var registers = _container.GetAllInstances(serviceType);
            return registers.LastOrDefault()!;
        }
        catch
        {
            return default;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return _container.GetAllInstances(serviceType);
        }
        catch
        {
            var registration = _container.GetRegistration(serviceType);
            return registration switch
            {
                not null => new[] { registration.GetInstance() },
                _ => Array.Empty<object>()
            };
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        return _container.GetCurrentRegistrations().Any(x => x.ServiceType == serviceType);
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
        // The function does nothing because there should be no registration called on this object.
        // Anyway, Locator.SetLocator performs some unnecessary registrations.
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract = null) => throw new NotImplementedException();

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract = null) => throw new NotImplementedException();

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException();

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
            _container.Dispose();
        }
    }

    private void RegisterFactories(SimpleInjectorInitializer initializer)
    {
        foreach (var typeFactories in initializer.RegisteredFactories)
        {
            _container.Collection.Register(
                typeFactories.Key,
                typeFactories.Value.Select(n =>
                    new TransientSimpleInjectorRegistration(_container, typeFactories.Key, n)));
        }
    }
}
