// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Ninject;

namespace Splat.Ninject;

/// <summary>
/// Ninject implementation for <see cref="IMutableDependencyResolver"/>.
/// </summary>
/// <seealso cref="IMutableDependencyResolver" />
/// <remarks>
/// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
/// </remarks>
/// <param name="kernel">The kernel.</param>
public class NinjectDependencyResolver(IKernel kernel) : IDependencyResolver
{
    private int _isDisposed;

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
        return GetServices(serviceType).LastOrDefault()!;
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
        return GetServices(serviceType, contract).LastOrDefault()!;
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        try
        {
            // Get all bindings and filter by metadata to avoid implicit self-binding issues
            var matchingBindings = kernel.GetBindings(serviceType)
                .Where(b => IsCorrectMetadata(b.BindingConfiguration.Metadata, null))
                .ToList();

            if (matchingBindings.Count == 0)
            {
                return [];
            }

            // Resolve each binding individually
            var results = new List<object>(matchingBindings.Count);
            foreach (var binding in matchingBindings)
            {
                try
                {
                    var instance = kernel.Get(serviceType, meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch
        {
            return [];
        }
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        try
        {
            // Get all bindings and filter by metadata to avoid implicit self-binding issues
            var matchingBindings = kernel.GetBindings(serviceType)
                .Where(b => IsCorrectMetadata(b.BindingConfiguration.Metadata, contract))
                .ToList();

            if (matchingBindings.Count == 0)
            {
                return [];
            }

            // Resolve each binding individually
            var results = new List<object>(matchingBindings.Count);
            foreach (var binding in matchingBindings)
            {
                try
                {
                    var instance = kernel.Get(serviceType, meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch
        {
            return [];
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        return kernel.CanResolve(serviceType, metadata => IsCorrectMetadata(metadata, null));
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        return kernel.CanResolve(serviceType, metadata => IsCorrectMetadata(metadata, contract));
    }

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        _ = kernel.Bind(serviceType).ToMethod(_ => factory());
    }

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        if (contract is null)
        {
            _ = kernel.Bind(serviceType).ToMethod(_ => factory());
        }
        else
        {
            _ = kernel.Bind(serviceType).ToMethod(_ => factory()).Named(contract);
        }
    }

    /// <inheritdoc />
    public virtual void UnregisterCurrent(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        var bindings = kernel.GetBindings(serviceType).ToArray();

        if (bindings is null || bindings.Length < 1)
        {
            return;
        }

        var matchingBinding = bindings.LastOrDefault(x => IsCorrectMetadata(x.BindingConfiguration.Metadata, null));

        if (matchingBinding is null)
        {
            return;
        }

        kernel.RemoveBinding(matchingBinding);
    }

    /// <inheritdoc />
    public virtual void UnregisterCurrent(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var bindings = kernel.GetBindings(serviceType).ToArray();

        if (bindings is null || bindings.Length < 1)
        {
            return;
        }

        var matchingBinding = bindings.LastOrDefault(x => IsCorrectMetadata(x.BindingConfiguration.Metadata, contract));

        if (matchingBinding is null)
        {
            return;
        }

        kernel.RemoveBinding(matchingBinding);
    }

    /// <inheritdoc />
    public virtual void UnregisterAll(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        var bindings = kernel.GetBindings(serviceType).ToArray();

        if (bindings is null || bindings.Length < 1)
        {
            return;
        }

        var matchingBinding = bindings.Where(x => IsCorrectMetadata(x.BindingConfiguration.Metadata, null)).ToArray();

        if (matchingBinding.Length < 1)
        {
            return;
        }

        foreach (var binding in matchingBinding)
        {
            kernel.RemoveBinding(binding);
        }
    }

    /// <inheritdoc />
    public virtual void UnregisterAll(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var bindings = kernel.GetBindings(serviceType).ToArray();

        if (bindings is null || bindings.Length < 1)
        {
            return;
        }

        var matchingBinding = bindings.Where(x => IsCorrectMetadata(x.BindingConfiguration.Metadata, contract)).ToArray();

        if (matchingBinding.Length < 1)
        {
            return;
        }

        foreach (var binding in matchingBinding)
        {
            kernel.RemoveBinding(binding);
        }
    }

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback is not supported by the NInject framework");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback is not supported by the NInject framework");

    /// <inheritdoc/>
    public T? GetService<T>() =>
        GetServices<T>().LastOrDefault();

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        GetServices<T>(contract).LastOrDefault();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>()
    {
        try
        {
            // Get all bindings and filter by metadata to avoid implicit self-binding issues
            var matchingBindings = kernel.GetBindings(typeof(T))
                .Where(b => IsCorrectMetadata(b.BindingConfiguration.Metadata, null))
                .ToList();

            if (matchingBindings.Count == 0)
            {
                return [];
            }

            // Resolve each binding individually using generic method
            var results = new List<T>(matchingBindings.Count);
            foreach (var binding in matchingBindings)
            {
                try
                {
                    var instance = kernel.Get<T>(meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch
        {
            return [];
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        try
        {
            // Get all bindings and filter by metadata to avoid implicit self-binding issues
            var matchingBindings = kernel.GetBindings(typeof(T))
                .Where(b => IsCorrectMetadata(b.BindingConfiguration.Metadata, contract))
                .ToList();

            if (matchingBindings.Count == 0)
            {
                return [];
            }

            // Resolve each binding individually using generic method
            var results = new List<T>(matchingBindings.Count);
            foreach (var binding in matchingBindings)
            {
                try
                {
                    var instance = kernel.Get<T>(meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch
        {
            return [];
        }
    }

    /// <inheritdoc/>
    public bool HasRegistration<T>() =>
        HasRegistration(typeof(T));

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory) =>
        kernel.Bind<T>().ToMethod(_ => factory()!);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract)
    {
        if (contract is null)
        {
            kernel.Bind<T>().ToMethod(_ => factory()!);
        }
        else
        {
            kernel.Bind<T>().ToMethod(_ => factory()!).Named(contract);
        }
    }

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() =>
        UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc/>
    public void UnregisterAll<T>() =>
        UnregisterAll(typeof(T));

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() =>
        kernel.Bind<TService>().To<TImplementation>();

    /// <inheritdoc/>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (contract is null)
        {
            kernel.Bind<TService>().To<TImplementation>();
        }
        else
        {
            kernel.Bind<TService>().To<TImplementation>().Named(contract);
        }
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        kernel.Bind<T>().ToConstant(value);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        if (contract is null)
        {
            kernel.Bind<T>().ToConstant(value);
        }
        else
        {
            kernel.Bind<T>().ToConstant(value).Named(contract);
        }
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope();
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        if (contract is null)
        {
            kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope();
        }
        else
        {
            kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope().Named(contract);
        }
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
            // Use Interlocked.Exchange for thread-safe disposal flag setting
            var wasDisposed = Interlocked.Exchange(ref _isDisposed, 1);
            if (wasDisposed == 0)
            {
                kernel?.Dispose();
            }
        }
    }

    private static bool IsCorrectMetadata(global::Ninject.Planning.Bindings.IBindingMetadata metadata, string? contract) =>
        (metadata?.Name is null && string.IsNullOrWhiteSpace(contract))
        || (metadata?.Name is not null && metadata.Name.Equals(contract, StringComparison.OrdinalIgnoreCase));
}
