// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the AppLocator state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new AppLocatorScope();
///     // Test code here - AppLocator will be reset to a fresh state after the test
/// }
/// </code>
/// </example>
public sealed class AppLocatorScope : IDisposable
{
    private readonly IDependencyResolver _savedResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppLocatorScope"/> class.
    /// Saves the current AppLocator state and creates a new fresh instance.
    /// </summary>
    public AppLocatorScope()
    {
        _savedResolver = AppLocator.GetLocator();
        var newResolver = new InstanceGenericFirstDependencyResolver();
        newResolver.InitializeSplat();
        AppLocator.SetLocator(newResolver);
    }

    /// <summary>
    /// Restores the AppLocator to its previous state.
    /// </summary>
    public void Dispose() => AppLocator.SetLocator(_savedResolver);
}
