// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

[assembly: Xamarin.Forms.XmlnsDefinition("http://prismlibrary.com", "Splat.Prism.Forms")]

namespace Splat.Prism.Forms;

/// <summary>
/// A application instance which supports Prism types.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PrismApplication"/> class.
/// </remarks>
/// <param name="initializer">An initializer for initializing the platform.</param>
public abstract class PrismApplication(IPlatformInitializer? initializer = null) : PrismApplicationBase(initializer)
{
    /// <inheritdoc/>
    protected override IContainerExtension CreateContainerExtension() => new SplatContainerExtension();
}
