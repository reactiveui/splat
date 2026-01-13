// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;

namespace Splat;

/// <summary>
/// Provides a logger implementation that wraps an existing <see cref="ILogger"/> and exposes the full logging API
/// defined by <see cref="IFullLogger"/>. Supports logging messages at various severity levels with formatting and
/// exception support.
/// </summary>
/// <remarks>This class delegates all logging operations to the wrapped <see cref="ILogger"/> instance, adding
/// convenience overloads and formatting capabilities as defined by <see cref="IFullLogger"/>. It enables
/// allocation-free logging patterns and supports structured and formatted messages. Thread safety and performance
/// characteristics depend on the underlying <see cref="ILogger"/> implementation.</remarks>
public class WrappingFullLogger : AllocationFreeLoggerBase, IFullLogger
{
    private readonly ILogger _inner;
    private readonly MethodInfo _stringFormat = typeof(string).GetMethod("Format", [typeof(IFormatProvider), typeof(string), typeof(object[])]) ?? throw new InvalidOperationException("Cannot find the Format method which is required.");

    /// <summary>
    /// Initializes a new instance of the <see cref="WrappingFullLogger"/> class.
    /// </summary>
    /// <param name="inner">The <see cref="ILogger"/> to wrap in this class.</param>
    public WrappingFullLogger(ILogger inner)
        : base(inner)
    {
        ArgumentExceptionHelper.ThrowIfNull(inner);
        _inner = inner;
    }

