// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Linq.Expressions;
using DryIoc;

namespace Splat.DryIoc;

/// <summary>
/// DryIoc implementation for <see cref="IMutableDependencyResolver"/>.
/// https://bitbucket.org/dadhi/dryioc/wiki/Home.
/// </summary>
/// <seealso cref="Splat.IDependencyResolver" />
/// <remarks>
/// Initializes a new instance of the <see cref="DryIocDependencyResolver" /> class.
/// </remarks>
/// <param name="container">The container.</param>
public class DryIocDependencyResolver(IContainer? container = null) : IDependencyResolver
{
    private readonly IContainer _container = container ?? new Container();

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract = null) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        var key = (serviceType, contract ?? string.Empty);
        var registeredinSplat = _container.ResolveMany(serviceType, behavior: ResolveManyBehavior.AsFixedArray, serviceKey: key);
        if (registeredinSplat.Any())
        {
            return registeredinSplat;
        }

        var registeredWithContract = _container.ResolveMany(serviceType, behavior: ResolveManyBehavior.AsFixedArray, serviceKey: contract);
        return registeredWithContract.Any()
            ? registeredWithContract
            : _container.ResolveMany(serviceType, behavior: ResolveManyBehavior.AsFixedArray);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract = null) =>
        serviceType switch
        {
            null => throw new ArgumentNullException(nameof(serviceType)),
            _ => _container.GetServiceRegistrations().Any(x =>
            {
                if (x.ServiceType != serviceType)
                {
                    return false;
                }

                if (contract is null)
                {
                    return x.OptionalServiceKey is null;
                }

                var key = (serviceType, contract ?? string.Empty);

                return key.Equals(x.OptionalServiceKey) ||
                (contract is null && x.OptionalServiceKey is null) ||
                (x.OptionalServiceKey is string serviceKeyAsString
                 && contract?.Equals(serviceKeyAsString, StringComparison.Ordinal) == true);
            })
        };

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
#pragma warning disable RCS1256 // Invalid argument null check.
#if NETSTANDARD || NETFRAMEWORK
        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (serviceType is null)
        {
            throw new ArgumentNullException(nameof(serviceType));
        }
#else
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(serviceType);
#endif
#pragma warning restore RCS1256 // Invalid argument null check.

        if (string.IsNullOrEmpty(contract))
        {
            _container.RegisterDelegate(
                serviceType,
                _ => CreateThenConvert(serviceType!, factory),
                ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);

            return;
        }

        var key = (serviceType, contract);

        if (HasRegistration(serviceType, contract))
        {
            Trace.WriteLine($"Warning: Service {serviceType} already exists with key {contract}, the registration will be replaced.");
        }

        // Keyed instances can only have a single instance so keep latest
        _container.RegisterDelegate(
            serviceType,
            _ => CreateThenConvert(serviceType!, factory),
            ifAlreadyRegistered: IfAlreadyRegistered.Replace,
            serviceKey: key);
    }

    /// <inheritdoc />
    public virtual void UnregisterCurrent(Type? serviceType, string? contract = null)
    {
        var key = (serviceType, contract ?? string.Empty);
        var hadvalue = _container.GetServiceRegistrations().Any(x =>
        {
            if (x.ServiceType != serviceType)
            {
                return false;
            }

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
                   && contract?.Equals(serviceKeyAsString, StringComparison.Ordinal) == true)
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
        var key = (serviceType, contract ?? string.Empty);
        foreach (var x in _container.GetServiceRegistrations())
        {
            if (x.ServiceType != serviceType)
            {
                continue;
            }

            if (key.Equals(x.OptionalServiceKey))
            {
                _container.Unregister(serviceType, key);
                continue;
            }

            if (contract is null && x.OptionalServiceKey is null)
            {
                _container.Unregister(serviceType);
                continue;
            }

            if (x.OptionalServiceKey is string serviceKeyAsString
                   && contract?.Equals(serviceKeyAsString, StringComparison.Ordinal) == true)
            {
                _container.Unregister(serviceType, contract);
            }
        }
    }

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException();

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

    private static object? CreateThenConvert(Type serviceType, Func<object?> factory)
    {
        // we need to cast because we pass an object back and dryioc wants it explicitly cast.
        // alternative (happy to be proven wrong) is to break the interface and add a Register<T>(...) method?
        var instance = factory();

        return instance != null ? Cast(serviceType, instance) : null;
    }

    private static object? Cast(Type type, object data)
    {
        // based upon https://stackoverflow.com/a/27584212
        var dataParam = Expression.Parameter(typeof(object), "data");
        var body = Expression.Block(Expression.Convert(Expression.Convert(dataParam, data.GetType()), type));

        var run = Expression.Lambda(body, dataParam).Compile();
        return run.DynamicInvoke(data);
    }
}
