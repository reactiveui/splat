// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a disposable resource that can be checked for disposal status.
/// Based on the System.Reactive.Disposable class.
/// </summary>
internal sealed class BooleanDisposable() : IDisposable
{
    private volatile bool _isDisposed;

    /// <summary>
    /// Gets a value indicating whether the object is disposed.
    /// </summary>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// Sets the status to disposed, which can be observer through the <see cref="IsDisposed"/> property.
    /// </summary>
    public void Dispose() => _isDisposed = true;
}
