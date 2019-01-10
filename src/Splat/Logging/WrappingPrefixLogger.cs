// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace Splat
{
    /// <summary>
    /// A prefix logger which wraps a <see cref="ILogger"/>.
    /// </summary>
    public class WrappingPrefixLogger : ILogger
    {
        private readonly ILogger _inner;
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingPrefixLogger"/> class.
        /// Placeholder.
        /// </summary>
        /// <param name="inner">The <see cref="ILogger"/> to wrap in this class.</param>
        /// <param name="callingType">The type which will be calling this logger.</param>
        public WrappingPrefixLogger(ILogger inner, Type callingType)
        {
            _inner = inner;
            _prefix = string.Format(CultureInfo.InvariantCulture, "{0}: ", callingType.Name);
        }

        /// <inheritdoc />
        public LogLevel Level
        {
            get { return _inner.Level; }
            set { _inner.Level = value; }
        }

        /// <inheritdoc />
        public void Write([Localizable(false)]string message, LogLevel logLevel)
        {
            _inner.Write(_prefix + message, logLevel);
        }
    }
}
