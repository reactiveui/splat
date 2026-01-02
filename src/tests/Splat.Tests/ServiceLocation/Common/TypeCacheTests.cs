// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for TypeCache to verify type caching functionality.
/// </summary>
public class TypeCacheTests
{
    [Test]
    public async Task TypeCache_Should_ReturnCorrectType()
    {
        // Act
        var stringType = TypeCache<string>.Type;
        var intType = TypeCache<int>.Type;
        var testServiceType = TypeCache<TestService>.Type;

        // Assert
        await Assert.That(stringType).IsEqualTo(typeof(string));
        await Assert.That(intType).IsEqualTo(typeof(int));
        await Assert.That(testServiceType).IsEqualTo(typeof(TestService));
    }

    [Test]
    public async Task TypeCache_Should_ReturnSameInstanceForSameType()
    {
        // Act
        var type1 = TypeCache<string>.Type;
        var type2 = TypeCache<string>.Type;

        // Assert - Verify reference equality (caching)
        await Assert.That(ReferenceEquals(type1, type2)).IsTrue();
    }

    [Test]
    public async Task TypeCache_Should_ReturnDifferentInstancesForDifferentTypes()
    {
        // Act
        var stringType = TypeCache<string>.Type;
        var intType = TypeCache<int>.Type;

        // Assert
        await Assert.That(stringType).IsNotEqualTo(intType);
        await Assert.That(ReferenceEquals(stringType, intType)).IsFalse();
    }

    [Test]
    public async Task TypeCache_Should_WorkWithNullableTypes()
    {
        // Act
        var nullableIntType = TypeCache<int?>.Type;
        var intType = TypeCache<int>.Type;

        // Assert
        await Assert.That(nullableIntType).IsEqualTo(typeof(int?));
        await Assert.That(nullableIntType).IsNotEqualTo(intType);
    }

    [Test]
    public async Task TypeCache_Should_WorkWithGenericTypes()
    {
        // Act
        var listStringType = TypeCache<List<string>>.Type;
        var listIntType = TypeCache<List<int>>.Type;

        // Assert
        await Assert.That(listStringType).IsEqualTo(typeof(List<string>));
        await Assert.That(listIntType).IsEqualTo(typeof(List<int>));
        await Assert.That(listStringType).IsNotEqualTo(listIntType);
    }

    [Test]
    public async Task TypeCache_Should_WorkWithInterfaces()
    {
        // Act
        var ienumerableType = TypeCache<IEnumerable<string>>.Type;

        // Assert
        await Assert.That(ienumerableType).IsEqualTo(typeof(IEnumerable<string>));
    }

    private sealed class TestService
    {
        public int Id { get; set; }
    }
}
