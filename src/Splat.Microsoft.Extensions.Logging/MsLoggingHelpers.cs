// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>
/// Provides helper mappings and dictionaries for converting between custom LogLevel values and
/// Microsoft.Extensions.Logging.LogLevel values.
/// </summary>
/// <remarks>This class is intended for internal use to facilitate interoperability between different logging
/// abstractions. All members are static and thread-safe.</remarks>
internal static class MsLoggingHelpers
{
    /// <summary>
    /// Gets the mapping between custom log levels and Microsoft.Extensions.Logging log levels.
    /// </summary>
    /// <remarks>Use this property to translate between the application's log level enumeration and the
    /// standard log levels used by Microsoft.Extensions.Logging. The order and contents of the array are fixed and
    /// correspond to the defined log level pairs.</remarks>
    public static KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>[] Mappings { get; } =
    [
        new(LogLevel.Debug, global::Microsoft.Extensions.Logging.LogLevel.Debug),
        new(LogLevel.Info, global::Microsoft.Extensions.Logging.LogLevel.Information),
        new(LogLevel.Warn, global::Microsoft.Extensions.Logging.LogLevel.Warning),
        new(LogLevel.Error, global::Microsoft.Extensions.Logging.LogLevel.Error),
        new(LogLevel.Fatal, global::Microsoft.Extensions.Logging.LogLevel.Critical)];

    /// <summary>
    /// Gets a read-only mapping of Splat log levels to their corresponding Microsoft.Extensions.Logging log levels.
    /// </summary>
    /// <remarks>This dictionary provides a convenient way to translate between Splat's LogLevel enumeration
    /// and the log levels used by Microsoft.Extensions.Logging. The mapping is intended for use when integrating
    /// Splat-based logging with Microsoft.Extensions.Logging infrastructure.</remarks>
    public static ImmutableDictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel> Splat2MsLogDictionary { get; } = Mappings.ToImmutableDictionary();

    /// <summary>
    /// Gets a mapping between Microsoft.Extensions.Logging log levels and their corresponding Splat log levels.
    /// </summary>
    /// <remarks>This dictionary provides a convenient way to translate log levels from
    /// Microsoft.Extensions.Logging to the equivalent Splat log levels when integrating logging between the two
    /// frameworks.</remarks>
    public static ImmutableDictionary<global::Microsoft.Extensions.Logging.LogLevel, LogLevel> MsLog2SplatDictionary { get; } = Mappings.ToImmutableDictionary(x => x.Value, x => x.Key);
}
