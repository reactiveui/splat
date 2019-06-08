// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging
{
    /// <summary>
    /// A logging provider which talks to Splat.
    /// </summary>
    public sealed class MicrosoftExtensionsLogProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public global::Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new SplatLoggingAdapter(categoryName);
        }

        private class SplatLoggingAdapter : global::Microsoft.Extensions.Logging.ILogger
        {
            private readonly string _categoryName;

            public SplatLoggingAdapter(string categoryName)
            {
                _categoryName = categoryName;
            }

            /// <inheritdoc />
            public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                if (formatter == null)
                {
                    throw new ArgumentNullException(nameof(formatter));
                }

                var splatLogLevel = MsLoggingHelpers.MsLog2SplatDictionary[logLevel];

                var message = formatter(state, exception);

                LogHost.Default.Write(exception, message, splatLogLevel);
            }

            /// <inheritdoc />
            public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel)
            {
                return logLevel != global::Microsoft.Extensions.Logging.LogLevel.None;
            }

            /// <inheritdoc />
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
