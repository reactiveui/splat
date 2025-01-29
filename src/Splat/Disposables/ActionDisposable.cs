// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A disposable which will call the specified action.
/// </summary>
internal sealed class ActionDisposable(Action block) : IDisposable
{
    private Action _block = block;

    /// <summary>
    /// Gets a action disposable which does nothing.
    /// </summary>
    public static IDisposable Empty => new ActionDisposable(() => { });

    /// <inheritdoc />
    public void Dispose() => Interlocked.Exchange(ref _block, () => { })();
}
