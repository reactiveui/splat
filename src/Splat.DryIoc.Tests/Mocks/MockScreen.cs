// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Splat.DryIoc.Tests
{
    /// <summary>
    /// An <see cref="IScreen"/> implementation.
    /// </summary>
    /// <seealso cref="IScreen" />
    public class MockScreen : IScreen
    {
        /// <summary>
        /// Gets the Router associated with this Screen.
        /// </summary>
        public RoutingState Router { get; }
    }
}
