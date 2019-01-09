// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat
{
#if PORTABLE || WINDOWS_PHONE || NETFX_CORE
    /// <summary>
    /// Specifies whether a property should be localized.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocalizableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableAttribute"/> class.
        /// </summary>
        /// <param name="isLocalizable">If the value is localizable or not.</param>
        public LocalizableAttribute(bool isLocalizable)
        {
            IsLocalizable = isLocalizable;
        }

        /// <summary>
        /// Gets a value indicating whether a property should be localized.
        /// </summary>
        public bool IsLocalizable { get; }
    }
#endif
}
