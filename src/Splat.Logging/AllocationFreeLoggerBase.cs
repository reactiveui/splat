// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Splat;

/// <summary>Base class for a logger the provides allocation free logging.</summary>
/// <seealso cref="IAllocationFreeLogger" />
/// <remarks>
/// Initializes a new instance of the <see cref="AllocationFreeLoggerBase"/> class.
/// </remarks>
/// <param name="inner">The <see cref="ILogger" /> to wrap in this class.</param>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Existing API")]
public abstract partial class AllocationFreeLoggerBase(ILogger inner) : IAllocationFreeLogger
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
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Debug);
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Debug);
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(exception, string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Debug);
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
            LogLevel.Debug);
    }

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

        inner.Write(
            exception,
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
            LogLevel.Debug);
    }

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

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

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

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

    /// <inheritdoc />
    public virtual void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
    {
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
        if (!IsDebugEnabled)
        {
            return;
        }

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
