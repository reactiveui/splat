// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the Memoizing MRU Cache.
/// </summary>
[TestFixture]
public class MemoizingMRUCacheTests
{
    /// <summary>
    /// Checks to ensure an Argument Null Exception is thrown.
    /// </summary>
    [Test]
    public void ThrowsArgumentNullException()
    {
        var instance = GetTestInstance();
        Assert.Throws<ArgumentNullException>(() => instance.Get(null!));
    }

    /// <summary>
    /// Test that constructor throws ArgumentException for invalid max size.
    /// </summary>
    [Test]
    public void Constructor_ThrowsArgumentException_ForInvalidMaxSize()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentException>(() =>
                new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), 0));

            Assert.Throws<ArgumentException>(() =>
                new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), -1));
        }
    }

    /// <summary>
    /// Test that constructor throws ArgumentNullException for null calculation function.
    /// </summary>
    [Test]
    public void Constructor_ThrowsArgumentNullException_ForNullCalculationFunction() => Assert.Throws<ArgumentNullException>(() => new MemoizingMRUCache<string, DummyObjectClass1>(null!, 10));

    /// <summary>
    /// Test that TryGet throws ArgumentNullException for null key.
    /// </summary>
    [Test]
    public void TryGet_ThrowsArgumentNullException_ForNullKey()
    {
        var instance = GetTestInstance();
        Assert.Throws<ArgumentNullException>(() => instance.TryGet(null!, out _));
    }

    /// <summary>
    /// Test that Invalidate throws ArgumentNullException for null key.
    /// </summary>
    [Test]
    public void Invalidate_ThrowsArgumentNullException_ForNullKey()
    {
        var instance = GetTestInstance();
        Assert.Throws<ArgumentNullException>(() => instance.Invalidate(null!));
    }

    /// <summary>
    /// Test that cache evicts old items when max size is reached.
    /// </summary>
    [Test]
    public void Cache_EvictsOldItems_WhenMaxSizeReached()
    {
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            2,
            _ => releaseCount++);

        instance.Get("key1");
        instance.Get("key2");
        instance.Get("key3"); // evicts key1

        using (Assert.EnterMultipleScope())
        {
            Assert.That(releaseCount, Is.EqualTo(1));
            Assert.That(instance.TryGet("key1", out _), Is.False);
            Assert.That(instance.TryGet("key2", out _), Is.True);
            Assert.That(instance.TryGet("key3", out _), Is.True);
        }
    }

    /// <summary>
    /// Test that InvalidateAll with aggregateReleaseExceptions handles exceptions.
    /// </summary>
    [Test]
    public void InvalidateAll_WithAggregateExceptions_HandlesExceptions()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");
        instance.Get("key2");

        var exception = Assert.Throws<AggregateException>(() => instance.InvalidateAll(true));
        Assert.That(exception!.InnerExceptions, Has.Count.EqualTo(2));
    }

    /// <summary>
    /// Test that InvalidateAll without aggregating exceptions throws on first error.
    /// </summary>
    [Test]
    public void InvalidateAll_WithoutAggregateExceptions_ThrowsOnFirstError()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => throw new InvalidOperationException("Release error"));

        instance.Get("key1");

        Assert.Throws<InvalidOperationException>(() => instance.InvalidateAll(false));
    }

    /// <summary>
    /// Test that CachedValues returns current cache contents.
    /// </summary>
    [Test]
    public void CachedValues_ReturnsCurrentCacheContents()
    {
        var instance = GetTestInstance();
        var value1 = instance.Get("key1");
        var value2 = instance.Get("key2");

        var cachedValues = instance.CachedValues().ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(cachedValues, Has.Count.EqualTo(2));
            Assert.That(cachedValues, Does.Contain(value1));
            Assert.That(cachedValues, Does.Contain(value2));
        }
    }

    /// <summary>
    /// Test with custom comparer.
    /// </summary>
    [Test]
    [Ignore("Issue where the type is the same but they are not the same instance")]
    public void Constructor_WithCustomComparer_UsesComparer()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            StringComparer.OrdinalIgnoreCase);

        var value1 = instance.Get("KEY");
        var value2 = instance.Get("key");

        Assert.That(value1, Is.SameAs(value2));
    }

    /// <summary>
    /// Test with custom comparer and release function.
    /// </summary>
    [Test]
    public void Constructor_WithCustomComparerAndReleaseFunction_Works()
    {
        var releaseCount = 0;
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>(
            (_, _) => new(),
            10,
            _ => releaseCount++,
            StringComparer.OrdinalIgnoreCase);

        instance.Get("key1");
        instance.Invalidate("key1");

        Assert.That(releaseCount, Is.EqualTo(1));
    }

    /// <summary>
    /// Test that Get with context parameter works.
    /// </summary>
    [Test]
    public void Get_WithContext_PassesContextToFactory()
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

        Assert.That(receivedContext, Is.SameAs(testContext));
    }

    /// <summary>
    /// Test that Invalidate removes non-existent key gracefully.
    /// </summary>
    [Test]
    public void Invalidate_NonExistentKey_DoesNotThrow()
    {
        var instance = GetTestInstance();
        Assert.DoesNotThrow(() => instance.Invalidate("nonexistent"));
    }

    /// <summary>
    /// Test that InvalidateAll with empty cache works.
    /// </summary>
    [Test]
    public void InvalidateAll_EmptyCache_DoesNotThrow()
    {
        var instance = GetTestInstance();
        Assert.DoesNotThrow(() => instance.InvalidateAll());
    }

    /// <summary>
    /// Test that InvalidateAll with null release function works.
    /// </summary>
    [Test]
    public void InvalidateAll_NullReleaseFunction_Works()
    {
        var instance = new MemoizingMRUCache<string, DummyObjectClass1>((_, _) => new(), 10);
        instance.Get("key1");

        Assert.DoesNotThrow(() => instance.InvalidateAll());
    }

    /// <summary>
    /// Test that TryGet returns false for non-existent key.
    /// </summary>
    [Test]
    public void TryGet_NonExistentKey_ReturnsFalse()
    {
        var instance = GetTestInstance();

        var result = instance.TryGet("nonexistent", out var value);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.False);
            Assert.That(value, Is.Null);
        }
    }

    /// <summary>
    /// Checks to ensure a value is returned.
    /// </summary>
    [Test]
    public void ReturnsValue()
    {
        var instance = GetTestInstance();
        var result = instance.Get("Test1");
        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    [Test]
    public void GetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test1");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result1, Is.SameAs(result2));
        }
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    [Test]
    public void GetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test2");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result1, Is.Not.SameAs(result2));
        }
    }

    /// <summary>
    /// Checks to ensure a value is returned for 2 duplicate calls.
    /// </summary>
    [Test]
    public void TryGetReturnsSameValue()
    {
        var instance = GetTestInstance();
        var result1 = instance.Get("Test1");
        instance.TryGet("Test1", out var result2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result1, Is.SameAs(result2));
        }
    }

    /// <summary>
    /// Checks to ensure 2 different values are returned for 2 different calls.
    /// </summary>
    [Test]
    public void TryGetReturnsDifferentValues()
    {
        var instance = GetTestInstance();
        var p1 = instance.Get("Test1");
        var p2 = instance.Get("Test2");

        var result1 = instance.Get("Test1");
        var result2 = instance.Get("Test2");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result1, Is.Not.SameAs(result2));
            Assert.That(result1, Is.SameAs(p1));
            Assert.That(result2, Is.SameAs(p2));
        }
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get.
    /// </summary>
    [Test]
    public void ThreadSafeRetrievalTest()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        var results = tests.AsParallel().Select(_ => instance.Get("Test1")).ToList();
        var first = results.First();

        foreach (var item in results)
        {
            Assert.That(item, Is.SameAs(first));
        }
    }

    /// <summary>
    /// Crude test for checking thread safety when using Get and TryGet.
    /// </summary>
    [Test]
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

        foreach (var item in results)
        {
            if (item is not null)
            {
                Assert.That(item, Is.SameAs(first));
            }
        }
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    [Test]
    public void GetsResultsFromCacheValuesWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<IEnumerable<DummyObjectClass1>>? results = null;

        Assert.DoesNotThrow(() =>
        {
            results = tests.AsParallel().Select(_ =>
            {
                instance.Invalidate("Test1");
                instance.Get("Test1");
                return instance.CachedValues();
            }).ToList();
        });

        Assert.That(results, Is.Not.Null);
    }

    /// <summary>
    /// Check that invalidate plays nicely.
    /// </summary>
    [Test]
    public void GetsResultsWhenInvalidateAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<DummyObjectClass1>? results = null;

        Assert.DoesNotThrow(() =>
        {
            results = tests.AsParallel().Select(i =>
            {
                instance.Invalidate("Test1");
                var result = instance.Get("Test1");

                // Also exercise CachedValues
                _ = instance.CachedValues();
                return result;
            }).ToList();
        });

        Assert.That(results, Is.Not.Null);
    }

    /// <summary>
    /// Check that invalidate all plays nicely.
    /// </summary>
    [Test]
    public void GetsResultsWhenInvalidateAllAndGetAreUsed()
    {
        var instance = GetTestInstance();
        var tests = Enumerable.Range(0, 100);

        List<DummyObjectClass1>? results = null;

        Assert.DoesNotThrow(() =>
        {
            results = tests.AsParallel().Select(_ =>
            {
                instance.InvalidateAll();
                return instance.Get("Test1");
            }).ToList();
        });

        Assert.That(results, Is.Not.Null);
    }

    private static MemoizingMRUCache<string, DummyObjectClass1> GetTestInstance() =>
        new((_, _) => new(), 256);
}
