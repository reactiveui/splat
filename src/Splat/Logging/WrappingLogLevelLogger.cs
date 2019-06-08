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
    public class WrappingLogLevelLogger : ILogger
    {
        private readonly ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingLogLevelLogger"/> class.
        /// Placeholder.
        /// </summary>
        /// <param name="inner">The <see cref="ILogger"/> to wrap in this class.</param>
        public WrappingLogLevelLogger(ILogger inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public LogLevel Level => _inner.Level;

        /// <inheritdoc />
        public void Write([Localizable(false)]string message, LogLevel logLevel)
        {
            _inner.Write($"{logLevel}: {message}", logLevel);
        }

        /// <inheritdoc />
        public void Write(Exception exception, [Localizable(false)]string message, LogLevel logLevel)
        {
            _inner.Write(exception, $"{logLevel}: {message}", logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _inner.Write($"{logLevel}: {message}", type, logLevel);
        }

        /// <inheritdoc />
        public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _inner.Write(exception, $"{logLevel}: {message}", type, logLevel);
        }
    }
}
