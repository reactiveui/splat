// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Splat;

/// <summary>
/// Base class for a logger the provides allocation free logging.
/// </summary>
/// <seealso cref="IAllocationFreeLogger" />
/// <remarks>
/// Initializes a new instance of the <see cref="AllocationFreeLoggerBase"/> class.
/// </remarks>
/// <param name="inner">The <see cref="Splat.ILogger" /> to wrap in this class.</param>
[SuppressMessage("Naming", "CA1716: Do not use built in identifiers", Justification = "Deliberate usage")]
public abstract class AllocationFreeLoggerBase(ILogger inner) : IAllocationFreeLogger
{
    /// <inheritdoc />
    public LogLevel Level => inner.Level;

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
    public virtual void Debug<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (IsDebugEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (IsDebugEnabled)
        {
            inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (IsDebugEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (IsDebugEnabled)
        {
            inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsDebugEnabled)
        {
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
                LogLevel.Debug);
        }
    }

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsDebugEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (IsDebugEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (IsDebugEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
    {
        if (IsDebugEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        Exception exception,
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
            inner.Write(
                exception,
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
    public virtual void Info<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (IsInfoEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Info<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument),
                LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public virtual void Info<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (IsInfoEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2),
                LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public virtual void Info<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
                LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
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
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                LogLevel.Info);
        }
    }

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3,
                    argument4),
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
    {
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
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
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
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
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9,
        TArgument10>(
        Exception exception,
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
        if (IsInfoEnabled)
        {
            inner.Write(
                exception,
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
    public virtual void Warn<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (IsWarnEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Warn<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (IsWarnEnabled)
        {
            inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public virtual void Warn<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (IsWarnEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (IsWarnEnabled)
        {
            inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public virtual void Warn<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsWarnEnabled)
        {
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
                LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
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
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                LogLevel.Warn);
        }
    }

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3,
                    argument4),
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
    {
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
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
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
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
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9,
        TArgument10>(
        Exception exception,
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
        if (IsWarnEnabled)
        {
            inner.Write(
                exception,
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
    public virtual void Error<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (IsErrorEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument),
                LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public virtual void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (IsErrorEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2),
                LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public virtual void Error<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
                LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3),
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
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                LogLevel.Error);
        }
    }

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3,
                    argument4),
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
    {
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
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
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
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
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        Exception exception,
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
        if (IsErrorEnabled)
        {
            inner.Write(
                exception,
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
    public virtual void Fatal<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (IsFatalEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument),
                LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public virtual void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (IsFatalEnabled)
        {
            inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2),
                LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public virtual void Fatal<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
                LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3),
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
            inner.Write(
                string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
                LogLevel.Fatal);
        }
    }

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
                string.Format(
                    CultureInfo.InvariantCulture,
                    messageFormat,
                    argument1,
                    argument2,
                    argument3,
                    argument4),
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
    {
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
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
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
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
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
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
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
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
            inner.Write(
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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9,
        TArgument10>(
        Exception exception,
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
        if (IsFatalEnabled)
        {
            inner.Write(
                exception,
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
    public void Write([Localizable(false)] string message, LogLevel logLevel) => inner.Write(message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => inner.Write(exception, message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write(message, type, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write(exception, message, type, logLevel);
}
