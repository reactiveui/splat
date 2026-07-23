// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>Tests for the Registration&lt;T&gt; struct.</summary>
public class RegistrationTests
{
    /// <summary>A sample value used across the mode-conversion tests.</summary>
    private const string SampleValue = "value";

    /// <summary>Tests that from instance creates instance registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromInstance_CreatesInstanceRegistration()
    {
        // Arrange
        const string instance = "test";

        // Act
        var registration = Registration<string>.FromInstance(instance);

        // Assert
        await Assert.That(registration.IsFactory).IsFalse();
        await Assert.That(registration.GetInstance()).IsEqualTo(instance);
    }

    /// <summary>Tests that from factory creates factory registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromFactory_CreatesFactoryRegistration()
    {
        // Arrange
        Func<string?> factory = static () => "test";

        // Act
        var registration = Registration<string>.FromFactory(factory);

        // Assert
        await Assert.That(registration.IsFactory).IsTrue();
        await Assert.That(registration.GetFactory() == factory).IsTrue();
    }

    /// <summary>Tests that get instance returns correct instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetInstance_ReturnsCorrectInstance()
    {
        // Arrange
        const int instance = 42;
        var registration = Registration<int>.FromInstance(instance);

        // Act
        var result = registration.GetInstance();

        // Assert
        await Assert.That(result).IsEqualTo(instance);
    }

    /// <summary>Tests that get factory returns correct factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetFactory_ReturnsCorrectFactory()
    {
        // Arrange
        const string expectedValue = "factory result";
        Func<string?> factory = static () => expectedValue;
        var registration = Registration<string>.FromFactory(factory);

        // Act
        var resultFactory = registration.GetFactory();
        await Assert.That(resultFactory).IsNotNull();
        var result = resultFactory();

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    /// <summary>Tests that instance registration with null value stores null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InstanceRegistration_WithNullValue_StoresNull()
    {
        // Arrange & Act
        var registration = Registration<string?>.FromInstance(null);

        // Assert
        await Assert.That(registration.IsFactory).IsFalse();
        await Assert.That(registration.GetInstance()).IsNull();
    }

    /// <summary>Tests that factory registration that returns null works correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FactoryRegistration_ThatReturnsNull_WorksCorrectly()
    {
        // Arrange
        Func<string?> factory = static () => null;
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

    /// <summary>Tests that registration is value type can be compared.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Registration_IsValueType_CanBeCompared()
    {
        // Arrange
        const string instance = "test";
        var reg1 = Registration<string>.FromInstance(instance);
        var reg2 = Registration<string>.FromInstance(instance);

        // Act & Assert - struct equality
        await Assert.That(reg1.Equals(reg2)).IsTrue();
    }

    /// <summary>Tests that factory registration with different factories are not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FactoryRegistration_WithDifferentFactories_AreNotEqual()
    {
        // Arrange
        Func<string?> factory1 = static () => "test1";
        Func<string?> factory2 = static () => "test2";
        var reg1 = Registration<string>.FromFactory(factory1);
        var reg2 = Registration<string>.FromFactory(factory2);

        // Act & Assert - different factories mean different registrations
        await Assert.That(reg1.Equals(reg2)).IsFalse();
    }

    /// <summary>Tests that registration with complex type works correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Registration_WithComplexType_WorksCorrectly()
    {
        // Arrange
        const int expectedValue = 42;
        var complexObject = new TestClass { Value = expectedValue, Name = "Test" };
        var registration = Registration<TestClass>.FromInstance(complexObject);

        // Act
        var result = registration.GetInstance();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo(complexObject);
        await Assert.That(result.Value).IsEqualTo(expectedValue);
        await Assert.That(result.Name).IsEqualTo("Test");
    }

    /// <summary>Tests that factory registration with complex type works correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FactoryRegistration_WithComplexType_WorksCorrectly()
    {
        // Arrange
        const int expectedValue = 100;
        Func<TestClass?> factory = static () => new() { Value = expectedValue, Name = "Factory" };
        var registration = Registration<TestClass>.FromFactory(factory);

        // Act
        var resultFactory = registration.GetFactory();
        await Assert.That(resultFactory).IsNotNull();
        var result = resultFactory();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Value).IsEqualTo(expectedValue);
        await Assert.That(result.Name).IsEqualTo("Factory");
    }

    /// <summary>Tests that <see cref="Registration{T}.GetInstance"/> returns default when the registration is a factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetInstance_OnFactoryRegistration_ReturnsDefault()
    {
        // Arrange
        var registration = Registration<string?>.FromFactory(static () => SampleValue);

        // Act
        var result = registration.GetInstance();

        // Assert - factory-mode registrations do not carry an instance.
        await Assert.That(result).IsNull();
    }

    /// <summary>Tests that <see cref="Registration{T}.GetFactory"/> returns null when the registration is an instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetFactory_OnInstanceRegistration_ReturnsNull()
    {
        // Arrange
        var registration = Registration<string>.FromInstance(SampleValue);

        // Act
        var result = registration.GetFactory();

        // Assert - instance-mode registrations do not carry a factory.
        await Assert.That(result is null).IsTrue();
    }

    /// <summary>Tests that <see cref="Registration{T}.TryGetInstance"/> succeeds for an instance registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGetInstance_OnInstanceRegistration_ReturnsTrueWithInstance()
    {
        // Arrange
        var registration = Registration<string>.FromInstance(SampleValue);

        // Act
        var success = registration.TryGetInstance(out var result);

        // Assert
        using (Assert.Multiple())
        {
            await Assert.That(success).IsTrue();
            await Assert.That(result).IsEqualTo(SampleValue);
        }
    }

    /// <summary>Tests that <see cref="Registration{T}.TryGetInstance"/> fails for a factory registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TryGetInstance_OnFactoryRegistration_ReturnsFalse()
    {
        // Arrange
        var registration = Registration<string?>.FromFactory(static () => SampleValue);

        // Act
        var success = registration.TryGetInstance(out var result);

        // Assert
        using (Assert.Multiple())
        {
            await Assert.That(success).IsFalse();
            await Assert.That(result is null).IsTrue();
        }
    }

    /// <summary>A simple reference type used for testing registrations.</summary>
    private sealed class TestClass
    {
        /// <summary>Gets or sets the value.</summary>
        public int Value { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string? Name { get; set; }
    }
}
