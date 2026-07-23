// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for <see cref="ServiceTypeRegistryCache"/> per-resolver non-generic registries.</summary>
public class ServiceTypeRegistryCacheTests
{
    /// <summary>A sample value produced by the first registered factory.</summary>
    private const string First = "first";

    /// <summary>A sample value produced by the second registered factory.</summary>
    private const string Second = "second";

    /// <summary>The number of factories expected from a two-factory registration.</summary>
    private const int TwoItems = 2;

    /// <summary>Verifies that a freshly created registry reports no factories for disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAllFactoriesForDisposal_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        var registry = CreateRegistry();

        // Act
        var result = registry.GetAllFactoriesForDisposal();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Verifies that every registered factory is returned for disposal enumeration without being invoked.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetAllFactoriesForDisposal_WithRegistrations_ReturnsAllFactories()
    {
        // Arrange
        var registry = CreateRegistry();
        registry.Register(typeof(string), static () => First);
        registry.Register(typeof(int), static () => Second);

        // Act
        var result = registry.GetAllFactoriesForDisposal();

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        var values = result.Select(static f => f()).ToArray();
        await Assert.That(values).Contains(First);
        await Assert.That(values).Contains(Second);
    }

    /// <summary>Verifies that resolving all services where every factory yields null returns an empty array.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithAllNullFactories_ReturnsEmptyArray()
    {
        // Arrange
        var registry = CreateRegistry();
        registry.Register(typeof(string), static () => null);
        registry.Register(typeof(string), static () => null);

        // Act
        var result = registry.GetServices(typeof(string));

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(0);
    }

    /// <summary>Verifies that resolving all services with a mix of null and non-null factories keeps only the non-null values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithMixedNullFactories_ReturnsOnlyNonNullValues()
    {
        // Arrange - the middle factory yields null so the result buffer must shrink to the surviving values.
        var registry = CreateRegistry();
        registry.Register(typeof(string), static () => First);
        registry.Register(typeof(string), static () => null);
        registry.Register(typeof(string), static () => Second);

        // Act
        var result = registry.GetServices(typeof(string));

        // Assert
        await Assert.That(result.Length).IsEqualTo(TwoItems);
        await Assert.That(result[0]).IsEqualTo(First);
        await Assert.That(result[1]).IsEqualTo(Second);
    }

    /// <summary>Creates a fresh per-resolver registry backed by an isolated <see cref="ResolverState"/>.</summary>
    /// <returns>A new registry instance with no registrations.</returns>
    private static ServiceTypeRegistryCache.Registry CreateRegistry()
    {
        var state = new ResolverState();
        return ServiceTypeRegistryCache.Get(state);
    }
}
