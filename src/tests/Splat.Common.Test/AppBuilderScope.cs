// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Builder;

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the AppBuilder state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new AppBuilderScope();
///     // Test code here - AppBuilder will be reset to fresh state after the test
/// }
/// </code>
/// </example>
public sealed class AppBuilderScope : IDisposable
{
    private readonly (bool hasBeenBuilt, bool usingBuilder) _savedState;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBuilderScope"/> class.
    /// Saves the current AppBuilder state and resets it to default.
    /// </summary>
    public AppBuilderScope()
    {
        _savedState = AppBuilder.GetState();
        AppBuilder.ResetState();
    }

    /// <summary>
    /// Restores the AppBuilder to its previous state.
    /// </summary>
    public void Dispose() => AppBuilder.RestoreState(_savedState);
}
