// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>
/// Tests for the Container&lt;T&gt; class.
/// </summary>
[NotInParallel] // Container<T> is static, tests must run sequentially
public class ContainerTests
{
    [Before(HookType.Test)]
    public void Setup()
    {
        // Clear the container before each test
        Container<string>.Clear();
        Container<int>.Clear();
        Container<int?>.Clear();
        Container<TestService>.Clear();
    }

    [After(HookType.Test)]
    public void Cleanup()
    {
        // Clear the container after each test
        Container<string>.Clear();
        Container<int>.Clear();
        Container<int?>.Clear();
        Container<TestService>.Clear();
    }

    [Test]
    public async Task HasRegistrations_WhenEmpty_ReturnsFalse() =>

        // Act & Assert
        await Assert.That(Container<string>.HasRegistrations).IsFalse();

    [Test]
    public async Task HasRegistrations_AfterAdding_ReturnsTrue()
    {
        // Act
        Container<string>.Add("test");

        // Assert
        await Assert.That(Container<string>.HasRegistrations).IsTrue();
    }

    [Test]
    public async Task Add_WithInstance_StoresInstance()
    {
        // Arrange
        var instance = "test value";

        // Act
        Container<string>.Add(instance);
        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(instance);
    }

    [Test]
    public async Task Add_WithFactory_StoresFactory()
    {
        // Arrange
        var expectedValue = 42;
        Func<int?> factory = () => expectedValue;

        // Act
        Container<int?>.Add(factory);
        var success = Container<int?>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(expectedValue);
    }

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

    [Test]
    public async Task TryGet_WhenEmpty_ReturnsFalse()
    {
        // Act
        var success = Container<string>.TryGet(out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

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

    [Test]
    public async Task GetAll_WhenEmpty_ReturnsEmptyArray()
    {
        // Act
        var result = Container<string>.GetAll();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

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

    [Test]
    public async Task RemoveCurrent_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        Container<string>.RemoveCurrent();
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
    }

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

    [Test]
    public async Task Clear_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        Container<string>.Clear();
        await Assert.That(Container<string>.HasRegistrations).IsFalse();
    }

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

    [Test]
    public async Task Container_ThreadSafety_ConcurrentAdds()
    {
        // Arrange
        var tasks = new List<Task>();
        var itemCount = 100;

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

    [Test]
    public async Task Add_LargeNumberOfItems_MaintainsPerformance()
    {
        // Arrange
        var itemCount = 1000;

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

    private sealed class TestService
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
