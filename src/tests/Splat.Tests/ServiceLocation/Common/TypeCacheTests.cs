// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for TypeCache to verify type caching functionality.</summary>
public class TypeCacheTests
{
    /// <summary>Verifies that TypeCache returns the correct type for each generic argument.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that TypeCache returns the same cached instance for repeated requests of the same type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TypeCache_Should_ReturnSameInstanceForSameType()
    {
        // Act
        var type1 = TypeCache<string>.Type;
        var type2 = TypeCache<string>.Type;

        // Assert - Verify reference equality (caching)
        await Assert.That(ReferenceEquals(type1, type2)).IsTrue();
    }

    /// <summary>Verifies that TypeCache returns different instances for different types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that TypeCache works correctly with nullable value types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that TypeCache works correctly with closed generic types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that TypeCache works correctly with interface types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TypeCache_Should_WorkWithInterfaces()
    {
        // Act
        var ienumerableType = TypeCache<IEnumerable<string>>.Type;

        // Assert
        await Assert.That(ienumerableType).IsEqualTo(typeof(IEnumerable<string>));
    }

    /// <summary>A simple service type used for testing the type cache.</summary>
    private sealed class TestService
    {
        /// <summary>Gets or sets the identifier.</summary>
        public int Id { get; set; }
    }
}
