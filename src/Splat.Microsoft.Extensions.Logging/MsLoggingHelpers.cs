// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Splat.Microsoft.Extensions.Logging
{
    internal static class MsLoggingHelpers
    {
        public static KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>[] Mappings { get; } =
        {
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Debug, global::Microsoft.Extensions.Logging.LogLevel.Debug),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Info, global::Microsoft.Extensions.Logging.LogLevel.Information),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Warn, global::Microsoft.Extensions.Logging.LogLevel.Warning),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Error, global::Microsoft.Extensions.Logging.LogLevel.Error),
            new KeyValuePair<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>(LogLevel.Fatal, global::Microsoft.Extensions.Logging.LogLevel.Critical)
        };

        public static ImmutableDictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel> Splat2MsLogDictionary { get; } = Mappings.ToImmutableDictionary();

        public static ImmutableDictionary<global::Microsoft.Extensions.Logging.LogLevel, LogLevel> MsLog2SplatDictionary { get; } = Mappings.ToImmutableDictionary(x => x.Value, x => x.Key);
    }
}
