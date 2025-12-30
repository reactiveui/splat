// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !NET5_0_OR_GREATER

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicates that a parameter captures the expression passed for another parameter as a string.
/// Modification of Using SimonCropp's polyfill's library.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class DoesNotReturnIfAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="DoesNotReturnIfAttribute"/> class..</summary>
    /// <param name="parameterValue">
    /// The condition parameter value. Code after the method will be considered unreachable by diagnostics if the argument to
    /// the associated parameter matches this value.
    /// </param>
    public DoesNotReturnIfAttribute(bool parameterValue) => ParameterValue = parameterValue;

    /// <summary>Gets a value indicating whether the condition parameter value.</summary>
    public bool ParameterValue { get; }
}

#else
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(DoesNotReturnIfAttribute))]
#endif
