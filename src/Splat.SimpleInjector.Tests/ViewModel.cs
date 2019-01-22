// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Splat.Simplnjector
{
    /// <summary>
    /// View Model.
    /// </summary>
    public class ViewModel : ReactiveObject
    {
        private string _text;

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }
    }
}
