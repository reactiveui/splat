// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    private readonly AppLocatorScope _appLocatorScope;
    private readonly List<IKernel> _kernels = [];
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="NinjectKernelScope"/> class.
    /// Creates a fresh AppLocator state for the test.
    /// </summary>
    public NinjectKernelScope()
    {
        _appLocatorScope = new AppLocatorScope();
    }

    /// <summary>
    /// Tracks a kernel for disposal at the end of the test.
    /// </summary>
    /// <param name="kernel">The kernel to track.</param>
    public void TrackKernel(IKernel kernel)
    {
        _kernels.Add(kernel);
    }

    /// <summary>
    /// Disposes all tracked kernels and restores AppLocator state.
    /// </summary>
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
