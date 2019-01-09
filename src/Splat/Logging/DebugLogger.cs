// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

// We need to define the DEBUG symbol because we want the logger
// to work even when this package is compiled on release. Otherwise,
// the call to Debug.WriteLine will not be in the release binary
#define DEBUG

namespace Splat
{
    /// <summary>
    /// A logger which will send messages to the debug logger.
    /// </summary>
    public class DebugLogger : ILogger
    {
        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
