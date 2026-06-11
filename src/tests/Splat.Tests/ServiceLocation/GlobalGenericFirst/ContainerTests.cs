// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>Tests for the Container&lt;T&gt; class.</summary>
[NotInParallel] // Container<T> is static, tests must run sequentially
public class ContainerTests
{
    /// <summary>Clears the container state before each test.</summary>
    [Before(Test)]
    public void Setup()
    {
        // Clear the container before each test
        Container<string>.Clear();
        Container<int>.Clear();
        Container<int?>.Clear();
        Container<TestService>.Clear();
    }

    /// <summary>Clears the container state after each test.</summary>
    [After(Test)]
    public void Cleanup()
    {
        // Clear the container after each test
        Container<string>.Clear();
        Container<int>.Clear();
        Container<int?>.Clear();
        Container<TestService>.Clear();
    }

    /// <summary>Tests that has registrations when empty returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistrations_WhenEmpty_ReturnsFalse() =>

        // Act & Assert
        await Assert.That(Container<string>.HasRegistrations).IsFalse();

    /// <summary>Tests that has registrations after adding returns true.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistrations_AfterAdding_ReturnsTrue()
    {
        // Act
        Container<string>.Add("test");

        // Assert
        await Assert.That(Container<string>.HasRegistrations).IsTrue();
    }

    /// <summary>Tests that add with instance stores instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Add_WithInstance_StoresInstance()
    {
        // Arrange
        const string instance = "test value";

        // Act
        Container<string>.Add(instance);
        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(instance);
    }

