// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>
/// A scope that saves and restores the PlatformModeDetector state for test isolation.
/// Use in a using statement to ensure proper cleanup.
/// </summary>
/// <example>
/// <code>
/// [Test]
/// public void MyTest()
/// {
///     using var scope = new PlatformModeDetectorScope();
///     // Test code here - PlatformModeDetector will be reset to fresh state after the test
/// }
/// </code>
/// </example>
public sealed class PlatformModeDetectorScope : IDisposable
{
    /// <summary>The platform mode detector captured on construction.</summary>
    private readonly IPlatformModeDetector _savedDetector;

    /// <summary>The cached platform-mode-detection result captured on construction.</summary>
    private readonly bool? _savedCachedResult;

    /// <summary>Initializes a new instance of the <see cref="PlatformModeDetectorScope"/> class. Saves the current PlatformModeDetector state and resets it to default.</summary>
    public PlatformModeDetectorScope()
    {
        (_savedDetector, _savedCachedResult) = PlatformModeDetector.GetState();
        PlatformModeDetector.ResetState();
    }

    /// <summary>Restores the PlatformModeDetector to its previous state.</summary>
    public void Dispose() => PlatformModeDetector.RestoreState((_savedDetector, _savedCachedResult));
}
