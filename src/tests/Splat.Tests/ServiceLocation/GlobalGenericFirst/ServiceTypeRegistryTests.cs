// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>
/// Tests for the ServiceTypeRegistry class.
/// </summary>
[NotInParallel] // ServiceTypeRegistry is static, tests must run sequentially
public class ServiceTypeRegistryTests
{
    [Before(HookType.Test)]
    public void Setup()
    {
        // Clear the registry before each test
        ServiceTypeRegistry.Clear();
    }

    [After(HookType.Test)]
    public void Cleanup()
    {
        // Clear the registry after each test
        ServiceTypeRegistry.Clear();
    }

    [Test]
    public async Task TrackNonGenericRegistration_TracksType()
    {
        // Act
        ServiceTypeRegistry.TrackNonGenericRegistration(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsTrue();
    }

    [Test]
    public async Task TrackNonGenericRegistration_WithContract_TracksTypeAndContract()
    {
        // Act
        ServiceTypeRegistry.TrackNonGenericRegistration(typeof(string), "contract");

        // Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string), "contract")).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsFalse();
    }

    [Test]
    public async Task HasNonGenericRegistrations_WhenNotTracked_ReturnsFalse()
    {
        // Act & Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsFalse();
    }

    [Test]
    public async Task Register_StoresFactory()
    {
        // Arrange
        var expectedValue = "test";
        Func<object?> factory = () => expectedValue;

        // Act
        ServiceTypeRegistry.Register(typeof(string), factory);
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task Register_WithContract_StoresFactoryUnderContract()
    {
        // Arrange
        var expectedValue = "contract value";
        Func<object?> factory = () => expectedValue;

        // Act
        ServiceTypeRegistry.Register(typeof(string), factory, "contract");
        var result = ServiceTypeRegistry.GetService(typeof(string), "contract");

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task Register_MultipleForSameType_ReturnsLatest()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(string), () => "second");
        ServiceTypeRegistry.Register(typeof(string), () => "third");

        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo("third");
    }

    [Test]
    public async Task GetService_WhenEmpty_ReturnsNull()
    {
        // Act
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetService_WithWrongContract_ReturnsNull()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "test", "contract1");

        // Act
        var result = ServiceTypeRegistry.GetService(typeof(string), "contract2");

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetService_InvokesFactoryEachTime()
    {
        // Arrange
        var invocationCount = 0;
        ServiceTypeRegistry.Register(
            typeof(int),
            () =>
            {
                invocationCount++;
                return invocationCount;
            });

        // Act
        var result1 = ServiceTypeRegistry.GetService(typeof(int));
        var result2 = ServiceTypeRegistry.GetService(typeof(int));

        // Assert
        await Assert.That(invocationCount).IsEqualTo(2);
        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(2);
    }

    [Test]
    public async Task GetServices_WhenEmpty_ReturnsEmptyArray()
    {
        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    [Test]
    public async Task GetServices_WithMultipleRegistrations_ReturnsAllValues()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(string), () => "second");
        ServiceTypeRegistry.Register(typeof(string), () => "third");

        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
        await Assert.That(result[2]).IsEqualTo("third");
    }

