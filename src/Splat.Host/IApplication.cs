// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;

namespace Splat.Host
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihost?view=aspnetcore-2.2 .
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns>An observable sequence notifying completion.</returns>
        IObservable<Unit> Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>An observable sequence notifying completion.</returns>
        IObservable<Unit> Stop();
    }
}
