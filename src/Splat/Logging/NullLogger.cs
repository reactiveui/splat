﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat
{
    /// <summary>
    /// a logger which will never emit any value.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
        }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel, Exception exception)
        {
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel, Exception exception)
        {
        }
    }
}
