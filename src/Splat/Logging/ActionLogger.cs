// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace Splat
{
    /// <summary>
    /// A logger where you pass in Action delegates that will be invoked when the Write methods are invoked.
    /// </summary>
    public class ActionLogger : ILogger
    {
        private readonly Action<string, LogLevel> _writeNoType;
        private readonly Action<string, LogLevel, Exception> _writeNoTypeWithException;
        private readonly Action<string, Type, LogLevel> _writeWithType;
        private readonly Action<string, Type, LogLevel, Exception> _writeWithTypeAndException;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogger"/> class.
        /// </summary>
        /// <param name="writeNoType">A action which is called when the <see cref="Write(string, LogLevel)"/> is called.</param>
        /// <param name="writeWithType">A action which is called when the <see cref="Write(string, Type, LogLevel)"/> is called.</param>
        /// <param name="writeNoTypeWithException">A action which is called when the <see cref="Write(string, LogLevel, Exception)"/> is called.</param>
        /// <param name="writeWithTypeAndException">A action which is called when the <see cref="Write(string, Type, LogLevel, Exception)"/> is called.</param>
        public ActionLogger(
            Action<string, LogLevel> writeNoType,
            Action<string, Type, LogLevel> writeWithType,
            Action<string, LogLevel, Exception> writeNoTypeWithException,
            Action<string, Type, LogLevel, Exception> writeWithTypeAndException)
        {
            _writeNoType = writeNoType;
            _writeWithType = writeWithType;
            _writeNoTypeWithException = writeNoTypeWithException;
            _writeWithTypeAndException = writeWithTypeAndException;
        }

        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            _writeNoType?.Invoke(message, logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, LogLevel logLevel, Exception exception)
        {
            _writeNoTypeWithException?.Invoke(message, logLevel, exception);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _writeWithType?.Invoke(message, type, logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel, Exception exception)
        {
            _writeWithTypeAndException?.Invoke(message, type, logLevel, exception);
        }
    }
}
