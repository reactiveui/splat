﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A exception that occurs when there is a problem using the logging module.
/// </summary>
[Serializable]
public class LoggingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingException"/> class.
    /// </summary>
    public LoggingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingException"/> class.
    /// </summary>
    /// <param name="message">The message about the exception.</param>
    public LoggingException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingException"/> class.
    /// </summary>
    /// <param name="message">The message about the exception.</param>
    /// <param name="innerException">Any other internal exceptions we are mapping.</param>
    public LoggingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
