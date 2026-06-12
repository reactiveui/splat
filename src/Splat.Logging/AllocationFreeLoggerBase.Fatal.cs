// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;

namespace Splat;

/// <summary>Provides the allocation-free <c>Fatal</c> logging overloads for <see cref="AllocationFreeLoggerBase"/>.</summary>
public abstract partial class AllocationFreeLoggerBase
{
    /// <inheritdoc />
    public virtual void Fatal<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(
            exception,
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument),
            LogLevel.Fatal);
    }

    /// <inheritdoc />
    public virtual void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(
            exception,
            string.Format(
                CultureInfo.InvariantCulture,
                messageFormat,
                argument1,
                argument2),
            LogLevel.Fatal);
    }

    /// <inheritdoc />
    public virtual void Fatal<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
            LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsFatalEnabled)
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
                argument3),
            LogLevel.Fatal);
    }

    /// <inheritdoc />
    public virtual void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (!IsFatalEnabled)
        {
            return;
        }

        inner.Write(
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
    }

    /// <inheritdoc />
    public void Fatal<
        TArgument1,
        TArgument2,
        TArgument3,
        TArgument4,
        TArgument5,
        TArgument6,
        TArgument7,
        TArgument8,
        TArgument9,
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
        if (!IsFatalEnabled)
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
            LogLevel.Fatal);
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
