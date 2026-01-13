// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>
/// Provides an adapter that wraps a Microsoft.Extensions.Logging.ILogger instance for use with the ILogger interface.
/// </summary>
/// <remarks>This class enables integration between components expecting an ILogger implementation and the
/// Microsoft.Extensions.Logging infrastructure. It delegates all logging operations to the underlying
/// Microsoft.Extensions.Logging.ILogger instance supplied at construction. Thread safety and log level filtering are
/// determined by the behavior of the wrapped logger.</remarks>
[DebuggerDisplay("Name={_inner.GetType()} Level={Level}")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "API limitation we can't use structured. TODO fix")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "API limitation we can't use structured. TODO fix")]
public sealed class MicrosoftExtensionsLoggingLogger : ILogger
{
    private readonly global::Microsoft.Extensions.Logging.ILogger _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftExtensionsLoggingLogger"/> class.
    /// </summary>
    /// <param name="inner">The Microsoft.Extensions.Logging logger instance to wrap.</param>
    public MicrosoftExtensionsLoggingLogger(global::Microsoft.Extensions.Logging.ILogger inner)
    {
        ArgumentExceptionHelper.ThrowIfNull(inner);
        _inner = inner;
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
    public void Write(string message, LogLevel logLevel) => _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        using (_inner.BeginScope(type.ToString()))
        {
            _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], message);
        }
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        using (_inner.BeginScope(type.ToString()))
        {
            _inner.Log(MsLoggingHelpers.Splat2MsLogDictionary[logLevel], exception, message);
        }
    }
}
