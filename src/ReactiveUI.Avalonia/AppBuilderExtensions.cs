// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using ReactiveUI;
using Splat;

namespace Avalonia.ReactiveUI
{
    /// <summary>
    /// App Builder Extensions.
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Initializes ReactiveUI framework to use with Avalonia. Registers Avalonia
        /// scheduler, an activation for view fetcher, a template binding hook. Remember
        /// to call this method if you are using ReactiveUI in your application.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// An AppBuilder.
        /// </returns>
        public static AppBuilder UseReactiveUI(this AppBuilder builder) =>
            builder.AfterPlatformServicesSetup(_ => Locator.RegisterResolverCallbackChanged(() =>
            {
                if (Locator.CurrentMutable is null)
                {
                    return;
                }

                PlatformRegistrationManager.SetRegistrationNamespaces(RegistrationNamespace.Avalonia);
                RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
                Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
            }));

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
                    _ => builder.AfterPlatformServicesSetup(_ =>
                    {
                        if (Locator.CurrentMutable is null)
                        {
                            return;
                        }

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

                        PlatformRegistrationManager.SetRegistrationNamespaces(RegistrationNamespace.Avalonia);
                        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                        Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
                        Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));

                        var container = containerFactory();
                        Locator.CurrentMutable.RegisterConstant(container, typeof(Container));
                        var dependencyResolver = dependencyResolverFactory(container);
                        Locator.SetLocator(dependencyResolver);
                        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                        containerConfig(container);
                    })
                };
    }
}
