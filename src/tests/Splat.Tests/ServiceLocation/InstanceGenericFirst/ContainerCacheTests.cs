// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for ContainerCache to verify per-resolver isolation via ConditionalWeakTable.
/// </summary>
public class ContainerCacheTests
{
    [Test]
    public async Task ContainerCache_Should_IsolatePerResolverState()
    {
        // Arrange
        var state1 = new ResolverState();
        var state2 = new ResolverState();

        var container1 = ContainerCache<TestService>.Get(state1);
        var container2 = ContainerCache<TestService>.Get(state2);

        // Act
        container1.Add(new TestService { Value = 100 });
        container2.Add(new TestService { Value = 200 });

        var result1 = container1.TryGet(out var service1);
        var result2 = container2.TryGet(out var service2);

        // Assert
        await Assert.That(result1).IsTrue();
        await Assert.That(result2).IsTrue();
        await Assert.That(service1!.Value).IsEqualTo(100);
        await Assert.That(service2!.Value).IsEqualTo(200);
    }

    [Test]
    public async Task ContainerCache_Should_ReuseContainerForSameState()
    {
        // Arrange
        var state = new ResolverState();

        // Act
        var container1 = ContainerCache<TestService>.Get(state);
        var container2 = ContainerCache<TestService>.Get(state);

        // Assert
        await Assert.That(ReferenceEquals(container1, container2)).IsTrue();
    }

    [Test]
    public async Task ContainerCache_Should_ReturnEmptyForNewState()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);

        // Act
        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(service).IsNull();
        await Assert.That(container.HasRegistrations).IsFalse();
    }

    [Test]
    public async Task ContainerCache_Should_HandleFactoryRegistrations()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        var callCount = 0;

        // Act
        container.Add(() =>
        {
            callCount++;
            return new() { Value = 42 };
        });

        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(42);
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task ContainerCache_Should_ReturnMostRecentRegistration()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);

        // Act
        container.Add(new TestService { Value = 1 });
        container.Add(new TestService { Value = 2 });
        container.Add(new TestService { Value = 3 });

        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(3);
    }

    [Test]
    public async Task ContainerCache_Should_RemoveCurrent()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = 1 });
        container.Add(new TestService { Value = 2 });

        // Act
        container.RemoveCurrent();
        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(1);
    }

    [Test]
    public async Task ContainerCache_Should_ClearAllRegistrations()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = 1 });
        container.Add(new TestService { Value = 2 });

        // Act
        container.Clear();
        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(service).IsNull();
        await Assert.That(container.HasRegistrations).IsFalse();
    }

    [Test]
    public async Task ContainerCache_GetAll_Should_ReturnAllRegistrations()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = 1 });
        container.Add(new TestService { Value = 2 });
        container.Add(new TestService { Value = 3 });

        // Act
        var all = container.GetAll();

        // Assert
        await Assert.That(all.Length).IsEqualTo(3);
        await Assert.That(all[0].Value).IsEqualTo(1);
        await Assert.That(all[1].Value).IsEqualTo(2);
        await Assert.That(all[2].Value).IsEqualTo(3);
    }

    public class TestService
    {
        public int Value { get; set; }
    }
}
