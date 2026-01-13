// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Provides access to the current bitmap loader used for loading bitmap images within the application.
/// </summary>
/// <remarks>The bitmap loader is responsible for resolving and loading bitmap resources. The default
/// implementation is obtained from the application's dependency resolver. This class is typically used to configure or
/// retrieve the global bitmap loader instance. Changing the current loader affects all subsequent bitmap loading
/// operations that rely on this static context.</remarks>
public static class BitmapLoader
{
    // TODO: This needs to be improved once we move the "Detect in Unit Test
    // Runner" code into Splat
    private static IBitmapLoader? _current = AppLocator.Current.GetService<IBitmapLoader>();

    /// <summary>
    /// Gets or sets the current default bitmap loader instance used by the application.
    /// </summary>
    /// <remarks>This property provides access to the global bitmap loader implementation. Setting this
    /// property replaces the current default loader for all subsequent bitmap loading operations. Typically, this is
    /// configured during application startup and should not be changed at runtime unless reconfiguring the dependency
    /// resolver.</remarks>
    [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
    public static IBitmapLoader Current
    {
        get
        {
            var ret = _current;
            return ret switch
            {
                null => throw new BitmapLoaderException("Could not find a default bitmap loader. This should never happen, your dependency resolver is broken"),
                _ => ret
            };
        }
        set => _current = value;
    }

    /// <summary>
    /// Gets the current bitmap loader state, if available. Used by test scopes.
    /// </summary>
    /// <returns>The current <see cref="IBitmapLoader"/> instance representing the loader state, or <see langword="null"/> if no
    /// state is set.</returns>
    internal static IBitmapLoader? GetState() => _current;

    /// <summary>
    /// Restores the current bitmap loader state to the specified value. Used by test scopes.
    /// </summary>
    /// <param name="state">The bitmap loader state to restore. Can be null to clear the current state.</param>
    internal static void RestoreState(IBitmapLoader? state) => _current = state;

    /// <summary>
    /// Resets the internal bitmap loader state to use the current application service instance. Used by test scopes.
    /// </summary>
    /// <remarks>This method is intended for internal use to reinitialize the bitmap loader, typically after
    /// application service changes. It should not be called directly by application code.</remarks>
    internal static void ResetState() => _current = AppLocator.Current.GetService<IBitmapLoader>();
}
