// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using DryIoc;

namespace Splat.DryIoc;

/// <summary>
/// DryIoc implementation for <see cref="IMutableDependencyResolver"/>.
/// https://bitbucket.org/dadhi/dryioc/wiki/Home.
/// </summary>
/// <seealso cref="IDependencyResolver" />
/// <remarks>
/// Initializes a new instance of the <see cref="DryIocDependencyResolver" /> class.
/// </remarks>
/// <param name="container">The container.</param>
public class DryIocDependencyResolver(IContainer? container = null) : IDependencyResolver
{
    private readonly IContainer _container = container ?? new Container();

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
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
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        return _container.GetServiceRegistrations().Any(x =>
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
        });
    }

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

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
    public virtual void UnregisterCurrent(Type? serviceType, string? contract)
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
    public virtual void UnregisterAll(Type? serviceType, string? contract)
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

    /// <inheritdoc/>
    public object? GetService(Type? serviceType) =>
        GetService(serviceType, null);

    /// <inheritdoc/>
    public T? GetService<T>() =>
        (T?)GetService(typeof(T), null);

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType) =>
        GetServices(serviceType, null);

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>() =>
        GetServices(typeof(T), null).Cast<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>
        GetServices(typeof(T), contract).Cast<T>();

    /// <inheritdoc/>
    public bool HasRegistration(Type? serviceType) =>
        HasRegistration(serviceType, null);

    /// <inheritdoc/>
    public bool HasRegistration<T>() =>
        HasRegistration(typeof(T), null);

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc/>
    public void Register(Func<object?> factory, Type? serviceType) =>
        Register(factory, serviceType, null);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory) =>
        Register(() => factory(), typeof(T), null);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract) =>
        Register(() => factory(), typeof(T), contract);

    /// <inheritdoc/>
    public void UnregisterCurrent(Type? serviceType) =>
        UnregisterCurrent(serviceType, null);

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() =>
        UnregisterCurrent(typeof(T), null);

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc/>
    public void UnregisterAll(Type? serviceType) =>
        UnregisterAll(serviceType, null);

    /// <inheritdoc/>
    public void UnregisterAll<T>() =>
        UnregisterAll(typeof(T), null);

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(serviceType, null, callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), null, callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    void IMutableDependencyResolver.Register<TService, TImplementation>() =>
        _container.RegisterDelegate<TService>(
            _ => new TImplementation(),
            ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);

    /// <inheritdoc/>
    void IMutableDependencyResolver.Register<TService, TImplementation>(string? contract)
    {
        var key = (typeof(TService), contract ?? string.Empty);

        if (HasRegistration<TService>(contract))
        {
            Trace.WriteLine($"Warning: Service {typeof(TService)} already exists with key {contract}, the registration will be replaced.");
        }

        _container.RegisterDelegate<TService>(
            _ => new TImplementation(),
            ifAlreadyRegistered: IfAlreadyRegistered.Replace,
            serviceKey: key);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        _container.RegisterInstance<T>(
            value!,
            ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        var key = (typeof(T), contract ?? string.Empty);

        if (HasRegistration<T>(contract))
        {
            Trace.WriteLine($"Warning: Service {typeof(T)} already exists with key {contract}, the registration will be replaced.");
        }

        _container.RegisterInstance<T>(
            value!,
            ifAlreadyRegistered: IfAlreadyRegistered.Replace,
            serviceKey: key);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        _container.RegisterDelegate<T>(
            _ => valueFactory()!,
            Reuse.Singleton,
            ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var key = (typeof(T), contract ?? string.Empty);

        if (HasRegistration<T>(contract))
        {
            Trace.WriteLine($"Warning: Service {typeof(T)} already exists with key {contract}, the registration will be replaced.");
        }

        _container.RegisterDelegate<T>(
            _ => valueFactory()!,
            Reuse.Singleton,
            ifAlreadyRegistered: IfAlreadyRegistered.Replace,
            serviceKey: key);
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
