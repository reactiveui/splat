// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="NullServiceType"/> class.</summary>
public sealed class NullServiceTypeTests
{
    /// <summary>Verifies that the constructor preserves the supplied factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ShouldPreserveFactory()
    {
        var expected = new object();
        Func<object?> factory = () => expected;

        var nullServiceType = new NullServiceType(factory);

        // The constructor should store the exact same delegate instance that was supplied.
        await Assert.That(ReferenceEquals(nullServiceType.Factory, factory)).IsTrue();
    }

    /// <summary>Verifies that the Factory delegate can be invoked.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Factory_ShouldBeInvokable()
    {
        var expected = new object();
        Func<object?> factory = () => expected;

        var nullServiceType = new NullServiceType(factory);
        var result = nullServiceType.Factory();

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>Verifies that the Factory delegate may return null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Factory_CanReturnNull()
    {
        Func<object?> factory = () => null;

        var nullServiceType = new NullServiceType(factory);
        var result = nullServiceType.Factory();

        await Assert.That(result).IsNull();
    }

    /// <summary>Verifies that CachedType is the NullServiceType type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CachedType_ShouldBeNullServiceType()
    {
        var cachedType = NullServiceType.CachedType;

        await Assert.That(cachedType).IsEqualTo(typeof(NullServiceType));
    }

    /// <summary>Verifies that CachedType returns the same instance on repeated access.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CachedType_ShouldBeSameInstance()
    {
        var firstAccess = NullServiceType.CachedType;
        var secondAccess = NullServiceType.CachedType;

        await Assert.That(firstAccess).IsEqualTo(secondAccess);
        await Assert.That(ReferenceEquals(firstAccess, secondAccess)).IsTrue();
    }

    /// <summary>Verifies that separate instances retain independent factories.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MultipleInstances_CanHaveDifferentFactories()
    {
        const string value1 = "first";
        const string value2 = "second";

        var instance1 = new NullServiceType(() => value1);
        var instance2 = new NullServiceType(() => value2);

        await Assert.That(instance1.Factory()).IsEqualTo(value1);
        await Assert.That(instance2.Factory()).IsEqualTo(value2);
    }

    /// <summary>Verifies that the Factory delegate can capture closure state.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Factory_CanBeClosure()
    {
        var counter = 0;
        var nullServiceType = new NullServiceType(() =>
        {
            counter++;
            return counter;
        });

        var result1 = nullServiceType.Factory();
        var result2 = nullServiceType.Factory();

        const int firstInvocationCount = 1;
        const int secondInvocationCount = 2;
        await Assert.That(result1).IsEqualTo(firstInvocationCount);
        await Assert.That(result2).IsEqualTo(secondInvocationCount);
    }
}
