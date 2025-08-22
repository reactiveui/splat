// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Builder;

/// <summary>
/// A builder class for configuring ReactiveUI without using reflection.
/// This provides an AOT-compatible alternative to the reflection-based InitializeReactiveUI method.
/// </summary>
public class AppBuilder
{
    private readonly IMutableDependencyResolver _resolver;
    private readonly List<Action<IMutableDependencyResolver>> _registrations = [];
    private Func<IMutableDependencyResolver> _resolverProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBuilder"/> class.
    /// </summary>
    /// <param name="resolver">The dependency resolver to configure.</param>
    public AppBuilder(IMutableDependencyResolver resolver)
    {
        UsingBuilder = true;
        _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        _resolverProvider = () => _resolver;
    }

    /// <summary>
    /// Gets a value indicating whether this instance has been built.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has been built; otherwise, <c>false</c>.
    /// </value>
    public static bool HasBeenBuilt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether [using builder].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [using builder]; otherwise, <c>false</c>.
    /// </value>
    public static bool UsingBuilder { get; private set; }

    /// <summary>
    /// Creates a splat builder with the Splat Locator instance with the current mutable resolver.
    /// </summary>
    /// <returns>The builder instance for chaining.</returns>
    public static AppBuilder CreateSplatBuilder() => new(Locator.CurrentMutable);

    /// <summary>
    /// Resets the builder state for tests, ONLY if the builder is being used in a unit test environment.
    /// </summary>
    public static void ResetBuilderStateForTests()
    {
        // Reset the static state of the builder if in unit tests or similar scenarios
        if (ModeDetector.InUnitTestRunner())
        {
            HasBeenBuilt = false;
            UsingBuilder = false;
        }
    }

    /// <summary>
    /// Direct the builder to use the current Splat Locator (Locator.CurrentMutable)
    /// for subsequent registrations. This is useful when configuring an external
    /// container (e.g., Autofac, DryIoc, Microsoft.Extensions.DependencyInjection)
    /// as the Splat dependency resolver prior to applying ReactiveUI registrations.
    /// </summary>
    /// <returns>The builder instance for chaining.</returns>
    public AppBuilder UseCurrentSplatLocator()
    {
        _resolverProvider = () => Locator.CurrentMutable;
        return this;
    }

    /// <summary>
    /// Using the splat module.
    /// </summary>
    /// <typeparam name="T">The Splat Module Type.</typeparam>
    /// <param name="registrationModule">The registration module to add.</param>
    /// <returns>
    /// The builder instance for method chaining.
    /// </returns>
    public AppBuilder UsingModule<T>(T registrationModule)
        where T : IModule
    {
        registrationModule.ThrowArgumentNullExceptionIfNull(nameof(registrationModule));
        _registrations.Add(registrationModule.Configure);
        return this;
    }

    /// <summary>
    /// Registers a custom registration action.
    /// </summary>
    /// <param name="configureAction">The configuration action to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AppBuilder WithCustomRegistration(Action<IMutableDependencyResolver> configureAction)
    {
        configureAction.ThrowArgumentNullExceptionIfNull(nameof(configureAction));

        _registrations.Add(configureAction);
        return this;
    }

    /// <summary>
    /// Registers the core ReactiveUI services.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("WithCoreServices may use reflection and will not work in AOT environments.")]
    [RequiresUnreferencedCode("WithCoreServices may use reflection and will not work in AOT environments.")]
#endif
    public virtual AppBuilder WithCoreServices() => this;

    /// <summary>
    /// Builds and applies all registrations to the dependency resolver.
    /// </summary>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The Build may use reflection and will not work in AOT environments.")]
    [RequiresUnreferencedCode("The method may use reflection and will not work in AOT environments.")]
#endif
    public void Build()
    {
        // If the builder has already been built, do nothing.
        if (HasBeenBuilt)
        {
            return;
        }

        // Mark as initialized using the builder so reflection-based initialization is disabled.
        HasBeenBuilt = true;

        // Ensure core services are always registered
        WithCoreServices();

        // Apply all registrations against the current resolver source
        foreach (var registration in _registrations)
        {
            var targetResolver = _resolverProvider();
            registration(targetResolver);
        }
    }
}
