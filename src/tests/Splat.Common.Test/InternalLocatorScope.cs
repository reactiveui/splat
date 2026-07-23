// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
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
    /// <summary>The internal locator captured on construction and restored on dispose.</summary>
    private readonly InternalLocator _savedLocator;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalLocatorScope"/> class.
    /// Saves the current AppLocator.InternalLocator state and creates a new fresh instance.
    /// </summary>
    public InternalLocatorScope()
    {
        _savedLocator = AppLocator.InternalLocator;
        Locator = new();
        Locator.CurrentMutable.InitializeSplat();
        AppLocator.InternalLocator = Locator;
    }

    /// <summary>Gets the InternalLocator instance for this scope.</summary>
    internal InternalLocator Locator { get; }

    /// <summary>Restores the AppLocator.InternalLocator to its previous state.</summary>
    public void Dispose()
    {
        Locator.Dispose();
        AppLocator.InternalLocator = _savedLocator;
    }
}
