// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A simple dependency resolver which takes Funcs for all its actions.
/// GetService is always implemented via GetServices().LastOrDefault().
/// This container is not thread safe.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FuncDependencyResolver"/> class.
/// </remarks>
/// <param name="getAllServices">A func which will return all the services contained for the specified service type and contract.</param>
/// <param name="register">A func which will be called when a service type and contract are registered.</param>
/// <param name="unregisterCurrent">A func which will unregister the current registered element for a service type and contract.</param>
/// <param name="unregisterAll">A func which will unregister all the registered elements for a service type and contract.</param>
/// <param name="toDispose">A optional disposable which is called when this resolver is disposed.</param>
public class FuncDependencyResolver(
    Func<Type?, string?, IEnumerable<object>> getAllServices,
    Action<Func<object?>, Type?, string?>? register = null,
    Action<Type?, string?>? unregisterCurrent = null,
    Action<Type?, string?>? unregisterAll = null,
    IDisposable? toDispose = null) : IDependencyResolver
{
    private readonly Func<Type?, string?, IEnumerable<object>> _innerGetServices = getAllServices;
    private readonly Action<Func<object?>, Type?, string?>? _innerRegister = register;
    private readonly Action<Type?, string?>? _unregisterCurrent = unregisterCurrent;
    private readonly Action<Type?, string?>? _unregisterAll = unregisterAll;
    private readonly Dictionary<(Type? type, string? callback), List<Action<IDisposable>>> _callbackRegistry = [];

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Field is Disposed using Interlocked method")]
    private IDisposable _inner = toDispose ?? ActionDisposable.Empty;
    private bool _isDisposed;

    /// <inheritdoc />
    public object? GetService(Type? serviceType) =>
        GetServices(serviceType).LastOrDefault();

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <inheritdoc />
    public T? GetService<T>() => (T?)GetService(typeof(T));

    /// <inheritdoc />
    public T? GetService<T>(string? contract) =>
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        return _innerGetServices(serviceType, string.Empty) ?? [];
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        return _innerGetServices(serviceType, contract) ?? [];
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>() => GetServices(typeof(T)).Cast<T>();

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>(string? contract) =>
        GetServices(typeof(T), contract).Cast<T>();

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        return _innerGetServices(serviceType, string.Empty) is not null;
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        return _innerGetServices(serviceType, contract) is not null;
    }

    /// <inheritdoc />
    public bool HasRegistration<T>() => HasRegistration(typeof(T));

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        if (_innerRegister is null)
        {
            throw new NotImplementedException("Register is not implemented in this resolver.");
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        _innerRegister(
            () =>
            isNull
                ? new NullServiceType(factory)
                : factory(),
            serviceType,
            string.Empty);

        var pair = (serviceType, string.Empty);

        if (!_callbackRegistry.TryGetValue(pair, out var callbackList))
        {
            return;
        }

        List<Action<IDisposable>>? toRemove = null;

        foreach (var callback in callbackList)
        {
            using var disp = new BooleanDisposable();

            callback(disp);

            if (disp.IsDisposed)
            {
                (toRemove ??= []).Add(callback);
            }
        }

        if (toRemove is not null)
        {
            foreach (var c in toRemove)
            {
                _ = _callbackRegistry[pair].Remove(c);
            }
        }
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (_innerRegister is null)
        {
            throw new NotImplementedException("Register is not implemented in this resolver.");
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        _innerRegister(
            () =>
            isNull
                ? new NullServiceType(factory)
                : factory(),
            serviceType,
            contract);

        var pair = (serviceType, contract);

        if (!_callbackRegistry.TryGetValue(pair, out var callbackList))
        {
            return;
        }

        List<Action<IDisposable>>? toRemove = null;

        foreach (var callback in callbackList)
        {
            using var disp = new BooleanDisposable();

            callback(disp);

            if (disp.IsDisposed)
            {
                (toRemove ??= []).Add(callback);
            }
        }

        if (toRemove is not null)
        {
            foreach (var c in toRemove)
            {
                _ = _callbackRegistry[pair].Remove(c);
            }
        }
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        Register(() => factory(), typeof(T));
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        Register(() => factory(), typeof(T), contract);
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() => Register(() => new TImplementation(), typeof(TService));

    /// <inheritdoc />
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() => Register(() => new TImplementation(), typeof(TService), contract);

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class => Register(() => value, typeof(T));

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class => Register(() => value, typeof(T), contract);

    /// <inheritdoc />
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        Register(() => lazy.Value, typeof(T));
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        Register(() => lazy.Value, typeof(T), contract);
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        if (_unregisterCurrent is null)
        {
            throw new NotImplementedException("UnregisterCurrent is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;

        _unregisterCurrent.Invoke(serviceType, null);
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract)
    {
        if (_unregisterCurrent is null)
        {
            throw new NotImplementedException("UnregisterCurrent is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;

        _unregisterCurrent.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <inheritdoc />
    public void UnregisterCurrent<T>(string? contract) => UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        if (_unregisterAll is null)
        {
            throw new NotImplementedException("UnregisterAll is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;

        _unregisterAll.Invoke(serviceType, null);
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        if (_unregisterAll is null)
        {
            throw new NotImplementedException("UnregisterAll is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;

        _unregisterAll.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <inheritdoc />
    public void UnregisterAll<T>(string? contract) => UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);

        var pair = (serviceType, string.Empty);

        if (!_callbackRegistry.TryGetValue(pair, out var value))
        {
            value = [];
            _callbackRegistry[pair] = value;
        }

        value.Add(callback);

        var disp = new ActionDisposable(() => value.Remove(callback));

        // Invoke callback once per existing registration to match ModernDependencyResolver behavior
        var existingServices = GetServices(serviceType);
        foreach (var service in existingServices)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);

        var pair = (serviceType, contract);

        if (!_callbackRegistry.TryGetValue(pair, out var value))
        {
            value = [];
            _callbackRegistry[pair] = value;
        }

        value.Add(callback);

        var disp = new ActionDisposable(() => value.Remove(callback));

        // Invoke callback once per existing registration to match ModernDependencyResolver behavior
        var existingServices = GetServices(serviceType, contract);
        foreach (var service in existingServices)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        return ServiceRegistrationCallback(typeof(T), callback);
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        return ServiceRegistrationCallback(typeof(T), contract, callback);
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
            // Dispose of all IDisposable callbacks
            foreach (var pair in _callbackRegistry)
            {
                foreach (var callback in pair.Value)
                {
                    using var disp = new BooleanDisposable();
                    callback(disp);
                }
            }

            _callbackRegistry.Clear();

            // Dispose all registered services that are IDisposable
            if (_innerGetServices != null)
            {
                var allServices = _innerGetServices(null, null);
                if (allServices != null)
                {
                    foreach (var service in allServices)
                    {
                        if (service is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }

            Interlocked.Exchange(ref _inner, ActionDisposable.Empty).Dispose();
        }

        _isDisposed = true;
    }
}
