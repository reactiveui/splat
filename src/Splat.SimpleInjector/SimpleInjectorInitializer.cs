// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.SimpleInjector;

/// <summary>
/// Initializer for SimpleInjector dependency resolver.
/// </summary>
public class SimpleInjectorInitializer : IDependencyResolver
{
    private readonly object _lockObject = new();

    /// <summary>
    /// Gets dictionary of registered factories.
    /// </summary>
    public Dictionary<Type, List<Func<object?>>> RegisteredFactories { get; }
        = [];

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        lock (_lockObject)
        {
            var fact = RegisteredFactories[serviceType].LastOrDefault();
            return fact?.Invoke()!;
        }
    }

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        lock (_lockObject)
        {
            return RegisteredFactories[serviceType]
                .Select(n => n()!);
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        lock (_lockObject)
        {
            return RegisteredFactories.TryGetValue(serviceType, out var values)
                   && values.Count > 0;
        }
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
        var isNull = serviceType is null;
        serviceType ??= typeof(NullServiceType);

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(serviceType, out var value))
            {
                value = [];
                RegisteredFactories.Add(serviceType, value);
            }

            value.Add(() =>
                isNull
                    ? new NullServiceType(factory)
                    : factory());
        }
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract = null) => throw new NotImplementedException();

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        lock (_lockObject)
        {
            _ = RegisteredFactories.Remove(serviceType);
        }
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool isDisposing)
    {
    }
}
