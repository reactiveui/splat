// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Splat.Host
{
    /// <summary>
    /// Replacing the IWantsToRegisterStuff interface.
    /// </summary>
    public interface IPlatformRegistrations
    {
        /// <summary>
        /// Registers the constant.
        /// </summary>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns>The platform registrar.</returns>
        IPlatformRegistrations RegisterConstant<TConcrete, TInterface>();

        /// <summary>
        /// Registers the constant.
        /// </summary>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <returns>The platform registrar.</returns>
        IPlatformRegistrations RegisterConstant<TConcrete, TInterface>(Func<TConcrete> factory);

        /// <summary>
        /// Registers the constant.
        /// </summary>
        /// <typeparam name="T">The constants type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="type">The type.</param>
        /// <returns>The platform registrar.</returns>
        IPlatformRegistrations RegisterConstant<T>(Func<T> factory, Type type);
    }
}
