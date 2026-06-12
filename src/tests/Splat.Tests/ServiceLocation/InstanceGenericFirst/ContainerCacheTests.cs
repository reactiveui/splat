// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for ContainerCache to verify per-resolver isolation via ConditionalWeakTable.</summary>
public class ContainerCacheTests
{
    /// <summary>The value registered against the first resolver state.</summary>
    private const int FirstStateValue = 100;

    /// <summary>The value registered against the second resolver state.</summary>
    private const int SecondStateValue = 200;

    /// <summary>The value produced by a factory registration.</summary>
    private const int FactoryValue = 42;

    /// <summary>The value of the first registration added to a container.</summary>
    private const int FirstValue = 1;

    /// <summary>The value of the second registration added to a container.</summary>
    private const int SecondValue = 2;

    /// <summary>The value of the third registration added to a container.</summary>
    private const int ThirdValue = 3;

    /// <summary>The number of registrations expected after adding three services.</summary>
    private const int ExpectedRegistrationCount = 3;

    /// <summary>Verifies that ContainerCache isolates registrations per resolver state.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContainerCache_Should_IsolatePerResolverState()
    {
        // Arrange
        var state1 = new ResolverState();
        var state2 = new ResolverState();

        var container1 = ContainerCache<TestService>.Get(state1);
        var container2 = ContainerCache<TestService>.Get(state2);

        // Act
        container1.Add(new TestService { Value = FirstStateValue });
        container2.Add(new TestService { Value = SecondStateValue });

        var result1 = container1.TryGet(out var service1);
        var result2 = container2.TryGet(out var service2);

        // Assert
        await Assert.That(result1).IsTrue();
        await Assert.That(result2).IsTrue();
        await Assert.That(service1!.Value).IsEqualTo(FirstStateValue);
        await Assert.That(service2!.Value).IsEqualTo(SecondStateValue);
    }

    /// <summary>Verifies that ContainerCache reuses the same container for the same resolver state.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that ContainerCache returns an empty container for a new resolver state.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that ContainerCache handles factory-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
            return new() { Value = FactoryValue };
        });

        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(FactoryValue);
        await Assert.That(callCount).IsEqualTo(1);
    }

    /// <summary>Verifies that ContainerCache returns the most recently added registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContainerCache_Should_ReturnMostRecentRegistration()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);

        // Act
        container.Add(new TestService { Value = FirstValue });
        container.Add(new TestService { Value = SecondValue });
        container.Add(new TestService { Value = ThirdValue });

        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(ThirdValue);
    }

    /// <summary>Verifies that ContainerCache removes the current registration, exposing the previous one.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContainerCache_Should_RemoveCurrent()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = FirstValue });
        container.Add(new TestService { Value = SecondValue });

        // Act
        container.RemoveCurrent();
        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(service!.Value).IsEqualTo(FirstValue);
    }

    /// <summary>Verifies that ContainerCache clears all registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContainerCache_Should_ClearAllRegistrations()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = FirstValue });
        container.Add(new TestService { Value = SecondValue });

        // Act
        container.Clear();
        var result = container.TryGet(out var service);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(service).IsNull();
        await Assert.That(container.HasRegistrations).IsFalse();
    }

    /// <summary>Verifies that ContainerCache GetAll returns all registrations in order.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContainerCache_GetAll_Should_ReturnAllRegistrations()
    {
        // Arrange
        var state = new ResolverState();
        var container = ContainerCache<TestService>.Get(state);
        container.Add(new TestService { Value = FirstValue });
        container.Add(new TestService { Value = SecondValue });
        container.Add(new TestService { Value = ThirdValue });

        // Act
        var all = container.GetAll();

        // Assert
        await Assert.That(all.Length).IsEqualTo(ExpectedRegistrationCount);
        await Assert.That(all[0].Value).IsEqualTo(FirstValue);
        await Assert.That(all[1].Value).IsEqualTo(SecondValue);
        await Assert.That(all[^1].Value).IsEqualTo(ThirdValue);
    }

    /// <summary>A simple service type used for testing the container cache.</summary>
    public class TestService
    {
        /// <summary>Gets or sets the value.</summary>
        public int Value { get; set; }
    }
}
