// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using Avalonia.Controls;
using ReactiveUI;

namespace Avalonia.ReactiveUI
{
    /// <summary>
    /// A ReactiveUI <see cref="UserControl"/> that implements the <see cref="IViewFor{TViewModel}"/> interface and
    /// will activate your ViewModel automatically if the view model implements <see cref="IActivatableViewModel"/>.
    /// When the DataContext property changes, this class will update the ViewModel property with the new DataContext
    /// value, and vice versa.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel type.</typeparam>
    public class ReactiveUserControl<TViewModel> : UserControl, IViewFor<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// The view model property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
        public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
            .Register<ReactiveUserControl<TViewModel>, TViewModel?>(nameof(ViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveUserControl{TViewModel}"/> class.
        /// </summary>
        public ReactiveUserControl()
        {
            // This WhenActivated block calls ViewModel's WhenActivated
            // block if the ViewModel implements IActivatableViewModel.
            this.WhenActivated(_ => { });
            this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);
        }

        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public TViewModel? ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }

        /// <summary>
        /// Called when the DataContext property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            ViewModel = DataContext as TViewModel;
        }

        private void OnViewModelChanged(object? value)
        {
            if (value == null)
            {
                ClearValue(DataContextProperty);
            }
            else if (DataContext != value)
            {
                DataContext = value;
            }
        }
    }
}
