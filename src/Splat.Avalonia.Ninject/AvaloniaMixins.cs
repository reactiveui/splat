﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;
using ReactiveUI;
using Splat;
using Splat.Ninject;

namespace Avalonia.ReactiveUI.Splat
{
    /// <summary>
    /// Avalonia Mixins.
    /// </summary>
    public static class AvaloniaMixins
    {
        /// <summary>
        /// Uses the splat with dry ioc.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="containerConfig">The configure.</param>
        /// <returns>An App Builder.</returns>
        public static AppBuilder UseReactiveUIWithNinject(this AppBuilder builder, Action<StandardKernel> containerConfig) =>
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

                    var container = new StandardKernel();
                    Locator.CurrentMutable.RegisterConstant(container);
                    Locator.SetLocator(new NinjectDependencyResolver(container));
                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                    containerConfig(container);
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
                        Locator.CurrentMutable.RegisterConstant(container);
                        var dependencyResolver = dependencyResolverFactory(container);
                        Locator.SetLocator(dependencyResolver);
                        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                        containerConfig(container);
                    })
                };
    }
}
