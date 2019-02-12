﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Splat.Ninject.Tests
{
    /// <summary>
    /// View One.
    /// </summary>
    /// <seealso cref="ViewModelOne" />
    public class ViewOne : IViewFor<ViewModelOne>
    {
        /// <inheritdoc />
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ViewModelOne)value;
        }

        /// <inheritdoc />
        public ViewModelOne ViewModel { get; set; }
    }
}