    [Test]
    public async Task GetServices_OnlyReturnsMatchingContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "no-contract-1");
        ServiceTypeRegistry.Register(typeof(string), () => "no-contract-2");
        ServiceTypeRegistry.Register(typeof(string), () => "contract-1", "contract");

        // Act
        var resultNoContract = ServiceTypeRegistry.GetServices(typeof(string));
        var resultWithContract = ServiceTypeRegistry.GetServices(typeof(string), "contract");

        // Assert
        await Assert.That(resultNoContract.Length).IsEqualTo(2);
        await Assert.That(resultWithContract.Length).IsEqualTo(1);
        await Assert.That(resultWithContract[0]).IsEqualTo("contract-1");
    }

    [Test]
    public async Task GetServices_FiltersOutNullValues()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "valid");
        ServiceTypeRegistry.Register(typeof(string), () => null);
        ServiceTypeRegistry.Register(typeof(string), () => "another valid");

        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo("valid");
        await Assert.That(result[1]).IsEqualTo("another valid");
    }

    [Test]
    public async Task HasRegistration_WhenEmpty_ReturnsFalse()
    {
        // Act & Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
    }

    [Test]
    public async Task HasRegistration_AfterRegistering_ReturnsTrue()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), () => "test");

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsTrue();
    }

    [Test]
    public async Task HasRegistration_WithContract_OnlyReturnsTrueForMatchingContract()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), () => "test", "contract");

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), "contract")).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
    }

    [Test]
    public async Task UnregisterCurrent_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));
    }

    [Test]
    public async Task UnregisterCurrent_WithSingleItem_ClearsRegistration()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "test");

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.GetService(typeof(string))).IsNull();
    }

    [Test]
    public async Task UnregisterCurrent_WithMultipleItems_RemovesLatest()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(string), () => "second");
        ServiceTypeRegistry.Register(typeof(string), () => "third");

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo("second");
    }

    [Test]
    public async Task UnregisterCurrent_OnlyAffectsSpecifiedContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "no contract");
        ServiceTypeRegistry.Register(typeof(string), () => "with contract", "contract");

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), "contract")).IsTrue();
    }

    [Test]
    public async Task UnregisterAll_RemovesAllRegistrationsForType()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(string), () => "second");
        ServiceTypeRegistry.Register(typeof(string), () => "third");

        // Act
        ServiceTypeRegistry.UnregisterAll(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        var result = ServiceTypeRegistry.GetServices(typeof(string));
        await Assert.That(result.Length).IsEqualTo(0);
    }

    [Test]
    public async Task UnregisterAll_OnlyAffectsSpecifiedTypeAndContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "string1");
        ServiceTypeRegistry.Register(typeof(int), () => 42);
        ServiceTypeRegistry.Register(typeof(string), () => "string-contract", "contract");

        // Act
        ServiceTypeRegistry.UnregisterAll(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(int))).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), "contract")).IsTrue();
    }

    [Test]
    public async Task Clear_RemovesAllRegistrations()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "string");
        ServiceTypeRegistry.Register(typeof(int), () => 42);
        ServiceTypeRegistry.Register(typeof(string), () => "contract", "contract");

        // Act
        ServiceTypeRegistry.Clear();

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(int))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), "contract")).IsFalse();
    }

    [Test]
    public async Task Clear_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        ServiceTypeRegistry.Clear();
    }

    [Test]
    public async Task Registry_WithDifferentTypes_AreIsolated()
    {
        // Arrange & Act
        ServiceTypeRegistry.Register(typeof(string), () => "string value");
        ServiceTypeRegistry.Register(typeof(int), () => 42);
        ServiceTypeRegistry.Register(typeof(double), () => 3.14);

        // Assert
        await Assert.That(ServiceTypeRegistry.GetService(typeof(string))).IsEqualTo("string value");
        await Assert.That(ServiceTypeRegistry.GetService(typeof(int))).IsEqualTo(42);
        await Assert.That(ServiceTypeRegistry.GetService(typeof(double))).IsEqualTo(3.14);
    }

    [Test]
    public async Task Registry_ThreadSafety_ConcurrentAdds()
    {
        // Arrange
        var tasks = new List<Task>();
        var itemCount = 100;

        // Act - add items concurrently
        for (int i = 0; i < itemCount; i++)
        {
            var value = i;
            tasks.Add(Task.Run(() => ServiceTypeRegistry.Register(typeof(int), () => value)));
        }

        await Task.WhenAll(tasks);

        // Assert
        var result = ServiceTypeRegistry.GetServices(typeof(int));
        await Assert.That(result.Length).IsEqualTo(itemCount);
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(int))).IsTrue();
    }

    [Test]
    public async Task Register_WithComplexTypes_WorksCorrectly()
    {
        // Arrange
        var testObject = new TestClass { Id = 1, Name = "Test" };
        ServiceTypeRegistry.Register(typeof(TestClass), () => testObject);

        // Act
        var result = ServiceTypeRegistry.GetService(typeof(TestClass)) as TestClass;

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Id).IsEqualTo(1);
        await Assert.That(result.Name).IsEqualTo("Test");
    }

    [Test]
    public async Task Register_LargeNumberOfTypes_MaintainsPerformance()
    {
        // Arrange
        var typeCount = 100;

        // Act
        for (int i = 0; i < typeCount; i++)
        {
            var value = i;
            ServiceTypeRegistry.Register(typeof(int), () => value, $"contract{i}");
        }

        // Assert
        for (int i = 0; i < typeCount; i++)
        {
            var contract = $"contract{i}";
            var result = ServiceTypeRegistry.GetService(typeof(int), contract);
            await Assert.That(result).IsEqualTo(i);
        }
    }

    [Test]
    public async Task GetAllFactoriesForDisposal_WhenEmpty_ReturnsEmptyEnumerable()
    {
        // Act
        var result = ServiceTypeRegistry.GetAllFactoriesForDisposal();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Count()).IsEqualTo(0);
    }

    [Test]
    public async Task GetAllFactoriesForDisposal_WithMultipleRegistrations_ReturnsAllFactories()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(string), () => "second");
        ServiceTypeRegistry.Register(typeof(int), () => 42);
        ServiceTypeRegistry.Register(typeof(double), () => 3.14, "contract");

        // Act
        var result = ServiceTypeRegistry.GetAllFactoriesForDisposal().ToArray();

        // Assert
        await Assert.That(result.Length).IsEqualTo(4);

        // Verify all factories can be invoked
        var values = result.Select(f => f()).ToArray();
        await Assert.That(values).Contains("first");
        await Assert.That(values).Contains("second");
        await Assert.That(values).Contains(42);
        await Assert.That(values).Contains(3.14);
    }

    [Test]
    public async Task GetAllFactoriesForDisposal_ReturnsSnapshot_NotAffectedBySubsequentChanges()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), () => "first");
        ServiceTypeRegistry.Register(typeof(int), () => 42);

        // Act
        var snapshot = ServiceTypeRegistry.GetAllFactoriesForDisposal().ToArray();

        // Make changes after taking snapshot
        ServiceTypeRegistry.Register(typeof(double), () => 3.14);
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert - snapshot should still have original 2 factories
        await Assert.That(snapshot.Length).IsEqualTo(2);
    }

    private sealed class TestClass
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
