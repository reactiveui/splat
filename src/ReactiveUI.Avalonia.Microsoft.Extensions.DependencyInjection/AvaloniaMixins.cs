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
        /// <param name="configure">The configure.</param>
        /// <param name="getServiceProvider">The get service provider.</param>
        /// <returns>An App Builder.</returns>
        public static AppBuilder UseReactiveUIWithMicrosoftDependencyResolver(this AppBuilder builder, Action<IServiceCollection> configure, Action<IServiceProvider?>? getServiceProvider = null) =>
            builder switch
            {
                null => throw new ArgumentNullException(nameof(builder)),
                _ => builder.UseReactiveUI().AfterPlatformServicesSetup(_ =>
                {
                    if (Locator.CurrentMutable is null)
                    {
                        return;
                    }

                    IServiceCollection services = new ServiceCollection();
                    Locator.CurrentMutable.RegisterConstant(services, typeof(IServiceCollection));
                    services.UseMicrosoftDependencyResolver();

                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

                    configure(services);
                    if (getServiceProvider is null)
                    {
                        return;
                    }

                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.UseMicrosoftDependencyResolver();
                    getServiceProvider(serviceProvider);
                })
            };
    }
}
