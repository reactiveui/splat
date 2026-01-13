// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// This is a test view relating to issue #889.
/// It's intended to ensure that view registration by different DI\IoC implementations
/// does not create an instance at the point of registration.
/// </summary>
public sealed class ViewThatShouldNotLoad : IViewFor<ViewModelOne>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewThatShouldNotLoad"/> class.
    /// </summary>
    public ViewThatShouldNotLoad() => throw new InvalidOperationException("This view should not be created.");

    /// <inheritdoc />
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ViewModelOne?)value;
    }

    /// <inheritdoc />
    public ViewModelOne? ViewModel { get; set; }
}
