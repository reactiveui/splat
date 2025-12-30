// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the BitmapLoader state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new BitmapLoaderScope();
///     // Test code here - BitmapLoader will be reset to fresh state after the test
/// }
/// </code>
/// </example>
public sealed class BitmapLoaderScope : IDisposable
{
    private readonly IBitmapLoader? _savedState;

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapLoaderScope"/> class.
    /// Saves the current BitmapLoader state and resets it to default.
    /// </summary>
    public BitmapLoaderScope()
    {
        _savedState = BitmapLoader.GetState();
        BitmapLoader.ResetState();
    }

    /// <summary>
    /// Restores the BitmapLoader to its previous state.
    /// </summary>
    public void Dispose()
    {
        BitmapLoader.RestoreState(_savedState);
    }
}
