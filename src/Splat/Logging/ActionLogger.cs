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
        private readonly Action<string, Type, LogLevel> _writeWithType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogger"/> class.
        /// </summary>
        /// <param name="writeNoType">A action which is called when the <see cref="Write(string, LogLevel)"/> is called.</param>
        /// <param name="writeWithType">A action which is called when the <see cref="Write(string, Type, LogLevel)"/> is called.</param>
        public ActionLogger(Action<string, LogLevel> writeNoType, Action<string, Type, LogLevel> writeWithType)
        {
            _writeNoType = writeNoType;
            _writeWithType = writeWithType;
        }

        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            _writeNoType?.Invoke(message, logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _writeWithType?.Invoke(message, type, logLevel);
        }
    }
}
