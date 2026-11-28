// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;
using Splat.Builder;

namespace DispatchDesk;

/// <summary>Registers the shared delivery queue and a fresh packing label for each request.</summary>
internal sealed class DispatchModule : IModule
{
    /// <inheritdoc/>
    public void Configure(IMutableDependencyResolver resolver)
    {
        resolver.RegisterLazySingleton(static () => new Queue<string>());
        resolver.Register(static () => new System.Text.StringBuilder());
    }
}
