// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Ninject;

namespace Splat.Ninject;

/// <summary>
/// Provides an implementation of the IDependencyResolver interface using the Ninject IoC container. Enables
/// registration, resolution, and management of service dependencies within an application.
/// </summary>
/// <remarks>This resolver integrates Ninject's binding and resolution capabilities with a standard dependency
/// resolver interface, allowing for flexible service registration and retrieval. Thread safety is ensured for disposal
/// operations, but individual service factories should be thread-safe if used in multi-threaded scenarios. Disposing
/// the resolver will also dispose the underlying Ninject kernel, releasing all managed resources.</remarks>
/// <param name="kernel">The Ninject kernel instance used to manage service bindings and resolve dependencies. Cannot be null.</param>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "These are dependency-resolution and registration APIs whose generic type parameter is the caller-supplied service type, which by contract cannot appear in the parameter list.")]
public class NinjectDependencyResolver(IKernel kernel) : IDependencyResolver
{
    /// <summary>Non-zero once disposed; used for a thread-safe single-dispose guard via <see cref="System.Threading.Interlocked"/>.</summary>
    private int _isDisposed;

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
        return Last(GetServices(serviceType))!;
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
        return Last(GetServices(serviceType, contract))!;
    }

    /// <inheritdoc/>
    public T? GetService<T>() =>
        Last(GetServices<T>());

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        Last(GetServices<T>(contract));

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        try
        {
            // Filter the bindings by metadata to avoid implicit self-binding issues, resolving each one individually
            var results = new List<object>();
            foreach (var binding in kernel.GetBindings(serviceType))
            {
                if (!IsCorrectMetadata(binding.BindingConfiguration.Metadata, null))
                {
                    continue;
                }

                try
                {
                    var instance = kernel.Get(serviceType, meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch (ActivationException)
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch (ActivationException)
        {
            return [];
        }
    }

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        try
        {
            // Filter the bindings by metadata to avoid implicit self-binding issues, resolving each one individually
            var results = new List<object>();
            foreach (var binding in kernel.GetBindings(serviceType))
            {
                if (!IsCorrectMetadata(binding.BindingConfiguration.Metadata, contract))
                {
                    continue;
                }

                try
                {
                    var instance = kernel.Get(serviceType, meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch (ActivationException)
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch (ActivationException)
        {
            return [];
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>()
    {
        try
        {
            // Filter the bindings by metadata to avoid implicit self-binding issues, resolving each one individually using the generic method
            var results = new List<T>();
            foreach (var binding in kernel.GetBindings(typeof(T)))
            {
                if (!IsCorrectMetadata(binding.BindingConfiguration.Metadata, null))
                {
                    continue;
                }

                try
                {
                    var instance = kernel.Get<T>(meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch (ActivationException)
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch (ActivationException)
        {
            return [];
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        try
        {
            // Filter the bindings by metadata to avoid implicit self-binding issues, resolving each one individually using the generic method
            var results = new List<T>();
            foreach (var binding in kernel.GetBindings(typeof(T)))
            {
                if (!IsCorrectMetadata(binding.BindingConfiguration.Metadata, contract))
                {
                    continue;
                }

                try
                {
                    var instance = kernel.Get<T>(meta => meta.Equals(binding.BindingConfiguration.Metadata));
                    if (instance is not null)
                    {
                        results.Add(instance);
                    }
                }
                catch (ActivationException)
                {
                    // Skip bindings that can't be resolved
                }
            }

            return results;
        }
        catch (ActivationException)
        {
            return [];
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        return kernel.CanResolve(serviceType, static metadata => IsCorrectMetadata(metadata, null));
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        return kernel.CanResolve(serviceType, metadata => IsCorrectMetadata(metadata, contract));
    }

    /// <inheritdoc/>
    public bool HasRegistration<T>() =>
        HasRegistration(typeof(T));

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

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

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory) =>
        kernel.Bind<T>().ToMethod(_ => factory()!);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract)
    {
        if (contract is null)
        {
            _ = kernel.Bind<T>().ToMethod(_ => factory()!);
        }
        else
        {
            _ = kernel.Bind<T>().ToMethod(_ => factory()!).Named(contract);
        }
    }

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
            _ = kernel.Bind<TService>().To<TImplementation>();
        }
        else
        {
            _ = kernel.Bind<TService>().To<TImplementation>().Named(contract);
        }
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        _ = kernel.Bind<T>().ToConstant(value);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        if (contract is null)
        {
            _ = kernel.Bind<T>().ToConstant(value);
        }
        else
        {
            _ = kernel.Bind<T>().ToConstant(value).Named(contract);
        }
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        _ = kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope();
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        if (contract is null)
        {
            _ = kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope();
        }
        else
        {
            _ = kernel.Bind<T>().ToMethod(_ => valueFactory()!).InSingletonScope().Named(contract);
        }
    }

    /// <inheritdoc />
    public virtual void UnregisterCurrent(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        global::Ninject.Planning.Bindings.IBinding? matchingBinding = null;
        foreach (var binding in kernel.GetBindings(serviceType))
        {
            if (IsCorrectMetadata(binding.BindingConfiguration.Metadata, null))
            {
                matchingBinding = binding;
            }
        }

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

        global::Ninject.Planning.Bindings.IBinding? matchingBinding = null;
        foreach (var binding in kernel.GetBindings(serviceType))
        {
            if (IsCorrectMetadata(binding.BindingConfiguration.Metadata, contract))
            {
                matchingBinding = binding;
            }
        }

        if (matchingBinding is null)
        {
            return;
        }

        kernel.RemoveBinding(matchingBinding);
    }

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() =>
        UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc />
    public virtual void UnregisterAll(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        var matchingBindings = new List<global::Ninject.Planning.Bindings.IBinding>();
        foreach (var binding in kernel.GetBindings(serviceType))
        {
            if (IsCorrectMetadata(binding.BindingConfiguration.Metadata, null))
            {
                matchingBindings.Add(binding);
            }
        }

        foreach (var binding in matchingBindings)
        {
            kernel.RemoveBinding(binding);
        }
    }

    /// <inheritdoc />
    public virtual void UnregisterAll(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var matchingBindings = new List<global::Ninject.Planning.Bindings.IBinding>();
        foreach (var binding in kernel.GetBindings(serviceType))
        {
            if (IsCorrectMetadata(binding.BindingConfiguration.Metadata, contract))
            {
                matchingBindings.Add(binding);
            }
        }

        foreach (var binding in matchingBindings)
        {
            kernel.RemoveBinding(binding);
        }
    }

    /// <inheritdoc/>
    public void UnregisterAll<T>() =>
        UnregisterAll(typeof(T));

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotSupportedException("ServiceRegistrationCallback is not supported by the NInject framework");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotSupportedException("ServiceRegistrationCallback is not supported by the NInject framework");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Disposes of the instance.</summary>
    /// <param name="disposing">Whether or not the instance is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        // Use Interlocked.Exchange for thread-safe disposal flag setting
        var wasDisposed = Interlocked.Exchange(ref _isDisposed, 1);
        if (wasDisposed != 0)
        {
            return;
        }

        kernel?.Dispose();
    }

    /// <summary>Determines whether the supplied binding metadata matches the requested contract.</summary>
    /// <param name="metadata">The binding metadata to inspect.</param>
    /// <param name="contract">The contract name to match, or <see langword="null"/> for the default contract.</param>
    /// <returns><see langword="true"/> if the metadata matches the contract; otherwise, <see langword="false"/>.</returns>
    private static bool IsCorrectMetadata(global::Ninject.Planning.Bindings.IBindingMetadata metadata, string? contract) =>
        (metadata?.Name is null && string.IsNullOrWhiteSpace(contract))
        || (metadata?.Name is not null && metadata.Name.Equals(contract, StringComparison.OrdinalIgnoreCase));

    /// <summary>Returns the last registration in the sequence, which is the one that wins.</summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="services">The resolved services, in registration order.</param>
    /// <returns>The last service, or the default when the sequence is empty.</returns>
    private static T? Last<T>(IEnumerable<T> services)
    {
        var last = default(T);
        foreach (var service in services)
        {
            last = service;
        }

        return last;
    }
}
