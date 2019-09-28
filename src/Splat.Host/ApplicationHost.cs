// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Host
{
    /// <summary>
    /// Provides convenience methods for <see cref="IApplicationHost"/> and <see cref="IApplicationHostBuilder"/>.
    /// </summary>
    public static class ApplicationHost
    {
        /// <summary>
        /// Creates the default host builder.
        /// </summary>
        /// <returns>An application host builder.</returns>
        public static IApplicationHostBuilder CreateDefaultBuilder() => default;
    }
}
