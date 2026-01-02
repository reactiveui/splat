// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>
/// Tests for the Registration&lt;T&gt; struct.
/// </summary>
public class RegistrationTests
{
    [Test]
    public async Task FromInstance_CreatesInstanceRegistration()
    {
        // Arrange
        var instance = "test";

        // Act
        var registration = Registration<string>.FromInstance(instance);

        // Assert
        await Assert.That(registration.IsFactory).IsFalse();
        await Assert.That(registration.GetInstance()).IsEqualTo(instance);
    }

    [Test]
    public async Task FromFactory_CreatesFactoryRegistration()
    {
        // Arrange
        Func<string?> factory = () => "test";

        // Act
        var registration = Registration<string>.FromFactory(factory);

        // Assert
        await Assert.That(registration.IsFactory).IsTrue();
        await Assert.That(registration.GetFactory() == factory).IsTrue();
    }

    [Test]
    public async Task GetInstance_ReturnsCorrectInstance()
    {
        // Arrange
        var instance = 42;
        var registration = Registration<int>.FromInstance(instance);

        // Act
        var result = registration.GetInstance();

        // Assert
        await Assert.That(result).IsEqualTo(instance);
    }

    [Test]
    public async Task GetFactory_ReturnsCorrectFactory()
    {
        // Arrange
        var expectedValue = "factory result";
        Func<string?> factory = () => expectedValue;
        var registration = Registration<string>.FromFactory(factory);

        // Act
        var resultFactory = registration.GetFactory();
        await Assert.That(resultFactory).IsNotNull();
        var result = resultFactory();

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task InstanceRegistration_WithNullValue_StoresNull()
    {
        // Arrange & Act
        var registration = Registration<string?>.FromInstance(null);

        // Assert
        await Assert.That(registration.IsFactory).IsFalse();
        await Assert.That(registration.GetInstance()).IsNull();
    }

    [Test]
    public async Task FactoryRegistration_ThatReturnsNull_WorksCorrectly()
    {
        // Arrange
        Func<string?> factory = () => null;
        var registration = Registration<string?>.FromFactory(factory);

        // Assert factory mode is set
        await Assert.That(registration.IsFactory).IsTrue();

        // Act
        var success = registration.TryGetFactory(out var resultFactory);
        await Assert.That(success).IsTrue();
        await Assert.That(resultFactory is not null).IsTrue();
        var result = resultFactory!();

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task Registration_IsValueType_CanBeCompared()
    {
        // Arrange
        var instance = "test";
        var reg1 = Registration<string>.FromInstance(instance);
        var reg2 = Registration<string>.FromInstance(instance);

        // Act & Assert - struct equality
        await Assert.That(reg1.Equals(reg2)).IsTrue();
    }

    [Test]
    public async Task FactoryRegistration_WithDifferentFactories_AreNotEqual()
    {
        // Arrange
        Func<string?> factory1 = () => "test1";
        Func<string?> factory2 = () => "test2";
        var reg1 = Registration<string>.FromFactory(factory1);
        var reg2 = Registration<string>.FromFactory(factory2);

        // Act & Assert - different factories mean different registrations
        await Assert.That(reg1.Equals(reg2)).IsFalse();
    }

    [Test]
    public async Task Registration_WithComplexType_WorksCorrectly()
    {
        // Arrange
        var complexObject = new TestClass { Value = 42, Name = "Test" };
        var registration = Registration<TestClass>.FromInstance(complexObject);

        // Act
        var result = registration.GetInstance();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo(complexObject);
        await Assert.That(result.Value).IsEqualTo(42);
        await Assert.That(result.Name).IsEqualTo("Test");
    }

    [Test]
    public async Task FactoryRegistration_WithComplexType_WorksCorrectly()
    {
        // Arrange
        Func<TestClass?> factory = () => new() { Value = 100, Name = "Factory" };
        var registration = Registration<TestClass>.FromFactory(factory);

        // Act
        var resultFactory = registration.GetFactory();
        await Assert.That(resultFactory).IsNotNull();
        var result = resultFactory();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Value).IsEqualTo(100);
        await Assert.That(result.Name).IsEqualTo("Factory");
    }

    private sealed class TestClass
    {
        public int Value { get; set; }

        public string? Name { get; set; }
    }
}
