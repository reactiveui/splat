// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;
using ReactiveUI;
using Splat;
using Splat.Autofac;

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
        /// <param name="withResolver">The get resolver.</param>
        /// <returns>
        /// An App Builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">builder.</exception>
        public static AppBuilder UseReactiveUIWithAutofac(this AppBuilder builder, Action<ContainerBuilder> containerConfig, Action<AutofacDependencyResolver>? withResolver = null) =>
            builder switch
            {
                null => throw new ArgumentNullException(nameof(builder)),
                _ => builder.UseReactiveUI().AfterPlatformServicesSetup(_ =>
                {
                    if (Locator.CurrentMutable is null)
                    {
                        return;
                    }

                    if (containerConfig is null)
                    {
                        throw new ArgumentNullException(nameof(containerConfig));
                    }

                    var builder = new ContainerBuilder();
                    var autofacResolver = new AutofacDependencyResolver(builder);
                    Locator.SetLocator(autofacResolver);
                    builder.RegisterInstance(autofacResolver);
                    autofacResolver.InitializeReactiveUI();
                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                    containerConfig(builder);
                    var container = builder.Build();
                    autofacResolver.SetLifetimeScope(container);

                    if (withResolver is not null)
                    {
                        withResolver(autofacResolver);
                    }
                })
            };
    }
}
