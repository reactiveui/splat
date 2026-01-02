// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation.GenericFirst;

/// <summary>
/// Tests for the ContractContainer&lt;T&gt; class.
/// </summary>
[NotInParallel] // ContractContainer<T> is static, tests must run sequentially
public class ContractContainerTests
{
    private const string Contract1 = "Contract1";
    private const string Contract2 = "Contract2";

    [Before(HookType.Test)]
    public void Setup()
    {
        // Clear all contracts before each test
        ContractContainer<string>.ClearAll();
        ContractContainer<int>.ClearAll();
        ContractContainer<int?>.ClearAll();
    }

    [After(HookType.Test)]
    public void Cleanup()
    {
        // Clear all contracts after each test
        ContractContainer<string>.ClearAll();
        ContractContainer<int>.ClearAll();
        ContractContainer<int?>.ClearAll();
    }

    [Test]
    public async Task HasRegistrations_WhenEmpty_ReturnsFalse() =>

        // Act & Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();

    [Test]
    public async Task HasRegistrations_AfterAdding_ReturnsTrue()
    {
        // Act
        ContractContainer<string>.Add("test", Contract1);

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsTrue();
    }

    [Test]
    public async Task Add_WithInstance_StoresInstance()
    {
        // Arrange
        var instance = "test value";

        // Act
        ContractContainer<string>.Add(instance, Contract1);
        var success = ContractContainer<string>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(instance);
    }

