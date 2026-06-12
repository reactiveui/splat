// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>Tests for the ArrayHelpers class.</summary>
public class ArrayHelpersTests
{
    /// <summary>The expected array length when two items are present.</summary>
    private const int TwoItems = 2;

    /// <summary>The expected array length when three items are present.</summary>
    private const int ThreeItems = 3;

    /// <summary>The value of the first item used in these tests.</summary>
    private const int FirstValue = 1;

    /// <summary>The value of the second item used in these tests.</summary>
    private const int SecondValue = 2;

    /// <summary>The value of the third item used in these tests.</summary>
    private const int ThirdValue = 3;

    /// <summary>A sample value appended to arrays in these tests.</summary>
    private const int SampleValue = 42;

    /// <summary>A seed value used to pre-populate arrays in these tests.</summary>
    private const int SeedValue = 99;

    /// <summary>Zero-based index of the third item.</summary>
    private const int ThirdIndex = 2;

    /// <summary>Tests that append nullable with null array creates new array with single item.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AppendNullable_WithNullArray_CreatesNewArrayWithSingleItem()
    {
        // Arrange
        var item = Registration<string>.FromInstance("test");

        // Act
        var result = ArrayHelpers.AppendNullable<string>(null, item);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(1);
        await Assert.That(result[0]).IsEqualTo(item);
    }

    /// <summary>Tests that append nullable with existing array appends item.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AppendNullable_WithExistingArray_AppendsItem()
    {
        // Arrange
        var item1 = Registration<string>.FromInstance("first");
        var item2 = Registration<string>.FromInstance("second");
        var existing = new[] { item1 };

        // Act
        var result = ArrayHelpers.AppendNullable(existing, item2);

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        await Assert.That(result[0]).IsEqualTo(item1);
        await Assert.That(result[1]).IsEqualTo(item2);
    }

