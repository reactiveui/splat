// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
    private readonly Dictionary<(Type? type, string callback), List<Action<IDisposable>>> _callbackRegistry = [];

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Field is Disposed using Interlocked method")]
    private IDisposable _inner = toDispose ?? ActionDisposable.Empty;
    private bool _isDisposed;

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract = null) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        return _innerGetServices(serviceType, contract) ?? [];
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        return _innerGetServices(serviceType, contract) is not null;
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
        if (_innerRegister is null)
        {
            throw new NotImplementedException();
        }

        var isNull = serviceType is null;

        serviceType ??= typeof(NullServiceType);

        _innerRegister(
            () =>
            isNull
                ? new NullServiceType(factory)
                : factory(),
            serviceType,
            contract);

        var pair = (serviceType, contract ?? string.Empty);

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
                _callbackRegistry[pair].Remove(c);
            }
        }
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract = null)
    {
        if (_unregisterCurrent is null)
        {
            throw new NotImplementedException();
        }

        serviceType ??= typeof(NullServiceType);

        _unregisterCurrent.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract = null)
    {
        if (_unregisterAll is null)
        {
            throw new NotImplementedException();
        }

        serviceType ??= typeof(NullServiceType);

        _unregisterAll.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        var pair = (serviceType, contract ?? string.Empty);

        if (!_callbackRegistry.TryGetValue(pair, out var value))
        {
            value = [];
            _callbackRegistry[pair] = value;
        }

        value.Add(callback);

        return new ActionDisposable(() => value.Remove(callback));
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
