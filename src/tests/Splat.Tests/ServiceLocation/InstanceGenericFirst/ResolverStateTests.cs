// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for ResolverState.</summary>
public class ResolverStateTests
{
    /// <summary>Verifies that each ResolverState instance has a unique identifier.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolverState_Should_HaveUniqueIds()
    {
        // Arrange & Act
        var state1 = new ResolverState();
        var state2 = new ResolverState();
        var state3 = new ResolverState();

        // Assert
        await Assert.That(state1.Id).IsNotEqualTo(state2.Id);
        await Assert.That(state2.Id).IsNotEqualTo(state3.Id);
        await Assert.That(state1.Id).IsNotEqualTo(state3.Id);
    }

    /// <summary>Verifies that ResolverState identifiers increase monotonically across instances.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolverState_Should_HaveMonotonicallyIncreasingIds()
    {
        // Arrange & Act
        var state1 = new ResolverState();
        var state2 = new ResolverState();
        var state3 = new ResolverState();

        // Assert
        await Assert.That(state2.Id).IsGreaterThan(state1.Id);
        await Assert.That(state3.Id).IsGreaterThan(state2.Id);
    }
}
