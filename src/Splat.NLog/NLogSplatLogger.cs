// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Splat.NLog
{
    /// <summary>
    /// NLog Logger taken from ReactiveUI 5.
    /// </summary>
    [DebuggerDisplay("Name={_inner.Name} Level={Level}")]
    internal sealed class NLogSplatLogger : IFullLogger
    {
        private readonly global::NLog.Logger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogSplatLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual nlog logger.</param>
        /// <exception cref="ArgumentNullException">NLog logger not passed.</exception>
        public NLogSplatLogger(global::NLog.Logger inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets or sets the logging level.
        /// </summary>
        public LogLevel Level { get; set; }

        public bool IsDebugEnabled => _inner.IsDebugEnabled;

        public bool IsInfoEnabled => _inner.IsInfoEnabled;

        public bool IsWarnEnabled => _inner.IsWarnEnabled;

        public bool IsErrorEnabled => _inner.IsErrorEnabled;

        public bool IsFatalEnabled => _inner.IsFatalEnabled;

        /// <summary>
        /// Writes a message at the specified log level.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="logLevel">The log level to write the message at.</param>
        public void Write(string message, LogLevel logLevel)
        {
            _inner.Log(RxUitoNLogLevel(logLevel), message);
        }

        public void Write(string message, Type type, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a debug message using a generic argument.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The argument to log.</param>
        public void Debug<T>(T value)
        {
            _inner.Debug(value);
        }

        /// <summary>
        /// Writes a debug message using a generic argument.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="value">The argument to log.</param>
        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Debug(formatProvider, value);
        }

        /// <summary>
        /// Logs a debug message and exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception that has occurred.</param>
        public void DebugException(string message, Exception exception)
        {
            _inner.Debug(exception, message);
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Debug(formatProvider, message, args);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(string message)
        {
            _inner.Debug(message);
        }

        public void Debug<T>(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Debug(string message, params object[] args)
        {
            _inner.Debug(message, args);
        }

        public void Debug<T>(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Debug(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Debug<TArgument>(string message, TArgument argument)
        {
            _inner.Debug(message, argument);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Debug(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Debug(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Debug(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Debug(message, argument1, argument2, argument3);
        }

        public void Info<T>(T value)
        {
            _inner.Info(value);
        }

        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Info(formatProvider, value);
        }

        /// <summary>
        /// Logs an information message and exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception that has occurred.</param>
        public void InfoException(string message, Exception exception)
        {
            _inner.Info(exception, message);
        }

        /// <summary>
        /// Logs an information message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Info(formatProvider, message, args);
        }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(string message)
        {
            _inner.Info(message);
        }

        public void Info<T>(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs an information message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Info(string message, params object[] args)
        {
            _inner.Info(message, args);
        }

        public void Info<T>(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Info(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Info<TArgument>(string message, TArgument argument)
        {
            _inner.Info(message, argument);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Info(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Info(message, argument1, argument2);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Info(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Info(message, argument1, argument2, argument3);
        }

        public void Warn<T>(T value)
        {
            _inner.Warn(value);
        }

        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Warn(formatProvider, value);
        }

        /// <summary>
        /// Logs a warning message and exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception that has occurred.</param>
        public void WarnException(string message, Exception exception)
        {
            _inner.Warn(exception, message);
        }

        /// <summary>
        /// Logs a warning message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Warn(formatProvider, message, args);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(string message)
        {
            _inner.Warn(message);
        }

        public void Warn<T>(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Warn(string message, params object[] args)
        {
            _inner.Warn(message, args);
        }

        public void Warn<T>(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Warn(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Warn<TArgument>(string message, TArgument argument)
        {
            _inner.Warn(message, argument);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Warn(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Warn(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Warn(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Warn(message, argument1, argument2, argument3);
        }

        public void Error<T>(T value)
        {
            _inner.Error(value);
        }

        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Error(formatProvider, value);
        }

        /// <summary>
        /// Logs an error message and exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception that has occurred.</param>
        public void ErrorException(string message, Exception exception)
        {
            _inner.Error(exception, message);
        }

        /// <summary>
        /// Logs an error message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Error(formatProvider, message, args);
        }

        /// <summary>
        /// Logs a error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(string message)
        {
            _inner.Error(message);
        }

        public void Error<T>(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Error(string message, params object[] args)
        {
            _inner.Error(message, args);
        }

        public void Error<T>(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Error(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Error<TArgument>(string message, TArgument argument)
        {
            _inner.Error(message, argument);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Error(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Error(message, argument1, argument2);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Error(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Error(message, argument1, argument2, argument3);
        }

        public void Fatal<T>(T value)
        {
            _inner.Fatal(value);
        }

        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            _inner.Fatal(formatProvider, value);
        }

        /// <summary>
        /// Logs a fatal message and exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception that has occurred.</param>
        public void FatalException(string message, Exception exception)
        {
            _inner.Fatal(exception, message);
        }

        /// <summary>
        /// Logs a fatal message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            _inner.Fatal(formatProvider, message, args);
        }

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(string message)
        {
            _inner.Fatal(message);
        }

        public void Fatal<T>(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a fatal message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The argument array to log.</param>
        public void Fatal(string message, params object[] args)
        {
            _inner.Fatal(message, args);
        }

        public void Fatal<T>(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            _inner.Fatal(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument">The type of argument.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument">The argument to log.</param>
        public void Fatal<TArgument>(string message, TArgument argument)
        {
            _inner.Fatal(message, argument);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Fatal(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Fatal(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="formatProvider">The format provider to use for the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Fatal(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments.
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1.</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2.</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="argument1">The first argument to log.</param>
        /// <param name="argument2">The second argument to log.</param>
        /// <param name="argument3">The third argument to log.</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Fatal(message, argument1, argument2, argument3);
        }

        private static global::NLog.LogLevel RxUitoNLogLevel(LogLevel logLevel)
        {
            var mappings = new[]
                               {
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Debug, global::NLog.LogLevel.Debug),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Info, global::NLog.LogLevel.Info),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Warn, global::NLog.LogLevel.Warn),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Error, global::NLog.LogLevel.Error),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Fatal, global::NLog.LogLevel.Fatal)
                               };

            return mappings.First(x => x.Item1 == logLevel).Item2;
        }
    }
}
