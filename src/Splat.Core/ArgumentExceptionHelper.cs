// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Provides utility methods for validating method arguments and throwing appropriate exceptions when argument values do
/// not meet expected conditions.
/// </summary>
/// <remarks>This static helper class centralizes common argument validation patterns, such as checking for null,
/// empty, or out-of-range values, and throws standard .NET exceptions (such as ArgumentNullException,
/// ArgumentException, and ArgumentOutOfRangeException) when validation fails. These methods are intended to simplify
/// and standardize argument checking in internal code. All methods are intended for use within the assembly and are not
/// designed for public API consumption.</remarks>
[ExcludeFromCodeCoverage]
internal static class ArgumentExceptionHelper
{
    /// <summary>
    /// Throws an exception if the specified argument is null.
    /// </summary>
    /// <param name="argument">The object to validate for null. If this value is null, an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter to include in the exception message. This value is typically provided automatically
    /// and can be omitted.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null.</exception>
    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>
    /// Throws an ArgumentNullException if the specified argument is null, using the provided message and parameter
    /// name.
    /// </summary>
    /// <param name="argument">The object to validate as non-null. If this value is null, an exception is thrown.</param>
    /// <param name="message">The message to include in the exception if the argument is null.</param>
    /// <param name="paramName">The name of the parameter to include in the exception. This is automatically supplied by the compiler and should
    /// not typically be specified manually.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null.</exception>
    public static void ThrowIfNullWithMessage([NotNull] object? argument, string message, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName, message);
        }
    }

    /// <summary>
    /// Throws an exception if the specified string argument is null or an empty string.
    /// </summary>
    /// <param name="argument">The string argument to validate. Cannot be null or an empty string.</param>
    /// <param name="paramName">The name of the parameter to include in the exception message. This value is typically provided automatically
    /// and should not be set explicitly in most cases.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> is an empty string.</exception>
    public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (argument.Length == 0)
        {
            throw new ArgumentException("The value cannot be an empty string.", paramName);
        }
    }

    /// <summary>
    /// Throws an exception if the specified string argument is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="argument">The string value to validate. Cannot be null, empty, or consist only of white-space characters.</param>
    /// <param name="paramName">The name of the parameter to include in the exception message. This value is typically provided automatically
    /// and should not be set manually.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> is an empty string or consists only of white-space characters.</exception>
    public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException("The value cannot be an empty string or composed entirely of whitespace.", paramName);
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is negative.
    /// </summary>
    /// <param name="value">The integer value to validate. Must be zero or positive.</param>
    /// <param name="paramName">The name of the parameter being validated. This value is used in the exception message. Optional.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than zero.</exception>
    public static void ThrowIfNegative(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value cannot be negative.");
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is less than or equal to the given comparison value.
    /// </summary>
    /// <param name="value">The value to validate against the comparison value.</param>
    /// <param name="other">The value to compare against. If <paramref name="value"/> is less than or equal to this value, an exception is
    /// thrown.</param>
    /// <param name="paramName">The name of the parameter representing the value being validated. This is used in the exception message.
    /// Optional.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than or equal to <paramref name="other"/>.</exception>
    public static void ThrowIfLessThanOrEqual(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= other)
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be less than or equal to {other}.");
        }
    }

    /// <summary>
    /// Throws an ArgumentException if the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to evaluate. If <see langword="true"/>, an exception is thrown.</param>
    /// <param name="message">The message to include in the exception if the condition is true.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is typically provided automatically by the
    /// compiler.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is <see langword="true"/>.</exception>
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, string message, [CallerArgumentExpression(nameof(condition))] string? paramName = null)
    {
        if (condition)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is zero.
    /// </summary>
    /// <param name="value">The integer value to validate. Must not be zero.</param>
    /// <param name="paramName">The name of the parameter being validated. This value is typically provided automatically and is used in the
    /// exception message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is zero.</exception>
    public static void ThrowIfZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value cannot be zero.");
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is negative or zero.
    /// </summary>
    /// <param name="value">The integer value to validate. Must be greater than zero.</param>
    /// <param name="paramName">The name of the parameter being validated. This value is used in the exception message. Optional.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than or equal to zero.</exception>
    public static void ThrowIfNegativeOrZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value cannot be negative or zero.");
        }
    }

    /// <summary>
    /// Throws an ArgumentOutOfRangeException if the specified value is equal to the provided comparison value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="value">The value to test for equality against the comparison value.</param>
    /// <param name="other">The value to compare with the specified value.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is typically provided automatically and should
    /// not be set manually.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if value is equal to other.</exception>
    public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IEquatable<T>
    {
        if (value.Equals(other))
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be equal to {other}.");
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is not equal to the expected value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="value">The value to validate for equality.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is typically provided automatically and should
    /// not be set explicitly.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not equal to <paramref name="other"/>.</exception>
    public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IEquatable<T>
    {
        if (!value.Equals(other))
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value must be equal to {other}.");
        }
    }

    /// <summary>
    /// Throws an ArgumentOutOfRangeException if the specified value is greater than the given comparison value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate against the comparison value.</param>
    /// <param name="other">The value to compare against. If value is greater than this, an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is optional and is automatically provided by the
    /// compiler.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if value is greater than other.</exception>
    public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) > 0)
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be greater than {other}.");
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is greater than or equal to a given comparison value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate against the comparison value.</param>
    /// <param name="other">The value to compare with <paramref name="value"/>.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is optional and is automatically provided by the
    /// compiler.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than or equal to <paramref name="other"/>.</exception>
    public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) >= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be greater than or equal to {other}.");
        }
    }

    /// <summary>
    /// Throws an exception if a specified value is less than a given minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate against the minimum value.</param>
    /// <param name="other">The minimum allowable value. If <paramref name="value"/> is less than this value, an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter that caused the exception. This value is typically provided automatically and should
    /// not be set manually.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than <paramref name="other"/>.</exception>
    public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be less than {other}.");
        }
    }

    /// <summary>
    /// Throws an ArgumentOutOfRangeException if the specified value is less than or equal to the comparison value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">The name of the parameter representing the value being validated. This is automatically provided by the
    /// compiler.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if value is less than or equal to other.</exception>
    public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, $"The value cannot be less than or equal to {other}.");
        }
    }
}
