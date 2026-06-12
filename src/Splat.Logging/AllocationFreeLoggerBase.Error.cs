// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;

namespace Splat;

/// <summary>Provides the allocation-free <c>Error</c> logging overloads for <see cref="AllocationFreeLoggerBase"/>.</summary>
public abstract partial class AllocationFreeLoggerBase
{
    /// <inheritdoc />
    public virtual void Error<TArgument>([Localizable(false)] string messageFormat, TArgument argument)
    {
        if (!IsErrorEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument), LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument)
    {
        if (!IsErrorEnabled)
        {
            return;
        }

        inner.Write(
            exception,
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument),
            LogLevel.Error);
    }

    /// <inheritdoc />
    public virtual void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2)
    {
        if (!IsErrorEnabled)
        {
            return;
        }

        inner.Write(string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2), LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2)
    {
        if (!IsErrorEnabled)
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
            LogLevel.Error);
    }

    /// <inheritdoc />
    public virtual void Error<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsErrorEnabled)
        {
            return;
        }

        inner.Write(
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3),
            LogLevel.Error);
    }

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
    {
        if (!IsErrorEnabled)
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
            LogLevel.Error);
    }

    /// <inheritdoc />
    public virtual void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
    {
        if (!IsErrorEnabled)
        {
            return;
        }

        inner.Write(
            string.Format(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4),
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
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
        if (!IsErrorEnabled)
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
            LogLevel.Error);
    }
}
