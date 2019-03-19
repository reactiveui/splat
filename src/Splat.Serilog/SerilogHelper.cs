// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace Splat
{
    internal static class SerilogHelper
    {
        /// <summary>
        /// Gets a list of mappings of Serilog levels and equivalent Splat log levels.
        /// </summary>
        public static KeyValuePair<LogLevel, LogEventLevel>[] Mappings { get; } =
        {
            new KeyValuePair<LogLevel, LogEventLevel>(LogLevel.Debug, LogEventLevel.Debug),
            new KeyValuePair<LogLevel, LogEventLevel>(LogLevel.Info, LogEventLevel.Information),
            new KeyValuePair<LogLevel, LogEventLevel>(LogLevel.Warn, LogEventLevel.Warning),
            new KeyValuePair<LogLevel, LogEventLevel>(LogLevel.Error, LogEventLevel.Error),
            new KeyValuePair<LogLevel, LogEventLevel>(LogLevel.Fatal, LogEventLevel.Fatal)
        };

        /// <summary>
        /// Gets a dictionary which maps Splat log levels to Serilogs.
        /// </summary>
        public static ImmutableDictionary<LogLevel, LogEventLevel> MappingsDictionary { get; } = Mappings.ToImmutableDictionary();
    }
}
