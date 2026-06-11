// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

using Splat.Common.Test;

namespace Splat.Ninject.Tests;

/// <summary>
/// A scope that manages Ninject kernel lifecycle and AppLocator state for test isolation.
/// Ensures proper cleanup of both the kernel and global AppLocator state.
/// </summary>
public sealed class NinjectKernelScope : IDisposable
{
    /// <summary>The AppLocator scope that isolates global locator state for the test.</summary>
    private readonly AppLocatorScope _appLocatorScope;

    /// <summary>The kernels tracked for disposal at the end of the test.</summary>
    private readonly List<IKernel> _kernels = [];

    /// <summary>A value indicating whether this scope has already been disposed.</summary>
    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="NinjectKernelScope"/> class. Creates a fresh AppLocator state for the test.</summary>
    public NinjectKernelScope() => _appLocatorScope = new();

    /// <summary>Tracks a kernel for disposal at the end of the test.</summary>
    /// <param name="kernel">The kernel to track.</param>
    public void TrackKernel(IKernel kernel) => _kernels.Add(kernel);

    /// <summary>Disposes all tracked kernels and restores AppLocator state.</summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        // Dispose all tracked kernels
        foreach (var kernel in _kernels)
        {
            kernel?.Dispose();
        }

        _kernels.Clear();

        // Restore AppLocator state
        _appLocatorScope?.Dispose();

        _disposed = true;
    }
}