    [Test]
    public async Task Add_WithFactory_StoresFactory()
    {
        // Arrange
        var expectedValue = 42;
        Func<int?> factory = () => expectedValue;

        // Act
        ContractContainer<int?>.Add(factory, Contract1);
        var success = ContractContainer<int?>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task Add_WithNullContract_UsesEmptyString()
    {
        // Act
        ContractContainer<string>.Add("test", null);
        var success = ContractContainer<string>.TryGet(string.Empty, out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("test");
    }

    [Test]
    public async Task Add_MultipleToSameContract_ReturnsLatest()
    {
        // Act
        ContractContainer<string>.Add("first", Contract1);
        ContractContainer<string>.Add("second", Contract1);
        ContractContainer<string>.Add("third", Contract1);

        var success = ContractContainer<string>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("third");
    }

    [Test]
    public async Task Add_ToDifferentContracts_AreIsolated()
    {
        // Act
        ContractContainer<string>.Add("contract1 value", Contract1);
        ContractContainer<string>.Add("contract2 value", Contract2);

        var success1 = ContractContainer<string>.TryGet(Contract1, out var result1);
        var success2 = ContractContainer<string>.TryGet(Contract2, out var result2);

        // Assert
        await Assert.That(success1).IsTrue();
        await Assert.That(result1).IsEqualTo("contract1 value");
        await Assert.That(success2).IsTrue();
        await Assert.That(result2).IsEqualTo("contract2 value");
    }

    [Test]
    public async Task TryGet_WhenEmpty_ReturnsFalse()
    {
        // Act
        var success = ContractContainer<string>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryGet_WithWrongContract_ReturnsFalse()
    {
        // Arrange
        ContractContainer<string>.Add("test", Contract1);

        // Act
        var success = ContractContainer<string>.TryGet(Contract2, out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryGet_WithFactoryReturningNull_ReturnsFalse()
    {
        // Arrange
        ContractContainer<string?>.Add(() => null, Contract1);

        // Act
        var success = ContractContainer<string?>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryGet_InvokesFactoryEachTime()
    {
        // Arrange
        var invocationCount = 0;
        ContractContainer<int>.Add(
            () =>
            {
                invocationCount++;
                return invocationCount;
            },
            Contract1);

        // Act
        ContractContainer<int>.TryGet(Contract1, out var result1);
        ContractContainer<int>.TryGet(Contract1, out var result2);

        // Assert
        await Assert.That(invocationCount).IsEqualTo(2);
        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(2);
    }

    [Test]
    public async Task GetAll_WhenEmpty_ReturnsEmptyArray()
    {
        // Act
        var result = ContractContainer<string>.GetAll(Contract1);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    [Test]
    public async Task GetAll_WithMultipleRegistrations_ReturnsAllValues()
    {
        // Arrange
        ContractContainer<string>.Add("first", Contract1);
        ContractContainer<string>.Add("second", Contract1);
        ContractContainer<string>.Add("third", Contract1);

        // Act
        var result = ContractContainer<string>.GetAll(Contract1);

        // Assert
        await Assert.That(result.Length).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
        await Assert.That(result[2]).IsEqualTo("third");
    }

    [Test]
    public async Task GetAll_OnlyReturnsMatchingContract()
    {
        // Arrange
        ContractContainer<string>.Add("contract1-1", Contract1);
        ContractContainer<string>.Add("contract1-2", Contract1);
        ContractContainer<string>.Add("contract2-1", Contract2);

        // Act
        var result1 = ContractContainer<string>.GetAll(Contract1);
        var result2 = ContractContainer<string>.GetAll(Contract2);

        // Assert
        await Assert.That(result1.Length).IsEqualTo(2);
        await Assert.That(result1[0]).IsEqualTo("contract1-1");
        await Assert.That(result1[1]).IsEqualTo("contract1-2");

        await Assert.That(result2.Length).IsEqualTo(1);
        await Assert.That(result2[0]).IsEqualTo("contract2-1");
    }

    [Test]
    public async Task RemoveCurrent_WhenEmpty_DoesNotThrow()
    {
        // Act & Assert - should not throw
        ContractContainer<string>.RemoveCurrent(Contract1);
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
    }

    [Test]
    public async Task RemoveCurrent_WithSingleItem_ClearsContract()
    {
        // Arrange
        ContractContainer<string>.Add("test", Contract1);

        // Act
        ContractContainer<string>.RemoveCurrent(Contract1);

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
        var success = ContractContainer<string>.TryGet(Contract1, out _);
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task RemoveCurrent_WithMultipleItems_RemovesLatest()
    {
        // Arrange
        ContractContainer<string>.Add("first", Contract1);
        ContractContainer<string>.Add("second", Contract1);
        ContractContainer<string>.Add("third", Contract1);

        // Act
        ContractContainer<string>.RemoveCurrent(Contract1);
        var success = ContractContainer<string>.TryGet(Contract1, out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("second");
    }

    [Test]
    public async Task RemoveCurrent_OnlyAffectsSpecifiedContract()
    {
        // Arrange
        ContractContainer<string>.Add("contract1", Contract1);
        ContractContainer<string>.Add("contract2", Contract2);

        // Act
        ContractContainer<string>.RemoveCurrent(Contract1);

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract2)).IsTrue();
    }

    [Test]
    public async Task Clear_RemovesAllRegistrationsForContract()
    {
        // Arrange
        ContractContainer<string>.Add("first", Contract1);
        ContractContainer<string>.Add("second", Contract1);
        ContractContainer<string>.Add("third", Contract1);

        // Act
        ContractContainer<string>.Clear(Contract1);

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
        var result = ContractContainer<string>.GetAll(Contract1);
        await Assert.That(result.Length).IsEqualTo(0);
    }

    [Test]
    public async Task Clear_OnlyAffectsSpecifiedContract()
    {
        // Arrange
        ContractContainer<string>.Add("contract1", Contract1);
        ContractContainer<string>.Add("contract2", Contract2);

        // Act
        ContractContainer<string>.Clear(Contract1);

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract2)).IsTrue();
    }

    [Test]
    public async Task ClearAll_RemovesAllContracts()
    {
        // Arrange
        ContractContainer<string>.Add("contract1", Contract1);
        ContractContainer<string>.Add("contract2", Contract2);

        // Act
        ContractContainer<string>.ClearAll();

        // Assert
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract1)).IsFalse();
        await Assert.That(ContractContainer<string>.HasRegistrations(Contract2)).IsFalse();
    }

    [Test]
    public async Task ClearAll_WhenEmpty_DoesNotThrow() =>

        // Act & Assert - should not throw
        ContractContainer<string>.ClearAll();

    [Test]
    public async Task ContractContainer_DifferentTypes_AreIsolated()
    {
        // Arrange & Act
        ContractContainer<string>.Add("string value", Contract1);
        ContractContainer<int>.Add(42, Contract1);

        // Assert
        ContractContainer<string>.TryGet(Contract1, out var stringResult);
        ContractContainer<int>.TryGet(Contract1, out var intResult);

        await Assert.That(stringResult).IsEqualTo("string value");
        await Assert.That(intResult).IsEqualTo(42);
    }

    [Test]
    public async Task ContractContainer_ThreadSafety_ConcurrentAdds()
    {
        // Arrange
        var tasks = new List<Task>();
        var itemCount = 100;

        // Act - add items concurrently to different contracts
        for (int i = 0; i < itemCount; i++)
        {
            var value = i;
            var contract = $"Contract{i % 10}";
            tasks.Add(Task.Run(() => ContractContainer<int>.Add(value, contract)));
        }

        await Task.WhenAll(tasks);

        // Assert - verify all contracts have items
        for (int i = 0; i < 10; i++)
        {
            var contract = $"Contract{i}";
            await Assert.That(ContractContainer<int>.HasRegistrations(contract)).IsTrue();
        }
    }

    [Test]
    public async Task Add_WithManyContracts_WorksCorrectly()
    {
        // Arrange
        var contractCount = 50;

        // Act
        for (int i = 0; i < contractCount; i++)
        {
            ContractContainer<int>.Add(i, $"Contract{i}");
        }

        // Assert
        for (int i = 0; i < contractCount; i++)
        {
            var contract = $"Contract{i}";
            var success = ContractContainer<int>.TryGet(contract, out var result);
            await Assert.That(success).IsTrue();
            await Assert.That(result).IsEqualTo(i);
        }
    }
}
