// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Host
{
    /// <summary>
    /// Application host builder.
    /// </summary>
    public interface IApplicationHostBuilder
    {
        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>An app host.</returns>
        IApplicationHost Build();

        /// <summary>
        /// Uses the startup class to initialize the host.
        /// </summary>
        /// <typeparam name="T">The startup type.</typeparam>
        /// <returns>An application host builder.</returns>
        IApplicationHostBuilder UseStartup<T>()
            where T : IStartup;
    }
}
