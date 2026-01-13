// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides an <see cref="IDisposable"/> implementation that executes a specified action when disposed.
/// </summary>
internal sealed class ActionDisposable : IDisposable
{
    private Action _block;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionDisposable"/> class that executes the specified action when disposed.
    /// </summary>
    /// <param name="block">The action to execute when the object is disposed. Cannot be null.</param>
    public ActionDisposable(Action block)
    {
        ArgumentExceptionHelper.ThrowIfNull(block);
        _block = block;
    }

    /// <summary>
    /// Gets an empty disposable object that performs no action when disposed.
    /// </summary>
    /// <remarks>This property can be used when an IDisposable implementation is required but no cleanup is
    /// necessary. The returned object is safe to dispose multiple times.</remarks>
    public static IDisposable Empty => new ActionDisposable(() => { });

    /// <inheritdoc />
    public void Dispose() => Interlocked.Exchange(ref _block, () => { })();
}
