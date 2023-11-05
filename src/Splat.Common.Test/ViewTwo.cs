﻿// Copyright (c) 2023 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Common.Test;

/// <summary>
/// View Two.
/// </summary>
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
