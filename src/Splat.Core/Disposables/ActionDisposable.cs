// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A disposable which will call the specified action.
/// </summary>
internal sealed class ActionDisposable : IDisposable
{
    private Action _block;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionDisposable"/> class.
    /// </summary>
    /// <param name="block">The action to execute when disposed.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="block"/> is null.</exception>
    public ActionDisposable(Action block)
    {
        block.ThrowArgumentNullExceptionIfNull(nameof(block));
        _block = block;
    }

    /// <summary>
    /// Gets an action disposable which does nothing.
    /// </summary>
    public static IDisposable Empty => new ActionDisposable(() => { });

    /// <inheritdoc />
    public void Dispose() => Interlocked.Exchange(ref _block, () => { })();
}
