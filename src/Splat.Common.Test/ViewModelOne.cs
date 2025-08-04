// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Common.Test;

/// <summary>
/// View Model One.
/// </summary>
[ExcludeFromCodeCoverage]
#pragma warning disable CA1515 // Consider making public types internal
public class ViewModelOne : IViewModelOne
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelOne"/> class.
    /// </summary>
    public ViewModelOne()
    {
    }
}
