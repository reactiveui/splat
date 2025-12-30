// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
///   Specifies that an output is not <see langword="null"/> even if the
///   corresponding type allows it.
///   Modification of Using SimonCropp's polyfill's library.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(
    validOn: AttributeTargets.Field |
             AttributeTargets.Parameter |
             AttributeTargets.Property |
             AttributeTargets.ReturnValue)]
internal sealed class NotNullAttribute : Attribute;
#else
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.NotNullAttribute))]
#endif
