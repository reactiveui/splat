// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

[assembly: Xamarin.Forms.XmlnsDefinition("http://prismlibrary.com", "Splat.Prism.Forms")]

namespace Splat.Prism.Forms;

/// <summary>
/// Provides a base class for Prism applications using the Splat dependency injection container.
/// </summary>
/// <remarks>This class integrates the Splat container with the Prism application framework. Inherit from this
/// class to create a Prism application that uses Splat for dependency injection.</remarks>
/// <param name="initializer">An optional platform-specific initializer that can be used to register platform services or perform
/// platform-specific setup. May be null if no platform-specific initialization is required.</param>
public abstract class PrismApplication(IPlatformInitializer? initializer = null) : PrismApplicationBase(initializer)
{
    /// <inheritdoc/>
    protected override IContainerExtension CreateContainerExtension() => new SplatContainerExtension();
}
