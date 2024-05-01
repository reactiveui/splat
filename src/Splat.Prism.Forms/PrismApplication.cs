// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Prism;
using Prism.Ioc;

[assembly: Xamarin.Forms.XmlnsDefinition("http://prismlibrary.com", "Splat.Prism.Forms")]

namespace Splat.Prism.Forms;

/// <summary>
/// A application instance which supports Prism types.
/// </summary>
public abstract class PrismApplication : PrismApplicationBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrismApplication"/> class.
    /// </summary>
    /// <param name="initializer">An initializer for initializing the platform.</param>
    protected PrismApplication(IPlatformInitializer? initializer = null)
        : base(initializer)
    {
    }

    /// <inheritdoc/>
    protected override IContainerExtension CreateContainerExtension() => new SplatContainerExtension();
}
