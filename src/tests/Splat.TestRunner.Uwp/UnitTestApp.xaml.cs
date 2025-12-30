// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;
using Xunit.Runners.UI;

namespace Splat.TestRunner.Uwp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : RunnerApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnInitializeRunner()
        {
            InitializeRunner();
        }

        private void InitializeRunner()
        {
            AddTestAssembly(typeof(Splat.Tests.BitmapLoaderTests).GetTypeInfo().Assembly);
        }
    }
}
