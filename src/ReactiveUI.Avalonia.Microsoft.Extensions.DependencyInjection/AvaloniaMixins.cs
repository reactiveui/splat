// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
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
                _ => builder.AfterPlatformServicesSetup(_ =>
                {
                    if (Locator.CurrentMutable is null)
                    {
                        return;
                    }

                    if (containerConfig is null)
                    {
                        throw new ArgumentNullException(nameof(containerConfig));
                    }

                    PlatformRegistrationManager.SetRegistrationNamespaces(RegistrationNamespace.Avalonia);
                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                    Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
                    Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));

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
    }
}
