// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Shared helpers for resolver teardown and notification paths that must never surface an exception.</summary>
/// <remarks>
/// Disposal and registration-change notification invoke arbitrary user code (callbacks and
/// <see cref="IDisposable.Dispose"/> implementations). A single faulty callback must not abort the
/// remaining teardown work, so those invocations are wrapped here and their exceptions suppressed.
/// </remarks>
internal static class ResolverExceptionHelpers
{
    /// <summary>Invokes <paramref name="action"/>, suppressing any exception it throws.</summary>
    /// <param name="action">The action to invoke.</param>
    /// <remarks>
    /// A swallowed exception is written to <see cref="System.Diagnostics.Debug"/> to aid diagnosis without
    /// interrupting the caller's loop. Intended for disposal and notification loops where each element must
    /// run regardless of the outcome of the others.
    /// </remarks>
    internal static void RunSwallowingExceptions(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            // Suppress exceptions raised by user callbacks or Dispose implementations during teardown/notification.
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}
