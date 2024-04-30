// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;

using Serilog.Events;

namespace Splat;

internal static class SerilogHelper
{
    /// <summary>
    /// Gets a list of mappings of Serilog levels and equivalent Splat log levels.
    /// </summary>
    public static KeyValuePair<LogLevel, LogEventLevel>[] Mappings { get; } =
    {
        new(LogLevel.Debug, LogEventLevel.Debug),
        new(LogLevel.Info, LogEventLevel.Information),
        new(LogLevel.Warn, LogEventLevel.Warning),
        new(LogLevel.Error, LogEventLevel.Error),
        new(LogLevel.Fatal, LogEventLevel.Fatal),
    };

    /// <summary>
    /// Gets a dictionary which maps Splat log levels to Serilogs.
    /// </summary>
    public static ImmutableDictionary<LogLevel, LogEventLevel> MappingsDictionary { get; } = Mappings.ToImmutableDictionary();
}
