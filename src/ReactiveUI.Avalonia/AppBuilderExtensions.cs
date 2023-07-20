// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
    }
}
