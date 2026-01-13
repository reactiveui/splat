// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>
/// Tests for the ArrayHelpers class.
/// </summary>
public class ArrayHelpersTests
{
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
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo(item1);
        await Assert.That(result[1]).IsEqualTo(item2);
    }

    [Test]
    public async Task AppendNullable_WithMultipleItems_MaintainsOrder()
    {
        // Arrange
        var items = new[]
        {
            Registration<int>.FromInstance(1),
            Registration<int>.FromInstance(2),
            Registration<int>.FromInstance(3)
        };

        // Act
        Registration<int>[]? result = null;
        foreach (var item in items)
        {
            result = ArrayHelpers.AppendNullable(result, item);
        }

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Length).IsEqualTo(3);
        for (int i = 0; i < items.Length; i++)
        {
            await Assert.That(result[i]).IsEqualTo(items[i]);
        }
    }

    [Test]
    public async Task RemoveLast_WithNullArray_ReturnsEmptyArray()
    {
        // Act
        var result = ArrayHelpers.RemoveLast<string>(null);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

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

    [Test]
    public async Task RemoveLast_WithMultipleItems_RemovesLastItem()
    {
        // Arrange
        var array = new[] { "first", "second", "third" };

        // Act
        var result = ArrayHelpers.RemoveLast(array);

        // Assert
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
    }

    [Test]
    public async Task RemoveLast_DoesNotModifyOriginalArray()
    {
        // Arrange
        var array = new[] { 1, 2, 3 };
        var originalLength = array.Length;

        // Act
        var result = ArrayHelpers.RemoveLast(array);

        // Assert
        await Assert.That(array.Length).IsEqualTo(originalLength);
        await Assert.That(array[2]).IsEqualTo(3);
    }

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
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("one");
        await Assert.That(result[1]).IsEqualTo("two");
        await Assert.That(result[2]).IsEqualTo("three");
    }

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
                    return 1;
                }),
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return 2;
                }),
            Registration<int>.FromFactory(
                () =>
                {
                    invocationCount++;
                    return 3;
                })
        };

        // Act
        var result = ArrayHelpers.MaterializeRegistrations(registrations);

        // Assert
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo(1);
        await Assert.That(result[1]).IsEqualTo(2);
        await Assert.That(result[2]).IsEqualTo(3);
        await Assert.That(invocationCount).IsEqualTo(3);
    }

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
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("instance");
        await Assert.That(result[1]).IsEqualTo("factory");
        await Assert.That(result[2]).IsEqualTo("another instance");
    }

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
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo("valid");
        await Assert.That(result[1]).IsEqualTo("another valid");
    }

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
        await Assert.That(invocationCount).IsEqualTo(2);
        await Assert.That(result1[0]).IsEqualTo(1);
        await Assert.That(result2[0]).IsEqualTo(2);
    }

    /// <summary>
    /// Verifies that Add creates a new entry when adding to a non-existent key.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Add_CreatesEntryAndIncrementsVersion()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();

        // Act
        ArrayHelpers.Add(entries, "key1", 42);

        // Assert
        await Assert.That(entries.ContainsKey("key1")).IsTrue();
        await Assert.That(entries["key1"].List.Count).IsEqualTo(1);
        await Assert.That(entries["key1"].List[0]).IsEqualTo(42);
        await Assert.That(entries["key1"].Version).IsEqualTo(1);
    }

    /// <summary>
    /// Verifies that Add appends to an existing entry and increments version.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Add_AppendsToExistingEntry()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();

        // Act
        ArrayHelpers.Add(entries, "key1", 1);
        ArrayHelpers.Add(entries, "key1", 2);
        ArrayHelpers.Add(entries, "key1", 3);

        // Assert
        await Assert.That(entries["key1"].List.Count).IsEqualTo(3);
        await Assert.That(entries["key1"].List[0]).IsEqualTo(1);
        await Assert.That(entries["key1"].List[1]).IsEqualTo(2);
        await Assert.That(entries["key1"].List[2]).IsEqualTo(3);
        await Assert.That(entries["key1"].Version).IsEqualTo(3);
    }

    /// <summary>
    /// Verifies that GetSnapshot rebuilds snapshot lazily when version is stale.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetSnapshot_RebuildsWhenVersionIsStale()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        ArrayHelpers.Add(entries, "key1", 2);
        ArrayHelpers.Add(entries, "key1", 3);

        // Act
        var snapshot = ArrayHelpers.GetSnapshot(entries, "key1");

        // Assert
        await Assert.That(snapshot).IsNotNull();
        await Assert.That(snapshot.Length).IsEqualTo(3);
        await Assert.That(snapshot[0]).IsEqualTo(1);
        await Assert.That(snapshot[1]).IsEqualTo(2);
        await Assert.That(snapshot[2]).IsEqualTo(3);
        await Assert.That(entries["key1"].SnapshotVersion).IsEqualTo(entries["key1"].Version);
    }

    /// <summary>
    /// Verifies that RemoveCurrent removes the last item and increments version.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task RemoveCurrent_RemovesLastItem()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        ArrayHelpers.Add(entries, "key1", 2);
        ArrayHelpers.Add(entries, "key1", 3);

        // Act
        ArrayHelpers.RemoveCurrent(entries, "key1");

        // Assert
        await Assert.That(entries["key1"].List.Count).IsEqualTo(2);
        await Assert.That(entries["key1"].List[0]).IsEqualTo(1);
        await Assert.That(entries["key1"].List[1]).IsEqualTo(2);
        await Assert.That(entries["key1"].Version).IsEqualTo(4);
    }

    /// <summary>
    /// Verifies that RemoveCurrent removes the entry from dictionary when the last item is removed.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task RemoveCurrent_RemovesEntryWhenLastItemRemoved()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 42);

        // Act
        ArrayHelpers.RemoveCurrent(entries, "key1");

        // Assert
        await Assert.That(entries.ContainsKey("key1")).IsFalse();
    }

    /// <summary>
    /// Verifies that RemoveCurrent handles removing from a non-existent key gracefully.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task RemoveCurrent_HandlesNonExistentKey()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();

        // Act - should not throw
        ArrayHelpers.RemoveCurrent(entries, "nonexistent");

        // Assert
        await Assert.That(entries.ContainsKey("nonexistent")).IsFalse();
    }

    /// <summary>
    /// Verifies that RemoveAll removes the entry from the dictionary.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task RemoveAll_RemovesEntry()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        ArrayHelpers.Add(entries, "key1", 2);

        // Act
        ArrayHelpers.RemoveAll(entries, "key1");

        // Assert
        await Assert.That(entries.ContainsKey("key1")).IsFalse();
    }

    /// <summary>
    /// Verifies that Clear removes all entries from the dictionary.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Clear_RemovesAllEntries()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        ArrayHelpers.Add(entries, "key2", 2);
        ArrayHelpers.Add(entries, "key3", 3);

        // Act
        ArrayHelpers.Clear(entries);

        // Assert
        await Assert.That(entries.Count).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that entry-based helpers maintain isolation between different keys.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task EntryHelpers_WithDifferentKeys_AreIsolated()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();

        // Act
        ArrayHelpers.Add(entries, "key1", 10);
        ArrayHelpers.Add(entries, "key2", 20);
        ArrayHelpers.Add(entries, "key3", 30);

        // Assert
        var snap1 = ArrayHelpers.GetSnapshot(entries, "key1");
        await Assert.That(snap1).IsNotNull();
        await Assert.That(snap1[0]).IsEqualTo(10);

        var snap2 = ArrayHelpers.GetSnapshot(entries, "key2");
        await Assert.That(snap2).IsNotNull();
        await Assert.That(snap2[0]).IsEqualTo(20);

        var snap3 = ArrayHelpers.GetSnapshot(entries, "key3");
        await Assert.That(snap3).IsNotNull();
        await Assert.That(snap3[0]).IsEqualTo(30);
    }

    /// <summary>
    /// Verifies that GetSnapshot returns cached snapshot when version hasn't changed.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetSnapshot_ReturnsCachedSnapshotWhenVersionUnchanged()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        var firstSnapshot = ArrayHelpers.GetSnapshot(entries, "key1");

        // Act
        var secondSnapshot = ArrayHelpers.GetSnapshot(entries, "key1");

        // Assert - Should return the same array reference
        await Assert.That(ReferenceEquals(firstSnapshot, secondSnapshot)).IsTrue();
    }

    /// <summary>
    /// Verifies that GetSnapshot rebuilds snapshot after version changes.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetSnapshot_RebuildsSnapshotAfterVersionChanges()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);
        var firstSnapshot = ArrayHelpers.GetSnapshot(entries, "key1");

        // Act - Add more items to change version
        ArrayHelpers.Add(entries, "key1", 2);
        var secondSnapshot = ArrayHelpers.GetSnapshot(entries, "key1");

        // Assert - Should return a different array reference with updated content
        await Assert.That(ReferenceEquals(firstSnapshot, secondSnapshot)).IsFalse();
        await Assert.That(firstSnapshot.Length).IsEqualTo(1);
        await Assert.That(secondSnapshot.Length).IsEqualTo(2);
    }

    /// <summary>
    /// Verifies that GetSnapshot returns empty array for non-existent key.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetSnapshot_ReturnsEmptyArrayForNonExistentKey()
    {
        // Arrange
        var entries = new Dictionary<string, ArrayHelpers.Entry<int>>();
        ArrayHelpers.Add(entries, "key1", 1);

        // Act
        var snapshot = ArrayHelpers.GetSnapshot(entries, "nonexistent");

        // Assert
        await Assert.That(snapshot).IsNotNull();
        await Assert.That(snapshot.Length).IsEqualTo(0);
    }
}
