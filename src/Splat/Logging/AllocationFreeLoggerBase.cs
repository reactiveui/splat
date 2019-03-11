// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Base class for a logger the provides allocation free logging.
    /// </summary>
    /// <seealso cref="IAllocationFreeLogger" />
    public class AllocationFreeLoggerBase : IAllocationFreeLogger
    {
        private readonly ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationFreeLoggerBase"/> class.
        /// </summary>
        /// <param name="inner">The <see cref="T:Splat.ILogger" /> to wrap in this class.</param>
        public AllocationFreeLoggerBase(ILogger inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public LogLevel Level => _inner.Level;

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
        public virtual void Debug<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3>(
            [Localizable(false)] string message,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
            string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
            string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
            string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
            string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9,
            TArgument10 argument10)
        {
            if (IsDebugEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9,
                        argument10),
                    LogLevel.Debug);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3>(
            [Localizable(false)] string message,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9,
            TArgument10>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9,
            TArgument10 argument10)
        {
            if (IsInfoEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9,
                        argument10),
                    LogLevel.Info);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3>(
            [Localizable(false)] string message,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9,
            TArgument10>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9,
            TArgument10 argument10)
        {
            if (IsWarnEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9,
                        argument10),
                    LogLevel.Warn);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3>(
            [Localizable(false)] string message,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9,
            TArgument10 argument10)
        {
            if (IsErrorEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9,
                        argument10),
                    LogLevel.Error);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument), LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(string.Format(CultureInfo.InvariantCulture, message, argument1, argument2), LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3>(
            [Localizable(false)] string message,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
            [Localizable(false)] string messageFormat,
            TArgument1 argument1,
            TArgument2 argument2,
            TArgument3 argument3,
            TArgument4 argument4,
            TArgument5 argument5,
            TArgument6 argument6,
            TArgument7 argument7,
            TArgument8 argument8,
            TArgument9 argument9,
            TArgument10 argument10)
        {
            if (IsFatalEnabled)
            {
                _inner.Write(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        argument1,
                        argument2,
                        argument3,
                        argument4,
                        argument5,
                        argument6,
                        argument7,
                        argument8,
                        argument9,
                        argument10),
                    LogLevel.Fatal);
            }
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            _inner.Write(message, logLevel);
        }

        /// <inheritdoc />
        public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
        {
            _inner.Write(exception, message, logLevel);
        }

        /// <inheritdoc />
        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _inner.Write(message, type, logLevel);
        }

        /// <inheritdoc />
        public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            _inner.Write(exception, message, type, logLevel);
        }
    }
}
