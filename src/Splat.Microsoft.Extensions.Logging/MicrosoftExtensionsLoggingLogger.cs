// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Splat
{
    /// <summary>
    /// Microsoft.Extensions.Logging Logger integration into Splat.
    /// </summary>
    [DebuggerDisplay("Name={_inner.GetType()} Level={Level}")]
    public sealed class MicrosoftExtensionsLoggingLogger : ILogger
    {
        private static readonly KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>[] _mappings = new[]
        {
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Debug, global::Microsoft.Extensions.Logging.LogLevel.Debug),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Info, global::Microsoft.Extensions.Logging.LogLevel.Information),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Warn, global::Microsoft.Extensions.Logging.LogLevel.Warning),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Error, global::Microsoft.Extensions.Logging.LogLevel.Error),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Fatal, global::Microsoft.Extensions.Logging.LogLevel.Critical)
        };

        private static readonly ImmutableDictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel> _mappingsDictionary = _mappings.ToImmutableDictionary();

        private readonly global::Microsoft.Extensions.Logging.ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftExtensionsLoggingLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual Microsoft.Extensions.Logging logger.</param>
        /// <exception cref="ArgumentNullException">Microsoft.Extensions.Logging logger not passed.</exception>
        public MicrosoftExtensionsLoggingLogger(global::Microsoft.Extensions.Logging.ILogger inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            SetLogLevel();
        }

        /// <inheritdoc />
        public LogLevel Level
        {
            get; private set;
        }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(_mappingsDictionary[logLevel], message);
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(_mappingsDictionary[logLevel], exception, message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(_mappingsDictionary[logLevel], $"{type.Name}: {message}");
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(_mappingsDictionary[logLevel], exception, $"{type.Name}: {message}");
        }

        private void OnInnerLoggerReconfigured(object sender, EventArgs e)
        {
            SetLogLevel();
        }

        /// <summary>
        /// Works out the log level.
        /// </summary>
        /// <remarks>
        /// This was done so the Level property doesn't keep getting re-evaluated each time a Write method is called.
        /// </remarks>
        private void SetLogLevel()
        {
            foreach (var mapping in _mappings)
            {
              if (_inner.IsEnabled(mapping.Value))
              {
                  Level = mapping.Key;
                  return;
              }
            }

            // Default to Fatal, it should always be enabled anyway.
            Level = LogLevel.Fatal;
        }
    }
}
