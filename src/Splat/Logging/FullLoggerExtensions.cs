// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat;

/// <summary>
/// Provides extension methods to the <see cref="IFullLogger"/> interface.
/// </summary>
public static class FullLoggerExtensions
{
    /// <summary>
    /// Sends the value provided by the provided delegate, only if Debug is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Debug logging is enabled.</param>
    public static void Debug(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsDebugEnabled)
        {
            logger.Debug(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Debug is enabled.
    /// </summary>
    /// <typeparam name="T">The type of object we are logging about.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Debug logging is enabled.</param>
    public static void Debug<T>(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsDebugEnabled)
        {
            logger.Debug<T>(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Debug is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Debug logging is enabled.</param>
    /// <param name="exception">A exception to log about.</param>
    public static void DebugException(this IFullLogger logger, Func<string> function, Exception exception)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsDebugEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            logger.DebugException(function.Invoke(), exception);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Debug is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Debug logging is enabled.</param>
    public static void Info(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsInfoEnabled)
        {
            logger.Info(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Debug is enabled.
    /// </summary>
    /// <typeparam name="T">The type of object we are logging about.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Debug logging is enabled.</param>
    public static void Info<T>(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsInfoEnabled)
        {
            logger.Info<T>(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Info is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Info logging is enabled.</param>
    /// <param name="exception">A exception to log about.</param>
    public static void InfoException(this IFullLogger logger, Func<string> function, Exception exception)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsInfoEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            logger.InfoException(function.Invoke(), exception);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Warn is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Warn logging is enabled.</param>
    public static void Warn(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsWarnEnabled)
        {
            logger.Warn(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Warn is enabled.
    /// </summary>
    /// <typeparam name="T">The type of object we are logging about.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Warn logging is enabled.</param>
    public static void Warn<T>(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsWarnEnabled)
        {
            logger.Warn<T>(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Warn is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Warn logging is enabled.</param>
    /// <param name="exception">A exception to log about.</param>
    public static void WarnException(this IFullLogger logger, Func<string> function, Exception exception)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsWarnEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            logger.WarnException(function.Invoke(), exception);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Error is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Error logging is enabled.</param>
    public static void Error(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsErrorEnabled)
        {
            logger.Error(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Error is enabled.
    /// </summary>
    /// <typeparam name="T">The type of object we are logging about.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Error logging is enabled.</param>
    public static void Error<T>(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsErrorEnabled)
        {
            logger.Error<T>(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Error is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Error logging is enabled.</param>
    /// <param name="exception">A exception to log about.</param>
    public static void ErrorException(this IFullLogger logger, Func<string> function, Exception exception)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsErrorEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            logger.ErrorException(function.Invoke(), exception);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Fatal is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Fatal logging is enabled.</param>
    public static void Fatal(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsFatalEnabled)
        {
            logger.Fatal(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Fatal is enabled.
    /// </summary>
    /// <typeparam name="T">The type of object we are logging about.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Fatal logging is enabled.</param>
    public static void Fatal<T>(this IFullLogger logger, Func<string> function)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsFatalEnabled)
        {
            logger.Fatal<T>(function.Invoke());
        }
    }

    /// <summary>
    /// Sends the value provided by the provided delegate, only if Fatal is enabled.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <param name="function">The function to evaluate if Fatal logging is enabled.</param>
    /// <param name="exception">A exception to log about.</param>
    public static void FatalException(this IFullLogger logger, Func<string> function, Exception exception)
    {
        logger.ThrowArgumentNullExceptionIfNull(nameof(logger));
        function.ThrowArgumentNullExceptionIfNull(nameof(function));

        if (logger.IsFatalEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            logger.ErrorException(function.Invoke(), exception);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
