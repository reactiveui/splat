// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;

namespace Avalonia.ReactiveUI
{
    /// <summary>
    /// Determines when Avalonia IVisuals get activated.
    /// </summary>
    public class AvaloniaActivationForViewFetcher : IActivationForViewFetcher
    {
        /// <summary>
        /// Returns affinity for view.
        /// </summary>
        /// <param name="view">The type for the View.</param>
        /// <returns>
        /// The affinity value which is equal to 0 or above.
        /// </returns>
        public int GetAffinityForView(Type view) => typeof(Visual).IsAssignableFrom(view) ? 10 : 0;

        /// <summary>
        /// Returns activation observable for activatable Avalonia view.
        /// </summary>
        /// <param name="view">The view to get the activation observable for.</param>
        /// <returns>
        /// A Observable which will returns if Activation was successful.
        /// </returns>
        public IObservable<bool> GetActivationForView(IActivatableView view) =>
            view is not Visual visual
                ? Observable.Return(false)
                : view switch
                {
                    Control control => GetActivationForControl(control),
                    _ => GetActivationForVisual(visual)
                };

        /// <summary>
        /// Listens to Loaded and Unloaded
        /// events for Avalonia Control.
        /// </summary>
        private static IObservable<bool> GetActivationForControl(Control control)
        {
            var controlLoaded = Observable
                .FromEventPattern<RoutedEventArgs>(
                    x => control.Loaded += x,
                    x => control.Loaded -= x)
                .Select(_ => true);
            var controlUnloaded = Observable
                .FromEventPattern<RoutedEventArgs>(
                    x => control.Unloaded += x,
                    x => control.Unloaded -= x)
                .Select(_ => false);
            return controlLoaded
                .Merge(controlUnloaded)
                .DistinctUntilChanged();
        }

        /// <summary>
        /// Listens to AttachedToVisualTree and DetachedFromVisualTree
        /// events for Avalonia IVisuals.
        /// </summary>
        private static IObservable<bool> GetActivationForVisual(Visual visual)
        {
            var visualLoaded = Observable
                .FromEventPattern<VisualTreeAttachmentEventArgs>(
                    x => visual.AttachedToVisualTree += x,
                    x => visual.AttachedToVisualTree -= x)
                .Select(_ => true);
            var visualUnloaded = Observable
                .FromEventPattern<VisualTreeAttachmentEventArgs>(
                    x => visual.DetachedFromVisualTree += x,
                    x => visual.DetachedFromVisualTree -= x)
                .Select(_ => false);
            return visualLoaded
                .Merge(visualUnloaded)
                .DistinctUntilChanged();
        }
    }
}
