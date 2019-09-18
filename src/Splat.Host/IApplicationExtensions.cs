// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Splat
{
    /// <summary>
    /// Extension methods to interact with the <see cref="IApplication"/>.
    /// </summary>
    public static class IApplicationExtensions
    {
        /// <summary>
        /// Runs the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>An observable sequence notifying completion.</returns>
        public static IObservable<Unit> Run(this IApplication application)
        {
            return Observable.Return(Unit.Default);
        }

        /// <summary>
        /// Waits for shutdown.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>An observable sequence notifying completion.</returns>
        public static IObservable<Unit> WaitForShutdown(this IApplication application)
        {
            return Observable.Return(Unit.Default);
        }
    }
}
