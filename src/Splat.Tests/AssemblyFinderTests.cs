// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the AssemblyFinder class.
/// </summary>
public class AssemblyFinderTests
{
    /// <summary>
    /// Test that AttemptToLoadType returns default for non-existent type.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_ReturnsDefault_ForNonExistentType()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>("NonExistent.Type");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Test that AttemptToLoadType returns default for invalid type name.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_ReturnsDefault_ForInvalidTypeName()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<object>("Invalid..Type..Name");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Test that AttemptToLoadType works with well-known types.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_Works_WithWellKnownTypes()
    {
        // Act - Try to load a well-known system type
        var result = AssemblyFinder.AttemptToLoadType<object>("System.String");

        // Assert - Should return default for reference types when type can't be instantiated with parameterless constructor
        Assert.Null(result);
    }

    /// <summary>
    /// Test that AttemptToLoadType returns default for value types when type doesn't exist.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_ReturnsDefault_ForValueTypes()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<int>("NonExistent.ValueType");

        // Assert
        Assert.Equal(0, result); // Default for int
    }

    /// <summary>
    /// Test that AttemptToLoadType handles empty string gracefully.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_HandlesEmptyString()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>(string.Empty);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Test that AttemptToLoadType handles null string gracefully.
    /// </summary>
    [Fact]
    public void AttemptToLoadType_HandlesNull()
    {
        // Act
        var result = AssemblyFinder.AttemptToLoadType<string>(null!);

        // Assert
        Assert.Null(result);
    }
}
