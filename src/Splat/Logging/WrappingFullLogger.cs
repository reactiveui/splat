// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;

namespace Splat
{
    /// <summary>
    /// A full logger which wraps a <see cref="ILogger"/>.
    /// </summary>
    public class WrappingFullLogger : IFullLogger
    {
        private readonly ILogger _inner;
        private readonly string _prefix;
        private readonly MethodInfo _stringFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingFullLogger"/> class.
        /// </summary>
        /// <param name="inner">The <see cref="ILogger"/> to wrap in this class.</param>
        /// <param name="callingType">The type which will be calling this logger.</param>
        public WrappingFullLogger(ILogger inner, Type callingType)
        {
            _inner = inner;
            _prefix = string.Format(CultureInfo.InvariantCulture, "{0}: ", callingType.Name);

            _stringFormat = typeof(string).GetMethod("Format", new[] { typeof(IFormatProvider), typeof(string), typeof(object[]) });
            Contract.Requires(inner != null);
            Contract.Requires(_stringFormat != null);
        }

        /// <inheritdoc />
        public LogLevel Level
        {
            get => _inner.Level;
            set => _inner.Level = value;
        }

        /// <inheritdoc />
        public bool IsDebugEnabled => Level <= LogLevel.Debug;

        /// <inheritdoc />
        public bool IsInfoEnabled => Level <= LogLevel.Info;

        /// <inheritdoc />
        public bool IsWarnEnabled => Level <= LogLevel.Warn;

        /// <inheritdoc />
        public bool IsErrorEnabled => Level <= LogLevel.Error;

        /// <inheritdoc />
        public bool IsFatalEnabled => Level <= LogLevel.Fatal;

        /// <inheritdoc />
        public void Debug<T>(T value)
        {
            _inner.Write(_prefix + value, LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Write(string.Format(formatProvider, "{0}{1}", _prefix, value), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void DebugException(string message, Exception exception)
        {
            _inner.Write($"{_prefix}{message}: {exception}", LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
            var result = InvokeStringFormat(formatProvider, message, args);

            _inner.Write(_prefix + result, LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            _inner.Write(_prefix + message, LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<T>(string message)
        {
            _inner.Write(message, typeof(T), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(_prefix + result, LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<T>(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(result, typeof(T), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument>(string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3), LogLevel.Debug);
        }

        /// <inheritdoc />
        public void Info<T>(T value)
        {
            _inner.Write(_prefix + value, LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Write(string.Format(formatProvider, "{0}{1}", _prefix, value), LogLevel.Info);
        }

        /// <inheritdoc />
        public void InfoException(string message, Exception exception)
        {
            _inner.Write($"{_prefix}{message}: {exception}", LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            var result = InvokeStringFormat(formatProvider, message, args);
            _inner.Write(_prefix + result, LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            _inner.Write(_prefix + message, LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<T>(string message)
        {
            _inner.Write(message, typeof(T), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(_prefix + result, LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<T>(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(result, typeof(T), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument>(string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3), LogLevel.Info);
        }

        /// <inheritdoc />
        public void Warn<T>(T value)
        {
            _inner.Write(_prefix + value, LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Write(string.Format(formatProvider, "{0}{1}", _prefix, value), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void WarnException(string message, Exception exception)
        {
            _inner.Write($"{_prefix}{message}: {exception}", LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            var result = InvokeStringFormat(formatProvider, message, args);
            _inner.Write(_prefix + result, LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
            _inner.Write(_prefix + message, LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<T>(string message)
        {
            _inner.Write(message, typeof(T), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(_prefix + result, LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<T>(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(result, typeof(T), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument>(string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3), LogLevel.Warn);
        }

        /// <inheritdoc />
        public void Error<T>(T value)
        {
            _inner.Write(_prefix + value, LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Write(string.Format(formatProvider, "{0}{1}", _prefix, value), LogLevel.Error);
        }

        /// <inheritdoc />
        public void ErrorException(string message, Exception exception)
        {
            _inner.Write($"{_prefix}{message}: {exception}", LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            var result = InvokeStringFormat(formatProvider, message, args);
            _inner.Write(_prefix + result, LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            _inner.Write(_prefix + message, LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<T>(string message)
        {
            _inner.Write(message, typeof(T), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(_prefix + result, LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<T>(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(result, typeof(T), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument>(string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3), LogLevel.Error);
        }

        /// <inheritdoc />
        public void Fatal<T>(T value)
        {
            _inner.Write(_prefix + value, LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Write(string.Format(formatProvider, "{0}{1}", _prefix, value), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void FatalException(string message, Exception exception)
        {
            _inner.Write($"{_prefix}{message}: {exception}", LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            var result = InvokeStringFormat(formatProvider, message, args);
            _inner.Write(_prefix + result, LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal(string message)
        {
            _inner.Write(_prefix + message, LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<T>(string message)
        {
            _inner.Write(message, typeof(T), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(_prefix + result, LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<T>(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(result, typeof(T), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument>(string message, TArgument argument)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(_prefix + string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3), LogLevel.Fatal);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            _inner.Write(message, logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _inner.Write(message, type, logLevel);
        }

        private string InvokeStringFormat(IFormatProvider formatProvider, string message, object[] args)
        {
            var sfArgs = new object[3];
            sfArgs[0] = formatProvider;
            sfArgs[1] = message;
            sfArgs[2] = args;
            return (string)_stringFormat.Invoke(null, sfArgs);
        }
    }
}