    /// <inheritdoc />
    public void Debug<T>(T value)
    {
        if (!IsDebugEnabled || value is null)
        {
            return;
        }

        _inner.Write(value.ToString() ?? "(null)", LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug<T>(IFormatProvider formatProvider, T value) => _inner.Write(string.Format(formatProvider, "{0}", value), LogLevel.Debug);

    /// <inheritdoc />
    public void DebugException(string? message, Exception exception) => _inner.Write(exception, $"{message}: {exception}", LogLevel.Debug);

    /// <inheritdoc />
    public void Debug(Exception exception, string? message) => _inner.Write(exception, $"{message}", LogLevel.Debug);

    /// <inheritdoc />
    public void Debug(IFormatProvider formatProvider, string message, params object[] args)
    {
        var result = InvokeStringFormat(formatProvider, message, args);

        _inner.Write(result, LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug(string? message) => _inner.Write(message ?? "(null)", LogLevel.Debug);

    /// <inheritdoc />
    public void Debug<T>(string? message) => _inner.Write(message ?? "(null)", typeof(T), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug<T>(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, typeof(T), LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            _inner.Write(function.Invoke(), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public void Debug<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            _inner.Write(function.Invoke(), typeof(T), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void DebugException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _inner.Write(exception, $"{function.Invoke()}: {exception}", LogLevel.Debug);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <inheritdoc />
    public void Debug(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            _inner.Write(exception, function.Invoke(), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public void Info<T>(T value)
    {
        if (!IsInfoEnabled || value is null)
        {
            return;
        }

        _inner.Write(value.ToString() ?? "(null)", LogLevel.Info);
    }

    /// <inheritdoc />
    public void Info<T>(IFormatProvider formatProvider, T value) => _inner.Write(string.Format(formatProvider, "{0}", value), LogLevel.Info);

    /// <inheritdoc />
    public void InfoException(string? message, Exception exception) => _inner.Write(exception, $"{message}: {exception}", LogLevel.Info);

    /// <inheritdoc />
    public void Info(Exception exception, string? message) => _inner.Write(exception, $"{message}", LogLevel.Info);

    /// <inheritdoc />
    public void Info(IFormatProvider formatProvider, string message, params object[] args)
    {
        var result = InvokeStringFormat(formatProvider, message, args);
        _inner.Write(result, LogLevel.Info);
    }

    /// <inheritdoc />
    public void Info(string? message) => _inner.Write(message ?? "(null)", LogLevel.Info);

    /// <inheritdoc />
    public void Info<T>(string? message) => _inner.Write(message ?? "(null)", typeof(T), LogLevel.Info);

    /// <inheritdoc />
    public void Info(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, LogLevel.Info);
    }

    /// <inheritdoc />
    public void Info<T>(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, typeof(T), LogLevel.Info);
    }

    /// <inheritdoc />
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Info);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Info);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Info);

    /// <inheritdoc />
    public void Info(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            _inner.Write(function.Invoke(), LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Info<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            _inner.Write(function.Invoke(), typeof(T), LogLevel.Info);
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void InfoException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _inner.Write(exception, $"{function.Invoke()}: {exception}", LogLevel.Info);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <inheritdoc />
    public void Info(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            _inner.Write(exception, function.Invoke(), LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Warn<T>(T value)
    {
        if (!IsWarnEnabled || value is null)
        {
            return;
        }

        _inner.Write(value.ToString() ?? "(null)", LogLevel.Warn);
    }

    /// <inheritdoc />
    public void Warn<T>(IFormatProvider formatProvider, T value) => _inner.Write(string.Format(formatProvider, "{0}", value), LogLevel.Warn);

    /// <inheritdoc />
    public void WarnException(string? message, Exception exception) => _inner.Write(exception, $"{message}: {exception}", LogLevel.Warn);

    /// <inheritdoc />
    public void Warn(Exception exception, string? message) => _inner.Write(exception, $"{message}", LogLevel.Warn);

    /// <inheritdoc />
    public void Warn(IFormatProvider formatProvider, string message, params object[] args)
    {
        var result = InvokeStringFormat(formatProvider, message, args);
        _inner.Write(result, LogLevel.Warn);
    }

    /// <inheritdoc />
    public void Warn(string? message) => _inner.Write(message ?? "(null)", LogLevel.Warn);

    /// <inheritdoc />
    public void Warn<T>(string? message) => _inner.Write(message ?? "(null)", typeof(T), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, LogLevel.Warn);
    }

    /// <inheritdoc />
    public void Warn<T>(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, typeof(T), LogLevel.Warn);
    }

    /// <inheritdoc />
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            _inner.Write(function.Invoke(), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Warn<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            _inner.Write(function.Invoke(), typeof(T), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void WarnException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _inner.Write(exception, $"{function.Invoke()}: {exception}", LogLevel.Warn);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <inheritdoc />
    public void Warn(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            _inner.Write(exception, function.Invoke(), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Error<T>(T value)
    {
        if (!IsErrorEnabled || value is null)
        {
            return;
        }

        _inner.Write(value.ToString() ?? "(null)", LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<T>(IFormatProvider formatProvider, T value) => _inner.Write(string.Format(formatProvider, "{0}", value), LogLevel.Error);

    /// <inheritdoc />
    public void ErrorException(string? message, Exception exception) => _inner.Write(exception, $"{message}: {exception}", LogLevel.Error);

    /// <inheritdoc />
    public void Error(Exception exception, string? message) => _inner.Write(exception, $"{message}", LogLevel.Error);

    /// <inheritdoc />
    public void Error(IFormatProvider formatProvider, string message, params object[] args)
    {
        var result = InvokeStringFormat(formatProvider, message, args);
        _inner.Write(result, LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error(string? message) => _inner.Write(message ?? "(null)", LogLevel.Error);

    /// <inheritdoc />
    public void Error<T>(string? message) => _inner.Write(message ?? "(null)", typeof(T), LogLevel.Error);

    /// <inheritdoc />
    public void Error(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<T>(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, typeof(T), LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Error);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Error);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Error);

    /// <inheritdoc />
    public void Error(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            _inner.Write(function.Invoke(), LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Error<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            _inner.Write(function.Invoke(), typeof(T), LogLevel.Error);
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void ErrorException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _inner.Write(exception, $"{function.Invoke()}: {exception}", LogLevel.Error);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <inheritdoc />
    public void Error(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            _inner.Write(exception, function.Invoke(), LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Fatal<T>(T value)
    {
        if (!IsFatalEnabled || value is null)
        {
            return;
        }

        _inner.Write(value.ToString() ?? "(null)", LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<T>(IFormatProvider formatProvider, T value) => _inner.Write(string.Format(formatProvider, "{0}", value), LogLevel.Fatal);

    /// <inheritdoc />
    public void FatalException(string? message, Exception exception) => _inner.Write(exception, $"{message}: {exception}", LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal(Exception exception, string? message) => _inner.Write(exception, $"{message}", LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
    {
        var result = InvokeStringFormat(formatProvider, message, args);
        _inner.Write(result, LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal(string? message) => _inner.Write(message ?? "(null)", LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal<T>(string? message) => _inner.Write(message ?? "(null)", typeof(T), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<T>(string message, params object[] args)
    {
        var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
        _inner.Write(result, typeof(T), LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            _inner.Write(function.Invoke(), LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public void Fatal<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            _inner.Write(function.Invoke(), typeof(T), LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void FatalException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _inner.Write(exception, $"{function.Invoke()}: {exception}", LogLevel.Fatal);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    /// <inheritdoc />
    public void Fatal(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            _inner.Write(exception, function.Invoke(), LogLevel.Fatal);
        }
    }

    private string InvokeStringFormat(IFormatProvider formatProvider, string message, object[] args)
    {
        var sfArgs = new object?[3];
        sfArgs[0] = formatProvider;
        sfArgs[1] = message;
        sfArgs[2] = args;
        return (string?)_stringFormat.Invoke(null, sfArgs) ?? "(null)";
    }
}
