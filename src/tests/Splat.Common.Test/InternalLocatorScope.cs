// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the AppLocator.InternalLocator state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new InternalLocatorScope();
///     var locator = scope.Locator;
///     // Test code here - AppLocator.InternalLocator will be reset after the test
/// }
/// </code>
/// </example>
internal sealed class InternalLocatorScope : IDisposable
{
    private readonly InternalLocator _savedLocator;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalLocatorScope"/> class.
    /// Saves the current AppLocator.InternalLocator state and creates a new fresh instance.
    /// </summary>
    public InternalLocatorScope()
    {
        _savedLocator = AppLocator.InternalLocator;
        Locator = new InternalLocator();
        Locator.CurrentMutable.InitializeSplat();
        AppLocator.InternalLocator = Locator;
    }

    /// <summary>
    /// Gets the InternalLocator instance for this scope.
    /// </summary>
    public InternalLocator Locator { get; }

    /// <summary>
    /// Restores the AppLocator.InternalLocator to its previous state.
    /// </summary>
    public void Dispose()
    {
        Locator.Dispose();
        AppLocator.InternalLocator = _savedLocator;
    }
}
