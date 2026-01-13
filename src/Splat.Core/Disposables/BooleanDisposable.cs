// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a disposable object that signals whether it has been disposed.
/// </summary>
/// <remarks>This type is typically used to provide a simple, thread-safe disposable flag for resource management
/// scenarios. Once disposed, the state is observable through the IsDisposed property. This class is not intended for
/// use with unmanaged resources.</remarks>
internal sealed class BooleanDisposable() : IDisposable
{
    private volatile bool _isDisposed;

    /// <summary>
    /// Gets a value indicating whether the object is disposed.
    /// </summary>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// Sets the status to disposed, which can be observed through the <see cref="IsDisposed"/> property.
    /// </summary>
    public void Dispose() => _isDisposed = true;
}
