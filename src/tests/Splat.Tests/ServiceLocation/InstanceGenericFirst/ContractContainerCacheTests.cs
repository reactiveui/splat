// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for <see cref="ContractContainerCache{T}"/> per-resolver contract containers.</summary>
public class ContractContainerCacheTests
{
    /// <summary>The first contract key used in these tests.</summary>
    private const string FirstContract = "first";

    /// <summary>The second contract key used in these tests.</summary>
    private const string SecondContract = "second";

    /// <summary>The count expected once a contract has been cleared.</summary>
    private const int EmptyCount = 0;

    /// <summary>Verifies that clearing all contracts removes every contract's registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ClearAll_RemovesRegistrationsForEveryContract()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContractContainerCache<ViewModelOne>.Get(state);
        container.Add(new ViewModelOne(), FirstContract);
        container.Add(new ViewModelOne(), SecondContract);

        using (Assert.Multiple())
        {
            await Assert.That(container.HasRegistrations(FirstContract)).IsTrue();
            await Assert.That(container.HasRegistrations(SecondContract)).IsTrue();
        }

        // Act
        container.ClearAll();

        // Assert
        using (Assert.Multiple())
        {
            await Assert.That(container.HasRegistrations(FirstContract)).IsFalse();
            await Assert.That(container.HasRegistrations(SecondContract)).IsFalse();
            await Assert.That(container.GetCount(FirstContract)).IsEqualTo(EmptyCount);
            await Assert.That(container.GetCount(SecondContract)).IsEqualTo(EmptyCount);
        }
    }
}
