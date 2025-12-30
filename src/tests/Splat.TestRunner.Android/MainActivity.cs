// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using Android.App;
using Android.OS;

using Xunit.Runners.UI;
using Xunit.Sdk;

namespace Splat.TestRunner.Android
{
    /// <summary>
    /// Unit Test Runner Activity.
    /// </summary>
    // ReSharper disable UnusedMember.Global
    [Activity(Label = "xUnit Android Runner", MainLauncher = true, Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : RunnerActivity

    // ReSharper restore UnusedMember.Global
    {
        /// <inheritdoc/>
        protected override void OnCreate(Bundle bundle)
        {
            Locator.CurrentMutable.RegisterPlatformBitmapLoader();
            AddTestAssembly(typeof(Splat.Tests.LocatorTests).Assembly);
            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);

            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }
    }
}
