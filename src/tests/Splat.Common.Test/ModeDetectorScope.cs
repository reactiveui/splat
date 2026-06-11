// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the ModeDetector state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new ModeDetectorScope();
///     // Test code here - ModeDetector will be reset to fresh state after the test
/// }
/// </code>
/// </example>
public sealed class ModeDetectorScope : IDisposable
{
    /// <summary>The mode detector and cached result captured on construction and restored on dispose.</summary>
    private readonly (IModeDetector detector, bool? cachedResult) _savedState;

    /// <summary>Initializes a new instance of the <see cref="ModeDetectorScope"/> class. Saves the current ModeDetector state and resets it to default.</summary>
    public ModeDetectorScope()
    {
        _savedState = ModeDetector.GetState();
        ModeDetector.ResetState();
    }

    /// <summary>Restores the ModeDetector to its previous state.</summary>
    public void Dispose() => ModeDetector.RestoreState(_savedState);
}
