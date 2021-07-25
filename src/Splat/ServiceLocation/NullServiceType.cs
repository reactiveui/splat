// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat
{
    /// <summary>
    /// Null Service Type.
    /// </summary>
    public class NullServiceType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullServiceType"/> class.
        /// </summary>
        /// <param name="factory">The value factory.</param>
        public NullServiceType(Func<object?> factory) => Factory = factory;

        /// <summary>
        /// Gets the Factory.
        /// </summary>
        public Func<object?> Factory { get; }
    }
}
