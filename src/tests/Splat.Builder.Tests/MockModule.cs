// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder.Tests;

/// <summary>A mock <see cref="IModule"/> implementation used for testing.</summary>
internal sealed class MockModule : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        // This is a mock module for testing purposes.
        // It does not need to do anything specific.
        // In a real scenario, you would register services here.
    }
}
