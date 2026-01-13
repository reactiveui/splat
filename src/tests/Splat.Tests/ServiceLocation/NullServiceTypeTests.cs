// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="NullServiceType"/> class.
/// </summary>
public sealed class NullServiceTypeTests
{
    [Test]
    public async Task Constructor_ShouldPreserveFactory()
    {
        var expected = new object();
        Func<object?> factory = () => expected;

        var nullServiceType = new NullServiceType(factory);
        var result = nullServiceType.Factory();

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Factory_ShouldBeInvokable()
    {
        var expected = new object();
        Func<object?> factory = () => expected;

        var nullServiceType = new NullServiceType(factory);
        var result = nullServiceType.Factory();

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Factory_CanReturnNull()
    {
        Func<object?> factory = () => null;

        var nullServiceType = new NullServiceType(factory);
        var result = nullServiceType.Factory();

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task CachedType_ShouldBeNullServiceType()
    {
        var cachedType = NullServiceType.CachedType;

        await Assert.That(cachedType).IsEqualTo(typeof(NullServiceType));
    }

    [Test]
    public async Task CachedType_ShouldBeSameInstance()
    {
        var firstAccess = NullServiceType.CachedType;
        var secondAccess = NullServiceType.CachedType;

        await Assert.That(firstAccess).IsEqualTo(secondAccess);
        await Assert.That(ReferenceEquals(firstAccess, secondAccess)).IsTrue();
    }

    [Test]
    public async Task MultipleInstances_CanHaveDifferentFactories()
    {
        var value1 = "first";
        var value2 = "second";

        var instance1 = new NullServiceType(() => value1);
        var instance2 = new NullServiceType(() => value2);

        await Assert.That(instance1.Factory()).IsEqualTo(value1);
        await Assert.That(instance2.Factory()).IsEqualTo(value2);
    }

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

        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(2);
    }
}
