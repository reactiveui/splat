// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Common.Test;

/// <summary>
/// View One.
/// </summary>
/// <seealso cref="ViewModelOne" />
[ExcludeFromCodeCoverage]
#pragma warning disable CA1515 // Consider making public types internal
public class ViewOne : IViewFor<ViewModelOne>
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <inheritdoc />
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ViewModelOne?)value;
    }

    /// <inheritdoc />
    public ViewModelOne? ViewModel { get; set; }
}
