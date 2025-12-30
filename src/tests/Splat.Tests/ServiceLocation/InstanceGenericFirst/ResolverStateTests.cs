// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for ResolverState.
/// </summary>
public class ResolverStateTests
{
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
