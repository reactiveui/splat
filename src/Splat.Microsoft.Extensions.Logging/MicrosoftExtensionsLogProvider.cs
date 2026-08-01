// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>
/// Provides an implementation of the ILoggerProvider interface that integrates Splat logging with
/// Microsoft.Extensions.Logging.
/// </summary>
/// <remarks>This provider enables applications using Microsoft.Extensions.Logging to route log messages through
/// the Splat logging infrastructure. It is typically used to bridge logging between libraries or frameworks that rely
/// on different logging abstractions. Instances of this class are intended to be registered with a logging factory or
/// dependency injection container.</remarks>
public sealed class MicrosoftExtensionsLogProvider : ILoggerProvider
{
    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public global::Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) => new SplatLoggingAdapter();

    /// <summary>Adapts a Splat logger so it can be consumed through the Microsoft.Extensions.Logging abstraction.</summary>
    private sealed class SplatLoggingAdapter : global::Microsoft.Extensions.Logging.ILogger
    {
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
            null!; // The documentation states we are allowed to return null; the nullable annotations added in net6 made this a build issue.
    }
}
