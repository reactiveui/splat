// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Contains help with formatting.
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// A constant for the new lines.
        /// </summary>
        internal static readonly char[] NewLine = Environment.NewLine.ToCharArray();

        /// <summary>
        /// Gets an exception for testing.
        /// </summary>
        internal static Exception Exception => new Exception();
    }
}