    /// <summary>Tests that append nullable with multiple items maintains order.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AppendNullable_WithMultipleItems_MaintainsOrder()
    {
        // Arrange
        var items = new[]
        {
            Registration<int>.FromInstance(FirstValue),
            Registration<int>.FromInstance(SecondValue),
            Registration<int>.FromInstance(ThirdValue)
        };

        // Act
        Registration<int>[]? result = null;
        foreach (var item in items)
        {
            result = ArrayHelpers.AppendNullable(result, item);
        }

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Length).IsEqualTo(ThreeItems);
        for (int i = 0; i < items.Length; i++)
        {
            await Assert.That(result[i]).IsEqualTo(items[i]);
        }
    }

    /// <summary>Tests that remove last with null array returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveLast_WithNullArray_ReturnsEmptyArray()
    {
        // Act
        var result = ArrayHelpers.RemoveLast<string>(null);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that remove last with empty array returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveLast_WithEmptyArray_ReturnsEmptyArray()
    {
        // Arrange
        var empty = Array.Empty<string>();

        // Act
        var result = ArrayHelpers.RemoveLast(empty);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that remove last with single item returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveLast_WithSingleItem_ReturnsEmptyArray()
    {
        // Arrange
        var array = new[] { "single" };

        // Act
        var result = ArrayHelpers.RemoveLast(array);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that remove last with multiple items removes last item.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveLast_WithMultipleItems_RemovesLastItem()
    {
        // Arrange
        var array = new[] { "first", "second", "third" };

        // Act
        var result = ArrayHelpers.RemoveLast(array);

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
    }

    /// <summary>Tests that remove last does not modify original array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RemoveLast_DoesNotModifyOriginalArray()
    {
        // Arrange
        var array = new[] { FirstValue, SecondValue, ThirdValue };
        var originalLength = array.Length;
        var lastIndex = array.Length - 1;

        // Act
        ArrayHelpers.RemoveLast(array);

        // Assert
        await Assert.That(array.Length).IsEqualTo(originalLength);
        await Assert.That(array[lastIndex]).IsEqualTo(ThirdValue);
    }

    /// <summary>Tests that materialize registrations with empty array returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithEmptyArray_ReturnsEmptyArray()
    {
        // Arrange
        var registrations = Array.Empty<Registration<string>>();

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that materialize registrations with instance registrations returns instances.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithInstanceRegistrations_ReturnsInstances()
    {
        // Arrange
        var registrations = new[]
        {
            Registration<string>.FromInstance("one"),
            Registration<string>.FromInstance("two"),
            Registration<string>.FromInstance("three")
        };

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result.Length).IsEqualTo(ThreeItems);
        await Assert.That(result[0]).IsEqualTo("one");
        await Assert.That(result[1]).IsEqualTo("two");
        await Assert.That(result[ThirdIndex]).IsEqualTo("three");
    }

    /// <summary>Tests that materialize registrations with factory registrations invokes factories.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithFactoryRegistrations_InvokesFactories()
    {
        // Arrange
        var invocationCount = 0;
        var registrations = new[]
        {
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return FirstValue;
                }),
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return SecondValue;
                }),
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return ThirdValue;
                })
        };

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result.Length).IsEqualTo(ThreeItems);
        await Assert.That(result[0]).IsEqualTo(FirstValue);
        await Assert.That(result[1]).IsEqualTo(SecondValue);
        await Assert.That(result[ThirdIndex]).IsEqualTo(ThirdValue);
        await Assert.That(invocationCount).IsEqualTo(ThreeItems);
    }

    /// <summary>Tests that materialize registrations with mixed registrations returns all values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithMixedRegistrations_ReturnsAllValues()
    {
        // Arrange
        var registrations = new[]
        {
            Registration<string>.FromInstance("instance"),
            Registration<string>.FromFactory(() => "factory"),
            Registration<string>.FromInstance("another instance")
        };

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result.Length).IsEqualTo(ThreeItems);
        await Assert.That(result[0]).IsEqualTo("instance");
        await Assert.That(result[1]).IsEqualTo("factory");
        await Assert.That(result[ThirdIndex]).IsEqualTo("another instance");
    }

    /// <summary>Tests that materialize registrations with null values filters out nulls.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithNullValues_FiltersOutNulls()
    {
        // Arrange
        var registrations = new[]
        {
            Registration<string?>.FromInstance("valid"),
            Registration<string?>.FromInstance(null),
            Registration<string?>.FromFactory(() => null),
            Registration<string?>.FromInstance("another valid")
        };

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        await Assert.That(result[0]).IsEqualTo("valid");
        await Assert.That(result[1]).IsEqualTo("another valid");
    }

    /// <summary>Tests that materialize registrations with factory that throws propagates exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_WithFactoryThatThrows_PropagatesException()
    {
        // Arrange
        var registrations = new[]
        {
            Registration<string>.FromFactory(() => throw new InvalidOperationException("Test exception"))
        };

        // Act & Assert
        await Assert.That(async () => ArrayHelpers.MaterializeRegistrations(registrations))
            .Throws<InvalidOperationException>();
    }

    /// <summary>Tests that materialize registrations called multiple times invokes factories each time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MaterializeRegistrations_CalledMultipleTimes_InvokesFactoriesEachTime()
    {
        // Arrange
        var invocationCount = 0;
        var registrations = new[]
        {
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return invocationCount;
                })
        };

        // Act
        var result1 = ArrayHelpers.MaterializeRegistrations(registrations);
        var result2 = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(invocationCount).IsEqualTo(TwoItems);
        await Assert.That(result1[0]).IsEqualTo(FirstValue);
        await Assert.That(result2[0]).IsEqualTo(SecondValue);
    }

    /// <summary>Tests that entry add increments count and publishes items.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_Add_IncrementsCountAndPublishesItems()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();

        // Act
        entry.Add(FirstValue);
        entry.Add(SecondValue);

        // Assert
        await Assert.That(entry.Count).IsEqualTo(TwoItems);
        await Assert.That(entry.HasItems).IsTrue();

        var snapshot = entry.GetSnapshot();
        await Assert.That(snapshot.Length).IsEqualTo(TwoItems);
        await Assert.That(snapshot[0]).IsEqualTo(FirstValue);
        await Assert.That(snapshot[1]).IsEqualTo(SecondValue);
    }

    /// <summary>Tests that entry has items is false when empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_HasItems_IsFalseWhenEmpty()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();

        // Assert
        await Assert.That(entry.HasItems).IsFalse();
        await Assert.That(entry.Count).IsEqualTo(0);
        await Assert.That(entry.GetSnapshot().Length).IsEqualTo(0);
    }

    /// <summary>Tests that entry get snapshot returns cached instance when unchanged.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_GetSnapshot_ReturnsCachedInstanceWhenUnchanged()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(1);

        // Act
        var first = entry.GetSnapshot();
        var second = entry.GetSnapshot();

        // Assert
        await Assert.That(second).IsSameReferenceAs(first);
    }

    /// <summary>Tests that entry get snapshot rebuilds after mutation.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_GetSnapshot_RebuildsAfterMutation()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(FirstValue);
        var first = entry.GetSnapshot();

        // Act
        entry.Add(SecondValue);
        var second = entry.GetSnapshot();

        // Assert
        await Assert.That(first.Length).IsEqualTo(1);
        await Assert.That(second.Length).IsEqualTo(TwoItems);
        await Assert.That(second).IsNotSameReferenceAs(first);
    }

    /// <summary>Tests that entry remove current removes last item and reports not empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_RemoveCurrent_RemovesLastItemAndReportsNotEmpty()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(FirstValue);
        entry.Add(SecondValue);

        // Act
        var becameEmpty = entry.RemoveCurrent();

        // Assert
        await Assert.That(becameEmpty).IsFalse();
        await Assert.That(entry.Count).IsEqualTo(1);

        var snapshot = entry.GetSnapshot();
        await Assert.That(snapshot.Length).IsEqualTo(1);
        await Assert.That(snapshot[0]).IsEqualTo(FirstValue);
    }

    /// <summary>Tests that entry remove current reports empty when last item removed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_RemoveCurrent_ReportsEmptyWhenLastItemRemoved()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(SampleValue);

        // Act
        var becameEmpty = entry.RemoveCurrent();

        // Assert
        await Assert.That(becameEmpty).IsTrue();
        await Assert.That(entry.HasItems).IsFalse();
        await Assert.That(entry.GetSnapshot().Length).IsEqualTo(0);
    }

    /// <summary>Tests that entry remove current on empty entry returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_RemoveCurrent_OnEmptyEntry_ReturnsFalse()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();

        // Act
        var becameEmpty = entry.RemoveCurrent();

        // Assert
        await Assert.That(becameEmpty).IsFalse();
        await Assert.That(entry.Count).IsEqualTo(0);
    }

    /// <summary>Tests that entry clear removes all items.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_Clear_RemovesAllItems()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(FirstValue);
        entry.Add(SecondValue);
        entry.Add(ThirdValue);

        // Act
        entry.Clear();

        // Assert
        await Assert.That(entry.HasItems).IsFalse();
        await Assert.That(entry.Count).IsEqualTo(0);
        await Assert.That(entry.GetSnapshot().Length).IsEqualTo(0);
    }

    /// <summary>Tests that entry copy items to appends all items.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Entry_CopyItemsTo_AppendsAllItems()
    {
        // Arrange
        var entry = new ArrayHelpers.Entry<int>();
        entry.Add(FirstValue);
        entry.Add(SecondValue);
        var destination = new List<int> { SeedValue };

        // Act
        entry.CopyItemsTo(destination);

        // Assert
        await Assert.That(destination.Count).IsEqualTo(ThreeItems);
        await Assert.That(destination[0]).IsEqualTo(SeedValue);
        await Assert.That(destination[1]).IsEqualTo(FirstValue);
        await Assert.That(destination[ThirdIndex]).IsEqualTo(SecondValue);
    }
}
