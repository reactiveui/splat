// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
/// </summary>
public class MemoizingMRUCacheTests
{
    /// <summary>
    /// Checks to ensure an Argument Null Exception is thrown.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThrowsArgumentNullException()
    {
        var instance = GetTestInstance();
        await Assert.That(() => instance.Get(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test that constructor throws ArgumentException for invalid max size.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ThrowsArgumentException_ForInvalidMaxSize()
    {
        using (Assert.Multiple())
        {
            await Assert.That(() =>
                new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), 0)).Throws<ArgumentOutOfRangeException>();

            await Assert.That(() =>
                new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), -1)).Throws<ArgumentOutOfRangeException>();
        }
    }

    /// <summary>
    /// Test that constructor throws ArgumentNullException for null calculation function.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ThrowsArgumentNullException_ForNullCalculationFunction() => await Assert.That(() => new MemoizingMRUCache<string, DummyObjectClass1>(null!, 10)).Throws<ArgumentNullException>();

    /// <summary>
    /// Test that TryGet throws ArgumentNullException for null key.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGet_ThrowsArgumentNullException_ForNullKey()
    {
        var instance = GetTestInstance();
        await Assert.That(() => instance.TryGet(null!, out _)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test that Invalidate throws ArgumentNullException for null key.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Invalidate_ThrowsArgumentNullException_ForNullKey()
    {
        var instance = GetTestInstance();
        await Assert.That(() => instance.Invalidate(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test that cache evicts old items when max size is reached.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Cache_EvictsOldItems_WhenMaxSizeReached()
    {
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            2,
            _ => releaseCount++);

        instance.Get("key1");
        instance.Get("key2");
        instance.Get("key3"); // evicts key1

        using (Assert.Multiple())
        {
            await Assert.That(releaseCount).IsEqualTo(1);
            await Assert.That(instance.TryGet("key1", out _)).IsFalse();
            await Assert.That(instance.TryGet("key2", out _)).IsTrue();
            await Assert.That(instance.TryGet("key3", out _)).IsTrue();
        }
    }

    /// <summary>
    /// Test that InvalidateAll with aggregateReleaseExceptions handles exceptions.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidateAll_WithAggregateExceptions_HandlesExceptions()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");
        instance.Get("key2");

        var exception = await Assert.That(() => instance.InvalidateAll(true)).Throws<AggregateException>();
        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.InnerExceptions).Count().IsEqualTo(2);
    }

    /// <summary>
    /// Test that InvalidateAll without aggregating exceptions throws on first error.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidateAll_WithoutAggregateExceptions_ThrowsOnFirstError()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");

        await Assert.That(() => instance.InvalidateAll(false)).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Test that CachedValues returns current cache contents.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CachedValues_ReturnsCurrentCacheContents()
    {
        var instance = GetTestInstance();
        var value1 = instance.Get("key1");
        var value2 = instance.Get("key2");

        var cachedValues = instance.CachedValues().ToList();

        using (Assert.Multiple())
        {
            await Assert.That(cachedValues).Count().IsEqualTo(2);
            await Assert.That(cachedValues).Contains(value1);
            await Assert.That(cachedValues).Contains(value2);
        }
    }

    /// <summary>
    /// Test with custom comparer.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithCustomComparer_UsesComparer()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            StringComparer.OrdinalIgnoreCase);

        var value1 = instance.Get("KEY");
        var value2 = instance.Get("key");

        await Assert.That(value1).IsSameReferenceAs(value2);
    }

    /// <summary>
    /// Test with custom comparer and release function.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithCustomComparerAndReleaseFunction_Works()
    {
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => releaseCount++,
            StringComparer.OrdinalIgnoreCase);

        instance.Get("key1");
        instance.Invalidate("key1");

        await Assert.That(releaseCount).IsEqualTo(1);
    }

    /// <summary>
    /// Test that Get with context parameter works.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Get_WithContext_PassesContextToFactory()
    {
        object? receivedContext = null;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, context) =>
            {
                receivedContext = context;
                return new();
            },
            10);

        var testContext = new object();

        instance.Get("key1", testContext);

        await Assert.That(receivedContext).IsSameReferenceAs(testContext);
    }

    /// <summary>
    /// Test that Invalidate removes non-existent key gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Invalidate_NonExistentKey_DoesNotThrow()
    {
        var instance = GetTestInstance();
        await Assert.That(() => instance.Invalidate("nonexistent")).ThrowsNothing();
    }

    /// <summary>
    /// Test that InvalidateAll with empty cache works.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidateAll_EmptyCache_DoesNotThrow()
    {
        var instance = GetTestInstance();
        await Assert.That(() => instance.InvalidateAll()).ThrowsNothing();
    }

    /// <summary>
    /// Test that InvalidateAll with null release function works.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidateAll_NullReleaseFunction_Works()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), 10);
        instance.Get("key1");

        await Assert.That(() => instance.InvalidateAll()).ThrowsNothing();
    }

    /// <summary>
    /// Test that TryGet returns false for non-existent key.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGet_NonExistentKey_ReturnsFalse()
    {
        var instance = GetTestInstance();

        var result = instance.TryGet("nonexistent", out var value);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsFalse();
            await Assert.That(value).IsNull();
        }
    }

    /// <summary>
    /// Checks to ensure a value is returned.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ReturnsValue()
    {
        var instance = GetTestInstance();
        var result = instance.Get("Test1");
        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test1");

        using (Assert.Multiple())
        {
            await Assert.That(result1).IsNotNull();
            await Assert.That(result2).IsNotNull();
            await Assert.That(result1).IsSameReferenceAs(result2);
        }
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test2");

        using (Assert.Multiple())
        {
            await Assert.That(result1).IsNotNull();
            await Assert.That(result2).IsNotNull();
            await Assert.That(result1).IsNotSameReferenceAs(result2);
        }
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        instance.TryGet("Test1", out var result2);

        using (Assert.Multiple())
        {
            await Assert.That(result1).IsNotNull();
            await Assert.That(result2).IsNotNull();
            await Assert.That(result1).IsSameReferenceAs(result2);
        }
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var p1 = instance.Get("Test1");
        var p2 = instance.Get("Test2");

        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test2");

        using (Assert.Multiple())
        {
            await Assert.That(result1).IsNotNull();
            await Assert.That(result2).IsNotNull();
            await Assert.That(result1).IsNotSameReferenceAs(result2);
            await Assert.That(result1).IsSameReferenceAs(p1);
            await Assert.That(result2).IsSameReferenceAs(p2);
        }
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThreadSafeRetrievalTest()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(_ => instance.Get("Test1")).ToList();
        var first = results.First();

        foreach (var item in results)
        {
            await Assert.That(item).IsSameReferenceAs(first);
        }
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get and TryGet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThreadSafeRetrievalTestWithGetAndTryGet()
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

        foreach (var item in results)
        {
            if (item is not null)
            {
                await Assert.That(item).IsSameReferenceAs(first);
            }
        }
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetsResultsFromCacheValuesWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<IEnumerable<DummyObjectClass1>>? results = null;

        await Assert.That(() => results = tests.AsParallel().Select(_ =>
            {
                instance.Invalidate("Test1");
                instance.Get("Test1");
                return instance.CachedValues();
            }).ToList()).ThrowsNothing();

        await Assert.That(results).IsNotNull();
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetsResultsWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<DummyObjectClass1>? results = null;

        await Assert.That(() => results = tests.AsParallel().Select(i =>
            {
                instance.Invalidate("Test1");
                var result = instance.Get("Test1");

                // Also exercise CachedValues
                _ = instance.CachedValues();
                return result;
            }).ToList()).ThrowsNothing();

        await Assert.That(results).IsNotNull();
    }

    /// <summary>
    /// Check that invalidate all plays nicely.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetsResultsWhenInvalidateAllAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<DummyObjectClass1>? results = null;

        await Assert.That(() => results = tests.AsParallel().Select(_ =>
            {
                instance.InvalidateAll();
                return instance.Get("Test1");
            }).ToList()).ThrowsNothing();

        await Assert.That(results).IsNotNull();
    }

    private static MemoizingMRUCache<string, DummyObjectClass1> GetTestInstance() =>
        new((_, _) => new(), 256);
}
