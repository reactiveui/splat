// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace Splat.Logging
{
    /// <summary>
    /// A full logger which used by the default static logger to allow capture of .NET framework caller data. Wraps a <see cref="IFullLogger"/>.
    /// </summary>
    public sealed class StaticFullLogger : IStaticFullLogger
    {
        private readonly IFullLogger _fulllogger;

        public StaticFullLogger(IFullLogger fullLogger)
        {
            _fulllogger = fullLogger ?? throw new ArgumentNullException(nameof(fullLogger));
        }

        /// <inheritdoc/>
        public void Debug<T>(T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Debug<T>(IFormatProvider formatProvider, T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Debug(Exception exception, string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Debug(string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Debug<T>(string message, string callerMemberName = null)
        {
            _fulllogger.Debug<T>(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, string callerMemberName = null)
        {
            _fulllogger.Debug(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument);
        }

        /// <inheritdoc/>
        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, string callerMemberName = null)
        {
            _fulllogger.Debug(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2);
        }

        /// <inheritdoc/>
        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, string callerMemberName = null)
        {
            _fulllogger.Debug(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2,
                argument3);
        }

        /// <inheritdoc/>
        public void Info<T>(T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Info<T>(IFormatProvider formatProvider, T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Info(Exception exception, string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Info(string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Info<T>(string message, string callerMemberName = null)
        {
            _fulllogger.Info<T>(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, string callerMemberName = null)
        {
            _fulllogger.Info(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument);
        }

        /// <inheritdoc/>
        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, string callerMemberName = null)
        {
            _fulllogger.Info(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2);
        }

        /// <inheritdoc/>
        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, string callerMemberName = null)
        {
            _fulllogger.Info(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2,
                argument3);
        }

        /// <inheritdoc/>
        public void Warn<T>(T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Warn<T>(IFormatProvider formatProvider, T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Warn(Exception exception, string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Warn(string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Warn<T>(string message, string callerMemberName = null)
        {
            _fulllogger.Warn<T>(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, string callerMemberName = null)
        {
            _fulllogger.Warn(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument);
        }

        /// <inheritdoc/>
        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, string callerMemberName = null)
        {
            _fulllogger.Warn(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2);
        }

        /// <inheritdoc/>
        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, string callerMemberName = null)
        {
            _fulllogger.Warn(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2,
                argument3);
        }

        /// <inheritdoc/>
        public void Error<T>(T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Error<T>(IFormatProvider formatProvider, T value, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Error(Exception exception, string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Error(string message, string callerMemberName = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Error<T>(string message, string callerMemberName = null)
        {
            _fulllogger.Error<T>(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, string callerMemberName = null)
        {
            _fulllogger.Error(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument);
        }

        /// <inheritdoc/>
        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, string callerMemberName = null)
        {
            _fulllogger.Error(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2);
        }

        /// <inheritdoc/>
        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, string callerMemberName = null)
        {
            _fulllogger.Error(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2,
                argument3);
        }

        /// <inheritdoc/>
        public void Fatal<T>(T value, string callerMemberName = null)
        {
            _fulllogger.Fatal<T>(
                callerMemberName,
                value);
        }

        /// <inheritdoc/>
        public void Fatal<T>(IFormatProvider formatProvider, T value, string callerMemberName = null)
        {
            _fulllogger.Fatal(
                formatProvider,
                callerMemberName,
                value);
        }

        /// <inheritdoc/>
        public void Fatal(Exception exception, string message, string callerMemberName = null)
        {
            _fulllogger.Fatal(exception, GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Fatal(string message, string callerMemberName = null)
        {
            _fulllogger.Fatal(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Fatal<T>(string message, string callerMemberName = null)
        {
            _fulllogger.Fatal<T>(GetSuffixedCallerData(message, callerMemberName));
        }

        /// <inheritdoc/>
        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, string callerMemberName = null)
        {
            _fulllogger.Fatal(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument);
        }

        /// <inheritdoc/>
        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, string callerMemberName = null)
        {
            _fulllogger.Fatal(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2);
        }

        /// <inheritdoc/>
        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, string callerMemberName = null)
        {
            _fulllogger.Fatal(
                formatProvider,
                GetSuffixedCallerData(message, callerMemberName),
                argument1,
                argument2,
                argument3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetSuffixedCallerData(string message, string callerMemberName)
        {
            return message + " (" + callerMemberName + ")";
        }
    }
}
