// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>Tests for the ServiceTypeRegistry class.</summary>
[NotInParallel] // ServiceTypeRegistry is static, tests must run sequentially
public class ServiceTypeRegistryTests
{
    /// <summary>Contract name used when registering against a custom contract.</summary>
    private const string Contract = "contract";

    /// <summary>Contract name used for the first registration in these tests.</summary>
    private const string First = "first";

    /// <summary>Contract name used for the second registration in these tests.</summary>
    private const string Second = "second";

    /// <summary>Contract name used for the third registration in these tests.</summary>
    private const string Third = "third";

    /// <summary>A sample integer value registered during these tests.</summary>
    private const int SampleIntValue = 42;

    /// <summary>A sample double value registered during these tests.</summary>
    private const double SampleDoubleValue = 3.14;

    /// <summary>The expected number of callback invocations in these tests.</summary>
    private const int ExpectedInvocationCount = 2;

    /// <summary>The expected registered-type count when two types are present.</summary>
    private const int TwoItems = 2;

    /// <summary>The expected registered-type count when three types are present.</summary>
    private const int ThreeItems = 3;

    /// <summary>The expected registered-type count when four types are present.</summary>
    private const int FourItems = 4;

    /// <summary>Clears the registry state before each test.</summary>
    [Before(Test)]
    public void Setup() =>

        // Clear the registry before each test
        ServiceTypeRegistry.Clear();

    /// <summary>Clears the registry state after each test.</summary>
    [After(Test)]
    public void Cleanup() =>

        // Clear the registry after each test
        ServiceTypeRegistry.Clear();

