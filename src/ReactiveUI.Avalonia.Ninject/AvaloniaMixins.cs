﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
        /// <param name="configure">The configure.</param>
        /// <returns>An App Builder.</returns>
        public static AppBuilder UseReactiveUIWithNinject(this AppBuilder builder, Action<StandardKernel> configure) =>
            builder switch
            {
                null => throw new ArgumentNullException(nameof(builder)),
                _ => builder.UseReactiveUI().AfterPlatformServicesSetup(_ =>
                {
                    if (Locator.CurrentMutable is null)
                    {
                        return;
                    }

                    var container = new StandardKernel();
                    Locator.CurrentMutable.RegisterConstant(container, typeof(StandardKernel));
                    container.UseNinjectDependencyResolver();

                    RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

                    configure(container);
                })
            };
    }
}
