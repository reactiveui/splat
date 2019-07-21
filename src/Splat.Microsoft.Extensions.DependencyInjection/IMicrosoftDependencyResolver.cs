// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Uses as a wrapper for the regular <see cref="IDependencyResolver"/>,
    /// to allow for updating the <see cref="IServiceProvider"/> once ready.
    /// </summary>
    public interface IMicrosoftDependencyResolver : IDependencyResolver
    {
        void Dispose();

        /// <summary>
        /// Uses to inject the service provider once it't ready.
        /// </summary>
        /// <param name="serviceProvider">The service provider that will be serving the application.</param>
        void UpdateContainer(IServiceProvider serviceProvider);
    }
}
