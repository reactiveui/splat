// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Splat.Simplnjector
{
    /// <summary>
    /// View.
    /// </summary>
    /// <seealso cref="ReactiveUI.IViewFor{Splat.Simplnjector.ViewModel}" />
    public class View : IViewFor<ViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        public View()
        {
            this.Bind(ViewModel, x => x.Text, x => x.Text);
        }

        /// <inheritdoc />
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ViewModel)value;
        }

        /// <inheritdoc />
        public ViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets text. TextBox.Text for example.
        /// </summary>
        public string Text { get; set; }
    }
}
