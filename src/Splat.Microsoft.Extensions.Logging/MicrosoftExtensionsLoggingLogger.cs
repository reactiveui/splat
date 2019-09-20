// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging
{
    /// <summary>
    /// Microsoft.Extensions.Logging Logger integration into Splat.
    /// </summary>
    [DebuggerDisplay("Name={_inner.GetType()} Level={Level}")]
    public sealed class MicrosoftExtensionsLoggingLogger : ILogger
    {
        private readonly global::Microsoft.Extensions.Logging.ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftExtensionsLoggingLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual Microsoft.Extensions.Logging logger.</param>
        /// <exception cref="ArgumentNullException">Microsoft.Extensions.Logging logger not passed.</exception>
        public MicrosoftExtensionsLoggingLogger(global::Microsoft.Extensions.Logging.ILogger inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <inheritdoc />
        public LogLevel Level
        {
            get
            {
                foreach (var mapping in MsLoggingHelpers.Mappings)
                {
                    if (_inner.IsEnabled(mapping.Value))
                    {
                        return mapping.Key;
                    }
                }

                // Default to Fatal, it should always be enabled anyway.
                return LogLevel.Fatal;
            }
        }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], message);
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], exception, message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            using (_inner.BeginScope(type.ToString()))
            {
                _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], message);
            }
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            using (_inner.BeginScope(type.ToString()))
            {
                _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], exception, message);
            }
        }
    }
}
