﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;
using ReactiveUI;
using Splat;
using Splat.DryIoc;

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
        public static AppBuilder UseReactiveUIWithDryIoc(this AppBuilder builder, Action<Container> containerConfig) =>
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

                    var container = new Container();
                    Locator.CurrentMutable.RegisterConstant(container, typeof(Container));
                    Locator.SetLocator(new DryIocDependencyResolver(container));
                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
                    containerConfig(container);
                })
            };
    }
}
