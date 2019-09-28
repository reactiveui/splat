// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Splat.Host
{
    /// <summary>
    /// Interface representing an <see cref="IConfiguration"/> builder.
    /// </summary>
    public interface IConfigurationBuilder
    {
        /// <summary>
        /// Gets the sources.
        /// </summary>
        IEnumerable<IConfigurationSource> Sources { get; }

        /// <summary>
        /// Adds the specified configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        void Add(IConfigurationSource configurationSource);

        /// <summary>
        /// Builds the <see cref="IConfiguration"/> instance.
        /// </summary>
        /// <returns>The configuration.</returns>
        IConfiguration Build();
    }
}
