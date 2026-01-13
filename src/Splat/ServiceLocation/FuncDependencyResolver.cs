// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// A lightweight adapter <see cref="IDependencyResolver"/> that delegates all operations to supplied callbacks.
/// </summary>
/// <remarks>
/// <para>
/// This resolver is intentionally minimal: it is primarily useful for bridging to another container or
/// for testing scenarios where the resolver behavior is represented as delegates.
/// </para>
/// <para><strong>Thread safety:</strong> This container is not thread-safe.</para>
/// <para>
/// <strong>Null service types:</strong> When the serviceType is <see langword="null"/>, the resolver
/// substitutes an internal sentinel type and wraps registrations in <see cref="NullServiceType"/> so they can later be
/// unwrapped when resolving with a null service type.
/// </para>
/// <para>
/// <strong>Disposal:</strong> On <see cref="Dispose()"/>, the resolver:
/// <list type="bullet">
/// <item><description>Signals registered callbacks (exceptions suppressed).</description></item>
/// <item><description>Executes disposal actions for lazy singletons (exceptions suppressed).</description></item>
/// <item><description>Enumerates all services from <c>getAllServices(null, null)</c> and disposes any <see cref="IDisposable"/> instances (exceptions suppressed).</description></item>
/// <item><description>Disposes the optional <paramref name="toDispose"/> instance provided at construction.</description></item>
/// </list>
/// </para>
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
    /// <summary>
    /// Stores registration callbacks keyed by (service type, contract).
    /// </summary>
    /// <remarks>
    /// When a service is registered via <c>Register*</c>, callbacks registered for that key are invoked.
    /// A callback may dispose the provided token to indicate it should be removed (one-shot semantics).
    /// </remarks>
    private readonly Dictionary<(Type? type, string? contract), List<Action<IDisposable>>> _callbackRegistry = new(16);

    /// <summary>
    /// Stores deferred disposal actions associated with registrations made by this resolver.
    /// </summary>
    /// <remarks>
    /// Currently used for lazy singletons to dispose the created instance (if any) during <see cref="Dispose()"/>.
    /// Exceptions thrown by these actions are suppressed during disposal to avoid masking other cleanup.
    /// </remarks>
    private readonly List<Action> _disposalActions = new(16);

    /// <summary>
    /// Optional inner disposable provided at construction, disposed when this resolver is disposed.
    /// </summary>
    [SuppressMessage(
        "Usage",
        "CA2213:Disposable fields should be disposed",
        Justification = "Field is disposed via Interlocked.Exchange in Dispose(bool)")]
    private IDisposable _inner = toDispose ?? ActionDisposable.Empty;

    /// <summary>
    /// Tracks whether this resolver has been disposed.
    /// </summary>
    /// <remarks>
    /// Used to prevent double-dispose and to reject registrations after disposal.
    /// </remarks>
    private bool _isDisposed;

    /// <inheritdoc />
    public object? GetService(Type? serviceType) =>
        GetLastOrDefault(GetServices(serviceType));

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>
        GetLastOrDefault(GetServices(serviceType, contract));

    /// <inheritdoc />
    public T? GetService<T>() => (T?)GetService(typeof(T));

    /// <inheritdoc />
    public T? GetService<T>(string? contract) =>
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType) =>
        GetServicesCore(serviceType, string.Empty);

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>
        GetServicesCore(serviceType, contract);

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>() => EnumerateAs<T>(GetServices(typeof(T)));

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>(string? contract) =>
        EnumerateAs<T>(GetServices(typeof(T), contract));

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        return getAllServices(serviceType, string.Empty) is not null;
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;
        return getAllServices(serviceType, contract) is not null;
    }

    /// <inheritdoc />
    public bool HasRegistration<T>() => HasRegistration(typeof(T));

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType) =>
        RegisterCore(factory, serviceType, string.Empty);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract) =>
        RegisterCore(factory, serviceType, contract);

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
        where TImplementation : class, TService, new() =>
        Register(static () => new TImplementation(), typeof(TService));

    /// <inheritdoc />
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() =>
        Register(static () => new TImplementation(), typeof(TService), contract);

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class => Register(() => value, typeof(T));

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class => Register(() => value, typeof(T), contract);

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class =>
        RegisterLazySingletonCore(valueFactory, string.Empty);

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class =>
        RegisterLazySingletonCore(valueFactory, contract);

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        if (unregisterCurrent is null)
        {
            throw new NotImplementedException("UnregisterCurrent is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;
        unregisterCurrent.Invoke(serviceType, null);
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract)
    {
        if (unregisterCurrent is null)
        {
            throw new NotImplementedException("UnregisterCurrent is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;
        unregisterCurrent.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <inheritdoc />
    public void UnregisterCurrent<T>(string? contract) => UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        if (unregisterAll is null)
        {
            throw new NotImplementedException("UnregisterAll is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;
        unregisterAll.Invoke(serviceType, null);
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        if (unregisterAll is null)
        {
            throw new NotImplementedException("UnregisterAll is not implemented in this resolver.");
        }

        serviceType ??= NullServiceType.CachedType;
        unregisterAll.Invoke(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <inheritdoc />
    public void UnregisterAll<T>(string? contract) => UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(serviceType, string.Empty, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);

        var key = (serviceType, contract);

        if (!_callbackRegistry.TryGetValue(key, out var callbacks))
        {
            callbacks = new(4);
            _callbackRegistry[key] = callbacks;
        }

        callbacks.Add(callback);

        var disp = new ActionDisposable(() => callbacks.Remove(callback));

        // Invoke callback once per existing registration to match ModernDependencyResolver behavior.
        // Note: we intentionally enumerate, but do not retain results (avoids extra allocations).
        foreach (var unused in GetServices(serviceType, contract))
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
            // Signal callbacks (exceptions suppressed by design).
            foreach (var kvp in _callbackRegistry)
            {
                var list = kvp.Value;
                for (var i = 0; i < list.Count; i++)
                {
                    try
                    {
                        using var disp = new BooleanDisposable();
                        list[i](disp);
                    }
                    catch
                    {
                        // Suppress exceptions during disposal.
                    }
                }
            }

            _callbackRegistry.Clear();

            // Execute disposal actions for lazy singletons.
            for (var i = 0; i < _disposalActions.Count; i++)
            {
                try
                {
                    _disposalActions[i]();
                }
                catch
                {
                    // Suppress exceptions during disposal.
                }
            }

            _disposalActions.Clear();

            // Dispose all registered services that are IDisposable (exceptions suppressed by design).
            var allServices = getAllServices?.Invoke(null, null);
            if (allServices is not null)
            {
                foreach (var service in allServices)
                {
                    try
                    {
                        (service as IDisposable)?.Dispose();
                    }
                    catch
                    {
                        // Suppress exceptions during disposal.
                    }
                }
            }

            Interlocked.Exchange(ref _inner, ActionDisposable.Empty).Dispose();
        }

        _isDisposed = true;
    }

    /// <summary>
    /// Returns the last element of an <see cref="IEnumerable{T}"/> if present; otherwise returns <see langword="null"/>.
    /// </summary>
    /// <param name="source">The sequence to examine.</param>
    /// <returns>
    /// The last element of <paramref name="source"/> when the sequence is non-empty; otherwise <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This helper avoids LINQ allocations and special-cases common collection shapes.
    /// </remarks>
    private static object? GetLastOrDefault(IEnumerable<object> source)
    {
        ArgumentExceptionHelper.ThrowIfNull(source);

        // Avoid LINQ; special-case common collection shapes.
        if (source is IList<object> list)
        {
            return list.Count == 0 ? null : list[list.Count - 1];
        }

        if (source is IReadOnlyList<object> roList)
        {
            return roList.Count == 0 ? null : roList[roList.Count - 1];
        }

        object? last = null;
        var found = false;

        foreach (var item in source)
        {
            last = item;
            found = true;
        }

        return found ? last : null;
    }

    /// <summary>
    /// Enumerates <paramref name="source"/> and casts each element to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target element type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>An enumerable sequence of <typeparamref name="T"/> values.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// Thrown if an element in <paramref name="source"/> cannot be cast to <typeparamref name="T"/>.
    /// </exception>
    /// <remarks>
    /// This is intentionally implemented as an iterator to avoid materializing intermediate collections.
    /// </remarks>
    private static IEnumerable<T> EnumerateAs<T>(IEnumerable<object> source)
    {
        ArgumentExceptionHelper.ThrowIfNull(source);

        foreach (var o in source)
        {
            yield return (T)o;
        }
    }

    /// <summary>
    /// Resolves all services for the specified type/contract pair, handling null service-type semantics.
    /// </summary>
    /// <param name="serviceType">
    /// The service type to resolve. When <see langword="null"/>, a sentinel type is used and results are unwrapped
    /// from <see cref="NullServiceType"/>.
    /// </param>
    /// <param name="contract">The contract associated with the registration, or <see langword="null"/>.</param>
    /// <returns>
    /// An enumerable of resolved services. Returns an empty array when the backing resolver yields <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="contract"/> is required by downstream callbacks and is unexpectedly null. (This method
    /// itself does not enforce non-null; it passes the value through.)
    /// </exception>
    /// <remarks>
    /// <para>
    /// When <paramref name="serviceType"/> is <see langword="null"/>, registrations are represented by
    /// <see cref="NullServiceType"/> instances and must be unwrapped by invoking <see cref="NullServiceType.Factory"/>.
    /// </para>
    /// <para>
    /// When unwrapping is required, the results are materialized into a <see cref="List{T}"/> to avoid multiple
    /// enumeration surprises and to preserve the observable semantics of returning concrete objects.
    /// </para>
    /// </remarks>
    private IEnumerable<object> GetServicesCore(Type? serviceType, string? contract)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        var services = getAllServices(serviceType, contract);
        if (services is null)
        {
            return [];
        }

        // Fast path: no unwrapping required.
        if (!isNull)
        {
            return services;
        }

        // Unwrap NullServiceType instances if we queried for null type.
        // We materialize to avoid multiple enumeration surprises and to preserve existing semantics.
        if (services is ICollection<object> c)
        {
            var list = new List<object>(c.Count);
            foreach (var s in services)
            {
                list.Add(s is NullServiceType nst ? nst.Factory()! : s);
            }

            return list;
        }
        else
        {
            var list = new List<object>();
            foreach (var s in services)
            {
                list.Add(s is NullServiceType nst ? nst.Factory()! : s);
            }

            return list;
        }
    }

    /// <summary>
    /// Registers a factory for a service type/contract pair and invokes any registration callbacks.
    /// </summary>
    /// <param name="factory">The factory used to produce service instances.</param>
    /// <param name="serviceType">
    /// The service type to register. When <see langword="null"/>, the registration is wrapped as <see cref="NullServiceType"/>
    /// against a sentinel type to support later null-type resolution.
    /// </param>
    /// <param name="contract">The contract associated with the registration, or <see langword="null"/>.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this resolver has been disposed.</exception>
    /// <exception cref="NotImplementedException">Thrown if the <c>register</c> delegate was not supplied.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Registration callbacks are invoked after the underlying registration is performed. A callback may dispose the
    /// provided token to indicate it should be removed from future notifications.
    /// </remarks>
    private void RegisterCore(Func<object?> factory, Type? serviceType, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(_isDisposed, this);

        if (register is null)
        {
            throw new NotImplementedException("Register is not implemented in this resolver.");
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        register(
            () => isNull ? new NullServiceType(factory) : factory(),
            serviceType,
            contract);

        var key = (serviceType, contract);

        if (!_callbackRegistry.TryGetValue(key, out var callbacks) || callbacks.Count == 0)
        {
            return;
        }

        // Invoke callbacks; remove any that dispose the provided token (one-shot semantics).
        for (var i = 0; i < callbacks.Count; i++)
        {
            using var disp = new BooleanDisposable();
            callbacks[i](disp);

            if (disp.IsDisposed)
            {
                callbacks.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Registers a lazy singleton for a service type/contract pair and tracks its disposal if created.
    /// </summary>
    /// <typeparam name="T">
    /// The service type. The type parameter is annotated to support trimming scenarios requiring a public parameterless constructor.
    /// </typeparam>
    /// <param name="valueFactory">Factory used to create the singleton instance on first access.</param>
    /// <param name="contract">The contract associated with the registration, or <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if this resolver has been disposed.</exception>
    /// <exception cref="NotImplementedException">Thrown if the <c>register</c> delegate was not supplied.</exception>
    /// <remarks>
    /// <para>
    /// The singleton is created via <see cref="Lazy{T}"/> using <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
    /// </para>
    /// <para>
    /// During resolver disposal, if the lazy value was created and implements <see cref="IDisposable"/>, it is disposed.
    /// </para>
    /// </remarks>
    private void RegisterLazySingletonCore<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Store disposal action that can safely dispose the lazy value.
        _disposalActions.Add(() =>
        {
            if (lazy.IsValueCreated && lazy.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        });

        // Wrap lazy value access to dispose and throw if resolver was disposed during construction.
        Register(
            () =>
            {
                var value = lazy.Value;
                if (Volatile.Read(ref _isDisposed))
                {
                    (value as IDisposable)?.Dispose();
                    ObjectDisposedExceptionHelper.ThrowIf(true, this);
                }

                return value;
            },
            typeof(T),
            contract);
    }
}
