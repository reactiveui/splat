// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Common.Test;

/// <summary>View Two.</summary>
/// <seealso cref="ViewModelTwo" />
[ExcludeFromCodeCoverage]
public class ViewTwo : IViewFor<ViewModelTwo>
{
    /// <inheritdoc />
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ViewModelTwo?)value;
    }

    /// <inheritdoc />
    public ViewModelTwo? ViewModel { get; set; }
}