    /// <summary>Tests that add with factory stores factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Add_WithFactory_StoresFactory()
    {
        // Arrange
        const int expectedValue = 42;
        Func<int?> factory = () => expectedValue;

        // Act
        Container<int?>.Add(factory);
        var success = Container<int?>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    /// <summary>Tests that add multiple instances returns latest.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Add_MultipleInstances_ReturnsLatest()
    {
        // Act
        Container<string>.Add("first");
        Container<string>.Add("second");
        Container<string>.Add("third");

        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("third");
    }

    /// <summary>Tests that try get when empty returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGet_WhenEmpty_ReturnsFalse()
    {
        // Act
        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

    /// <summary>Tests that try get with factory returning null returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGet_WithFactoryReturningNull_ReturnsFalse()
    {
        // Arrange
        Container<string?>.Add(() => null);

        // Act
        var success = Container<string?>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

    /// <summary>Tests that try get invokes factory each time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGet_InvokesFactoryEachTime()
    {
        // Arrange
        var invocationCount = 0;
        Container<int>.Add(() =>
        {
            invocationCount++;
            return invocationCount;
        });

        // Act
        Container<int>.TryGet(out var result1);
        Container<int>.TryGet(out var result2);

        // Assert
        await Assert.That(invocationCount).IsEqualTo(2);
        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(2);
    }

    /// <summary>Tests that get all when empty returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAll_WhenEmpty_ReturnsEmptyArray()
    {
        // Act
        var result = Container<string>.GetAll();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that get all with multiple registrations returns all values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAll_WithMultipleRegistrations_ReturnsAllValues()
    {
        // Arrange
        Container<string>.Add("first");
        Container<string>.Add("second");
        Container<string>.Add("third");

        // Act
        var result = Container<string>.GetAll();

        // Assert
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
        await Assert.That(result[2]).IsEqualTo("third");
    }

    /// <summary>Tests that get all with factories invokes all factories.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAll_WithFactories_InvokesAllFactories()
    {
        // Arrange
        var invocationCount = 0;
        Container<int>.Add(() =>
        {
            invocationCount++;
            return 1;
        });
        Container<int>.Add(() =>
        {
            invocationCount++;
            return 2;
        });
        Container<int>.Add(() =>
        {
            invocationCount++;
            return 3;
        });

        // Act
        var result = Container<int>.GetAll();

        // Assert
        await Assert.That(invocationCount).IsEqualTo(3);
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo(1);
        await Assert.That(result[1]).IsEqualTo(2);
        await Assert.That(result[2]).IsEqualTo(3);
    }

    /// <summary>Tests that get all with mixed registrations returns all values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAll_WithMixedRegistrations_ReturnsAllValues()
    {
        // Arrange
        Container<string>.Add("instance");
        Container<string>.Add(() => "factory");
        Container<string>.Add("another instance");

        // Act
        var result = Container<string>.GetAll();

        // Assert
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("instance");
        await Assert.That(result[1]).IsEqualTo("factory");
        await Assert.That(result[2]).IsEqualTo("another instance");
    }

    /// <summary>Tests that remove current when empty does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveCurrent_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        Container<string>.RemoveCurrent();
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
    }

    /// <summary>Tests that remove current with single item clears container.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveCurrent_WithSingleItem_ClearsContainer()
    {
        // Arrange
        Container<string>.Add("test");

        // Act
        Container<string>.RemoveCurrent();

        // Assert
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
        var success = Container<string>.TryGet(out _);
        await Assert.That(success).IsFalse();
    }

    /// <summary>Tests that remove current with multiple items removes latest.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveCurrent_WithMultipleItems_RemovesLatest()
    {
        // Arrange
        Container<string>.Add("first");
        Container<string>.Add("second");
        Container<string>.Add("third");

        // Act
        Container<string>.RemoveCurrent();
        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("second");
    }

    /// <summary>Tests that remove current multiple removes in reverse order.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveCurrent_Multiple_RemovesInReverseOrder()
    {
        // Arrange
        Container<int>.Add(1);
        Container<int>.Add(2);
        Container<int>.Add(3);

        // Act & Assert
        Container<int>.RemoveCurrent();
        Container<int>.TryGet(out var result1);
        await Assert.That(result1).IsEqualTo(2);

        Container<int>.RemoveCurrent();
        Container<int>.TryGet(out var result2);
        await Assert.That(result2).IsEqualTo(1);

        Container<int>.RemoveCurrent();
        var success = Container<int>.TryGet(out _);
        await Assert.That(success).IsFalse();
    }

    /// <summary>Tests that clear removes all registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_RemovesAllRegistrations()
    {
        // Arrange
        Container<string>.Add("first");
        Container<string>.Add("second");
        Container<string>.Add("third");

        // Act
        Container<string>.Clear();

        // Assert
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
        var result = Container<string>.GetAll();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that clear when empty does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        Container<string>.Clear();
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
    }

    /// <summary>Tests that container with complex type works correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Container_WithComplexType_WorksCorrectly()
    {
        // Arrange
        var service = new TestService { Id = 1, Name = "Test" };

        // Act
        Container<TestService>.Add(service);
        var success = Container<TestService>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(service);
        await Assert.That(result!.Id).IsEqualTo(1);
        await Assert.That(result.Name).IsEqualTo("Test");
    }

    /// <summary>Tests that container thread safety concurrent adds.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Container_ThreadSafety_ConcurrentAdds()
    {
        // Arrange
        var tasks = new List<Task>();
        const int itemCount = 100;

        // Act - add items concurrently
        for (int i = 0; i < itemCount; i++)
        {
            var value = i;
            tasks.Add(Task.Run(() => Container<int>.Add(value)));
        }

        await Task.WhenAll(tasks);

        // Assert
        var result = Container<int>.GetAll();
        await Assert.That(result.Length).IsEqualTo(itemCount);
        await Assert.That(Container<int>.HasRegistrations).IsTrue();
    }

    /// <summary>Tests that container different types are isolated.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Container_DifferentTypes_AreIsolated()
    {
        // Arrange & Act
        Container<string>.Add("string value");
        Container<int>.Add(42);

        // Assert
        Container<string>.TryGet(out var stringResult);
        Container<int>.TryGet(out var intResult);

        await Assert.That(stringResult).IsEqualTo("string value");
        await Assert.That(intResult).IsEqualTo(42);
    }

    /// <summary>Tests that add large number of items maintains performance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Add_LargeNumberOfItems_MaintainsPerformance()
    {
        // Arrange
        const int itemCount = 1000;

        // Act
        for (int i = 0; i < itemCount; i++)
        {
            Container<int>.Add(i);
        }

        var result = Container<int>.GetAll();

        // Assert
        await Assert.That(result.Length).IsEqualTo(itemCount);
        await Assert.That(result[0]).IsEqualTo(0);
        await Assert.That(result[itemCount - 1]).IsEqualTo(itemCount - 1);
    }

    /// <summary>A simple service type used for testing the container.</summary>
    private sealed class TestService
    {
        /// <summary>Gets or sets the identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string? Name { get; set; }
    }
}