    /// <summary>Tests that track non generic registration tracks type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TrackNonGenericRegistration_TracksType()
    {
        // Act
        ServiceTypeRegistry.TrackNonGenericRegistration(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsTrue();
    }

    /// <summary>Tests that track non generic registration with contract tracks type and contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TrackNonGenericRegistration_WithContract_TracksTypeAndContract()
    {
        // Act
        ServiceTypeRegistry.TrackNonGenericRegistration(typeof(string), Contract);

        // Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string), Contract)).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsFalse();
    }

    /// <summary>Tests that has non generic registrations when not tracked returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasNonGenericRegistrations_WhenNotTracked_ReturnsFalse() =>

        // Act & Assert
        await Assert.That(ServiceTypeRegistry.HasNonGenericRegistrations(typeof(string))).IsFalse();

    /// <summary>Tests that register stores factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_StoresFactory()
    {
        // Arrange
        const string expectedValue = "test";
        Func<object?> factory = static () => expectedValue;

        // Act
        ServiceTypeRegistry.Register(typeof(string), factory);
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    /// <summary>Tests that register with contract stores factory under contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithContract_StoresFactoryUnderContract()
    {
        // Arrange
        const string expectedValue = "contract value";
        Func<object?> factory = static () => expectedValue;

        // Act
        ServiceTypeRegistry.Register(typeof(string), factory, Contract);
        var result = ServiceTypeRegistry.GetService(typeof(string), Contract);

        // Assert
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    /// <summary>Tests that register multiple for same type returns latest.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_MultipleForSameType_ReturnsLatest()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(string), static () => Second);
        ServiceTypeRegistry.Register(typeof(string), static () => Third);

        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo(Third);
    }

    /// <summary>Tests that get service when empty returns null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WhenEmpty_ReturnsNull()
    {
        // Act
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>Tests that get service with wrong contract returns null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithWrongContract_ReturnsNull()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "test", "contract1");

        // Act
        var result = ServiceTypeRegistry.GetService(typeof(string), "contract2");

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>Tests that get service invokes factory each time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
        await Assert.That(invocationCount).IsEqualTo(ExpectedInvocationCount);
        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(ExpectedInvocationCount);
    }

    /// <summary>Tests that get services when empty returns empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WhenEmpty_ReturnsEmptyArray()
    {
        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that get services with multiple registrations returns all values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithMultipleRegistrations_ReturnsAllValues()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(string), static () => Second);
        ServiceTypeRegistry.Register(typeof(string), static () => Third);

        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result.Length).IsEqualTo(ThreeItems);
        await Assert.That(result[0]).IsEqualTo(First);
        await Assert.That(result[1]).IsEqualTo(Second);
        await Assert.That(result[^1]).IsEqualTo(Third);
    }

    /// <summary>Tests that get services only returns matching contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_OnlyReturnsMatchingContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "no-contract-1");
        ServiceTypeRegistry.Register(typeof(string), static () => "no-contract-2");
        ServiceTypeRegistry.Register(typeof(string), static () => "contract-1", Contract);

        // Act
        var resultNoContract = ServiceTypeRegistry.GetServices(typeof(string));
        var resultWithContract = ServiceTypeRegistry.GetServices(typeof(string), Contract);

        // Assert
        await Assert.That(resultNoContract.Length).IsEqualTo(TwoItems);
        await Assert.That(resultWithContract.Length).IsEqualTo(1);
        await Assert.That(resultWithContract[0]).IsEqualTo("contract-1");
    }

    /// <summary>Tests that get services filters out null values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_FiltersOutNullValues()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "valid");
        ServiceTypeRegistry.Register(typeof(string), static () => null);
        ServiceTypeRegistry.Register(typeof(string), static () => "another valid");

        // Act
        var result = ServiceTypeRegistry.GetServices(typeof(string));

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        await Assert.That(result[0]).IsEqualTo("valid");
        await Assert.That(result[1]).IsEqualTo("another valid");
    }

    /// <summary>Tests that has registration when empty returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WhenEmpty_ReturnsFalse() =>

        // Act & Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();

    /// <summary>Tests that has registration after registering returns true.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_AfterRegistering_ReturnsTrue()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), static () => "test");

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsTrue();
    }

    /// <summary>Tests that has registration with contract only returns true for matching contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WithContract_OnlyReturnsTrueForMatchingContract()
    {
        // Act
        ServiceTypeRegistry.Register(typeof(string), static () => "test", Contract);

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), Contract)).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
    }

    /// <summary>Tests that unregister current when empty does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_WhenEmpty_DoesNotThrow() =>

        // Act & Assert - should not throw
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

    /// <summary>Tests that unregister current with single item clears registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_WithSingleItem_ClearsRegistration()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "test");

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.GetService(typeof(string))).IsNull();
    }

    /// <summary>Tests that unregister current with multiple items removes latest.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_WithMultipleItems_RemovesLatest()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(string), static () => Second);
        ServiceTypeRegistry.Register(typeof(string), static () => Third);

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));
        var result = ServiceTypeRegistry.GetService(typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo(Second);
    }

    /// <summary>Tests that unregister current only affects specified contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_OnlyAffectsSpecifiedContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "no contract");
        ServiceTypeRegistry.Register(typeof(string), static () => "with contract", Contract);

        // Act
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), Contract)).IsTrue();
    }

    /// <summary>Tests that unregister all removes all registrations for type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_RemovesAllRegistrationsForType()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(string), static () => Second);
        ServiceTypeRegistry.Register(typeof(string), static () => Third);

        // Act
        ServiceTypeRegistry.UnregisterAll(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        var result = ServiceTypeRegistry.GetServices(typeof(string));
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Tests that unregister all only affects specified type and contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_OnlyAffectsSpecifiedTypeAndContract()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "string1");
        ServiceTypeRegistry.Register(typeof(int), static () => SampleIntValue);
        ServiceTypeRegistry.Register(typeof(string), static () => "string-contract", Contract);

        // Act
        ServiceTypeRegistry.UnregisterAll(typeof(string));

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(int))).IsTrue();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), Contract)).IsTrue();
    }

    /// <summary>Tests that clear removes all registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_RemovesAllRegistrations()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => "string");
        ServiceTypeRegistry.Register(typeof(int), static () => SampleIntValue);
        ServiceTypeRegistry.Register(typeof(string), static () => Contract, Contract);

        // Act
        ServiceTypeRegistry.Clear();

        // Assert
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(int))).IsFalse();
        await Assert.That(ServiceTypeRegistry.HasRegistration(typeof(string), Contract)).IsFalse();
    }

    /// <summary>Tests that clear when empty does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_WhenEmpty_DoesNotThrow() =>

        // Act & Assert - should not throw
        ServiceTypeRegistry.Clear();

    /// <summary>Tests that registry with different types are isolated.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Registry_WithDifferentTypes_AreIsolated()
    {
        // Arrange & Act
        ServiceTypeRegistry.Register(typeof(string), static () => "string value");
        ServiceTypeRegistry.Register(typeof(int), static () => SampleIntValue);
        ServiceTypeRegistry.Register(typeof(double), static () => SampleDoubleValue);

        // Assert
        await Assert.That(ServiceTypeRegistry.GetService(typeof(string))).IsEqualTo("string value");
        await Assert.That(ServiceTypeRegistry.GetService(typeof(int))).IsEqualTo(SampleIntValue);
        await Assert.That(ServiceTypeRegistry.GetService(typeof(double))).IsEqualTo(SampleDoubleValue);
    }

    /// <summary>Tests that registry thread safety concurrent adds.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Registry_ThreadSafety_ConcurrentAdds()
    {
        // Arrange
        var tasks = new List<Task>();
        const int itemCount = 100;

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

    /// <summary>Tests that register with complex types works correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Tests that register large number of types maintains performance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_LargeNumberOfTypes_MaintainsPerformance()
    {
        // Arrange
        const int typeCount = 100;

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

    /// <summary>Tests that get all factories for disposal when empty returns empty enumerable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAllFactoriesForDisposal_WhenEmpty_ReturnsEmptyEnumerable()
    {
        // Act
        var result = ServiceTypeRegistry.GetAllFactoriesForDisposal();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result).Count().IsEqualTo(0);
    }

    /// <summary>Tests that get all factories for disposal with multiple registrations returns all factories.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAllFactoriesForDisposal_WithMultipleRegistrations_ReturnsAllFactories()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(string), static () => Second);
        ServiceTypeRegistry.Register(typeof(int), static () => SampleIntValue);
        ServiceTypeRegistry.Register(typeof(double), static () => SampleDoubleValue, Contract);

        // Act
        var result = ServiceTypeRegistry.GetAllFactoriesForDisposal().ToArray();

        // Assert
        await Assert.That(result.Length).IsEqualTo(FourItems);

        // Verify all factories can be invoked
        var values = result.Select(static f => f()).ToArray();
        await Assert.That(values).Contains(First);
        await Assert.That(values).Contains(Second);
        await Assert.That(values).Contains(SampleIntValue);
        await Assert.That(values).Contains(SampleDoubleValue);
    }

    /// <summary>Tests that get all factories for disposal returns snapshot not affected by subsequent changes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAllFactoriesForDisposal_ReturnsSnapshot_NotAffectedBySubsequentChanges()
    {
        // Arrange
        ServiceTypeRegistry.Register(typeof(string), static () => First);
        ServiceTypeRegistry.Register(typeof(int), static () => SampleIntValue);

        // Act
        var snapshot = ServiceTypeRegistry.GetAllFactoriesForDisposal().ToArray();

        // Make changes after taking snapshot
        ServiceTypeRegistry.Register(typeof(double), static () => SampleDoubleValue);
        ServiceTypeRegistry.UnregisterCurrent(typeof(string));

        // Assert - snapshot should still have original 2 factories
        await Assert.That(snapshot.Length).IsEqualTo(TwoItems);
    }

    /// <summary>A simple reference type used for testing the service type registry.</summary>
    private sealed class TestClass
    {
        /// <summary>Gets or sets the identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string? Name { get; set; }
    }
}
