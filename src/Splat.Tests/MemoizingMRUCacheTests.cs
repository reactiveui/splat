// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the Memoizing MRU Cache.
/// </summary>
public class MemoizingMRUCacheTests
{
    /// <summary>
    /// Checks to ensure an Argument Null Exception is thrown.
    /// </summary>
    [Fact]
    public void ThrowsArgumentNullException()
    {
        var instance = GetTestInstance();
        Assert.Throws<ArgumentNullException>(() => instance.Get(null!));
    }

    /// <summary>
    /// Test that constructor throws ArgumentException for invalid max size.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsArgumentException_ForInvalidMaxSize()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), 0));
        Assert.Throws<ArgumentException>(() => new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), -1));
    }

    /// <summary>
    /// Test that constructor throws ArgumentNullException for null calculation function.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsArgumentNullException_ForNullCalculationFunction()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MemoizingMRUCache<string, DummyObjectClass1>(null!, 10));
    }

    /// <summary>
    /// Test that TryGet throws ArgumentNullException for null key.
    /// </summary>
    [Fact]
    public void TryGet_ThrowsArgumentNullException_ForNullKey()
    {
        // Arrange
        var instance = GetTestInstance();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => instance.TryGet(null!, out _));
    }

    /// <summary>
    /// Test that Invalidate throws ArgumentNullException for null key.
    /// </summary>
    [Fact]
    public void Invalidate_ThrowsArgumentNullException_ForNullKey()
    {
        // Arrange
        var instance = GetTestInstance();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => instance.Invalidate(null!));
    }

    /// <summary>
    /// Test that cache evicts old items when max size is reached.
    /// </summary>
    [Fact]
    public void Cache_EvictsOldItems_WhenMaxSizeReached()
    {
        // Arrange
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            2, // Small cache size
            _ => releaseCount++);

        // Act
        instance.Get("key1");
        instance.Get("key2");
        instance.Get("key3"); // This should evict key1

        // Assert
        Assert.Equal(1, releaseCount);
        Assert.False(instance.TryGet("key1", out _));
        Assert.True(instance.TryGet("key2", out _));
        Assert.True(instance.TryGet("key3", out _));
    }

    /// <summary>
    /// Test that InvalidateAll with aggregateReleaseExceptions handles exceptions.
    /// </summary>
    [Fact]
    public void InvalidateAll_WithAggregateExceptions_HandlesExceptions()
    {
        // Arrange
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");
        instance.Get("key2");

        // Act & Assert
        var exception = Assert.Throws<AggregateException>(() => instance.InvalidateAll(true));
        Assert.Equal(2, exception.InnerExceptions.Count);
    }

    /// <summary>
    /// Test that InvalidateAll without aggregating exceptions throws on first error.
    /// </summary>
    [Fact]
    public void InvalidateAll_WithoutAggregateExceptions_ThrowsOnFirstError()
    {
        // Arrange
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => instance.InvalidateAll(false));
    }

    /// <summary>
    /// Test that CachedValues returns current cache contents.
    /// </summary>
    [Fact]
    public void CachedValues_ReturnsCurrentCacheContents()
    {
        // Arrange
        var instance = GetTestInstance();
        var value1 = instance.Get("key1");
        var value2 = instance.Get("key2");

        // Act
        var cachedValues = instance.CachedValues().ToList();

        // Assert
        Assert.Equal(2, cachedValues.Count);
        Assert.Contains(value1, cachedValues);
        Assert.Contains(value2, cachedValues);
    }

    /// <summary>
    /// Test with custom comparer.
    /// </summary>
    [Fact(Skip = "Issue where the type is the same but they are not the same instance")]
    public void Constructor_WithCustomComparer_UsesComparer()
    {
        // Arrange
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            10,
            StringComparer.OrdinalIgnoreCase);

        // Act
        var value1 = instance.Get("KEY");
        var value2 = instance.Get("key");

        // Assert - Should be same object due to case-insensitive comparer
        Assert.Same(value1, value2);
    }

    /// <summary>
    /// Test with custom comparer and release function.
    /// </summary>
    [Fact]
    public void Constructor_WithCustomComparerAndReleaseFunction_Works()
    {
        // Arrange
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            10,
            _ => releaseCount++,
            StringComparer.OrdinalIgnoreCase);

        // Act
        instance.Get("key1");
        instance.Invalidate("key1");

        // Assert
        Assert.Equal(1, releaseCount);
    }

    /// <summary>
    /// Test that Get with context parameter works.
    /// </summary>
    [Fact]
    public void Get_WithContext_PassesContextToFactory()
    {
        // Arrange
        object? receivedContext = null;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, context) =>
            {
                receivedContext = context;
                return new();
            },
            10);

        var testContext = new object();

        // Act
        instance.Get("key1", testContext);

        // Assert
        Assert.Same(testContext, receivedContext);
    }

    /// <summary>
    /// Test that Invalidate removes non-existent key gracefully.
    /// </summary>
    [Fact]
    public void Invalidate_NonExistentKey_DoesNotThrow()
    {
        // Arrange
        var instance = GetTestInstance();

        // Act & Assert - should not throw
        instance.Invalidate("nonexistent");
    }

    /// <summary>
    /// Test that InvalidateAll with empty cache works.
    /// </summary>
    [Fact]
    public void InvalidateAll_EmptyCache_DoesNotThrow()
    {
        // Arrange
        var instance = GetTestInstance();

        // Act & Assert - should not throw
        instance.InvalidateAll();
    }

    /// <summary>
    /// Test that InvalidateAll with null release function works.
    /// </summary>
    [Fact]
    public void InvalidateAll_NullReleaseFunction_Works()
    {
        // Arrange
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (param, _) => new(),
            10);

        instance.Get("key1");

        // Act & Assert - should not throw
        instance.InvalidateAll();
    }

    /// <summary>
    /// Test that TryGet returns false for non-existent key.
    /// </summary>
    [Fact]
    public void TryGet_NonExistentKey_ReturnsFalse()
    {
        // Arrange
        var instance = GetTestInstance();

        // Act
        var result = instance.TryGet("nonexistent", out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    /// <summary>
    /// Checks to ensure a value is returned.
    /// </summary>
    [Fact]
    public void ReturnsValue()
    {
        var instance = GetTestInstance();
        var result = instance.Get("Test1");
        Assert.NotNull(result);
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    [Fact]
    public void GetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        Assert.NotNull(result1);
        var result2 = instance.Get("Test1");
        Assert.NotNull(result2);
        Assert.Same(result1, result2);
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    [Fact]
    public void GetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        Assert.NotNull(result1);
        var result2 = instance.Get("Test2");
        Assert.NotNull(result2);
        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    [Fact]
    public void TryGetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        Assert.NotNull(result1);
        instance.TryGet("Test1", out var result2);
        Assert.NotNull(result2);
        Assert.Same(result1, result2);
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    [Fact]
    public void TryGetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var p1 = instance.Get("Test1");
        var p2 = instance.Get("Test2");

        var result1 = instance.Get("Test1");
        Assert.NotNull(result1);

        var result2 = instance.Get("Test2");
        Assert.NotNull(result2);

        Assert.NotSame(result1, result2);
        Assert.Same(p1, result1);
        Assert.Same(p2, result2);
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get.
    /// </summary>
    [Fact]
    public void ThreadSafeRetrievalTest()
    {
        var instance = GetTestInstance();

        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(_ => instance.Get("Test1")).ToList();

        var first = results.First();

        foreach (var dummyObjectClass1 in results)
        {
            Assert.Same(first, dummyObjectClass1);
        }
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get and TryGet.
    /// </summary>
    [Fact]
    public void ThreadSafeRetrievalTestWithGetAndTryGet()
    {
        var instance = GetTestInstance();

        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(i =>
        {
            if (i % 2 == 0)
            {
                return instance.Get("Test1");
            }

            instance.TryGet("Test1", out var result);
            return result;
        }).ToList();

        var first = results.First(x => x is not null);

        foreach (var dummyObjectClass1 in results)
        {
            if (dummyObjectClass1 is not null)
            {
                Assert.Same(first, dummyObjectClass1);
            }
        }
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    [Fact]
    public void GetsResultsFromCacheValuesWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();

        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(i =>
        {
            instance.Invalidate("Test1");
            instance.Get("Test1");

            return instance.CachedValues();
        }).ToList();

        // This is actually a crude test of a misuse case.
        // There's no guarantee what comes out is entirely different for every task\thread.
        // Just make sure it doesn't throw an exception.
        Assert.NotNull(results);
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    [Fact]
    public void GetsResultsWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();

        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(i =>
        {
            instance.Invalidate("Test1");
            var result = instance.Get("Test1");

            // this is here just simply to test cache values plays nicely as well.
            var cachedValues = instance.CachedValues();
            return result;
        }).ToList();

        // This is actually a crude test of a misuse case.
        // There's no guarantee what comes out is entirely different for every task\thread.
        // Just make sure it doesn't throw an exception.
        Assert.NotNull(results);
    }

    /// <summary>
    /// Check that invalidate all plays nicely.
    /// </summary>
    [Fact]
    public void GetsResultsWhenInvalidateAllAndGetAreUsed()
    {
        var instance = GetTestInstance();

        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(i =>
        {
            instance.InvalidateAll();
            return instance.Get("Test1");
        }).ToList();

        // This is actually a crude test of a misuse case.
        // There's no guarantee what comes out is entirely different for every task\thread.
        // Just make sure it doesn't throw an exception.
        Assert.NotNull(results);
    }

    private static MemoizingMRUCache<string, DummyObjectClass1> GetTestInstance() =>
        new(
            (param, o) => new(),
            256);
}
