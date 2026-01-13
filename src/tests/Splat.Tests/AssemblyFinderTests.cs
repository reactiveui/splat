// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

public class AssemblyFinderTests
{
    /// <summary>
    /// Test that AttemptToLoadType returns default for non-existent type.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_ReturnsDefault_ForNonExistentType()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>("NonExistent.Type");

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test that AttemptToLoadType returns default for invalid type name.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_ReturnsDefault_ForInvalidTypeName()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<object>("Invalid..Type..Name");

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test that AttemptToLoadType works with well-known types.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_Works_WithWellKnownTypes()
    {
        // Act - Try to load a well-known system type
        var result = AssemblyFinder.AttemptToLoadType<object>("System.String");

        // Assert - Should return default for reference types when type can't be instantiated with parameterless constructor
        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test that AttemptToLoadType returns default for value types when type doesn't exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_ReturnsDefault_ForValueTypes()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<int>("NonExistent.ValueType");

        // Assert
        await Assert.That(result).IsEqualTo(0); // Default for int
    }

    /// <summary>
    /// Test that AttemptToLoadType handles empty string gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_HandlesEmptyString()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>(string.Empty);

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test that AttemptToLoadType handles null string gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AttemptToLoadType_HandlesNull()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>(null!);

        // Assert
        await Assert.That(result).IsNull();
    }
}
