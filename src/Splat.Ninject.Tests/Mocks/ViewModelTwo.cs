// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Splat.Ninject.Tests
{
    /// <summary>
    /// View Model Two.
    /// </summary>
    /// <seealso cref="ReactiveObject" />
    /// <seealso cref="IRoutableViewModel" />
    public class ViewModelTwo : ReactiveObject, IRoutableViewModel
    {
        /// <inheritdoc />
        public string UrlPathSegment { get; }

        /// <inheritdoc />
        public IScreen HostScreen { get; }
    }
}
