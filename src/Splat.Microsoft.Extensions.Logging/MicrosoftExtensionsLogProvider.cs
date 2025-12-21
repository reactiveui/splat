// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>
/// Provider that bridges Microsoft.Extensions.Logging to Splat's logging system.
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1823:Avoid unused private fields", Justification = "Deliberate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration", Justification = "Deliberate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop", "IDE0052:Remove unused member declaration", Justification = "Deliberate")]
        private readonly string _categoryName = categoryName;

        /// <inheritdoc />
        public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ArgumentExceptionHelper.ThrowIfNull(formatter);

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
