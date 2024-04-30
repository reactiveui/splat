// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

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
    public global::Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) => new SplatLoggingAdapter(categoryName);

    private sealed class SplatLoggingAdapter(string categoryName) : global::Microsoft.Extensions.Logging.ILogger
    {
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable RCS1213 // Remove unused member declaration.
        private readonly string _categoryName = categoryName;
#pragma warning restore RCS1213 // Remove unused member declaration.
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore CA1823 // Avoid unused private fields

        /// <inheritdoc />
        public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

#if NETSTANDARD || NETFRAMEWORK
            if (formatter is null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
#else
            ArgumentNullException.ThrowIfNull(formatter);
#endif

            var splatLogLevel = MsLoggingHelpers.MsLog2SplatDictionary[logLevel];

            var message = formatter(state, exception);

            LogHost.Default.Write(exception!, message, splatLogLevel);
        }

        /// <inheritdoc />
        public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) => logLevel != global::Microsoft.Extensions.Logging.LogLevel.None;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
             where TState : notnull =>

            // documentation states we're allowed to return null.
            // NRT in net6 causing build issue as of 2021-11-10.
            null!;
    }
}
