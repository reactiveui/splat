// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace ReactiveUI.Avalonia.Splat
{
    /// <summary>
    /// Avalonia Mixins.
    /// </summary>
    public static class AvaloniaMixins
    {
        /// <summary>
        /// Uses the splat with microsoft dependency resolver.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="containerConfig">The configure.</param>
        /// <param name="withResolver">The get service provider.</param>
        /// <returns>An App Builder.</returns>
        public static AppBuilder UseReactiveUIWithMicrosoftDependencyResolver(this AppBuilder builder, Action<IServiceCollection> containerConfig, Action<IServiceProvider?>? withResolver = null) =>
            builder switch
            {
                null => throw new ArgumentNullException(nameof(builder)),
                _ => builder.UseReactiveUI().AfterPlatformServicesSetup(_ =>
                {
                    if (Locator.CurrentMutable is null)
                    {
                        return;
                    }

#if NETSTANDARD
                    if (containerConfig is null)
                    {
                        throw new ArgumentNullException(nameof(containerConfig));
                    }
#else
                    ArgumentNullException.ThrowIfNull(containerConfig);
#endif

                    IServiceCollection serviceCollection = new ServiceCollection();
                    Locator.CurrentMutable.RegisterConstant(serviceCollection, typeof(IServiceCollection));
                    Locator.SetLocator(new MicrosoftDependencyResolver(serviceCollection));
                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                    containerConfig(serviceCollection);
                    var serviceProvider = serviceCollection.BuildServiceProvider();
                    if (Locator.Current is MicrosoftDependencyResolver resolver)
                    {
                        resolver.UpdateContainer(serviceProvider);
                    }
                    else
                    {
                        Locator.SetLocator(new MicrosoftDependencyResolver(serviceProvider));
                    }

                    if (withResolver is not null)
                    {
                        withResolver(serviceProvider);
                    }
                })
            };

        /// <summary>
        /// Uses the reactive UI with di container.
        /// </summary>
        /// <typeparam name="TContainer">The type of the container.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="containerFactory">The container factory.</param>
        /// <param name="containerConfig">The container configuration.</param>
        /// <param name="dependencyResolverFactory">The dependency resolver factory.</param>
        /// <returns>
        /// An AppBuilder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">builder.</exception>
        public static AppBuilder UseReactiveUIWithDIContainer<TContainer>(
            this AppBuilder builder,
            Func<TContainer> containerFactory,
            Action<TContainer> containerConfig,
            Func<TContainer, IDependencyResolver> dependencyResolverFactory) =>
                builder switch
                {
                    null => throw new ArgumentNullException(nameof(builder)),
                    _ => builder.UseReactiveUI().AfterPlatformServicesSetup(_ =>
                    {
                        if (Locator.CurrentMutable is null)
                        {
                            return;
                        }

#if NETSTANDARD
                        if (containerFactory is null)
                        {
                            throw new ArgumentNullException(nameof(containerFactory));
                        }

                        if (containerConfig is null)
                        {
                            throw new ArgumentNullException(nameof(containerConfig));
                        }

                        if (dependencyResolverFactory is null)
                        {
                            throw new ArgumentNullException(nameof(dependencyResolverFactory));
                        }
#else
                        ArgumentNullException.ThrowIfNull(containerFactory);
                        ArgumentNullException.ThrowIfNull(containerConfig);
                        ArgumentNullException.ThrowIfNull(dependencyResolverFactory);
#endif

                        var container = containerFactory();
                        Locator.CurrentMutable.RegisterConstant(container, typeof(TContainer));
                        var dependencyResolver = dependencyResolverFactory(container);
                        Locator.SetLocator(dependencyResolver);
                        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                        containerConfig(container);
                    })
                };
    }
}
