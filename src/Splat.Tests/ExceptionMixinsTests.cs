// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the ExceptionMixins class.
/// </summary>
public class ExceptionMixinsTests
{
    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull throws when value is null.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNull_ThrowsWhenValueIsNull()
    {
        // Arrange
        string? value = null;
        const string paramName = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.ThrowArgumentNullExceptionIfNull(paramName));
        Assert.Equal(paramName, exception.ParamName);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull does not throw when value is not null.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNull_DoesNotThrowWhenValueIsNotNull()
    {
        // Arrange
        const string value = "test";
        const string paramName = "testParam";

        // Act & Assert - should not throw
        value.ThrowArgumentNullExceptionIfNull(paramName);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull with message throws when value is null.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNullWithMessage_ThrowsWhenValueIsNull()
    {
        // Arrange
        string? value = null;
        const string paramName = "testParam";
        const string message = "Test error message";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.ThrowArgumentNullExceptionIfNull(paramName, message));
        Assert.Equal(paramName, exception.ParamName);
        Assert.Contains(message, exception.Message, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull with message does not throw when value is not null.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNullWithMessage_DoesNotThrowWhenValueIsNotNull()
    {
        // Arrange
        const string value = "test";
        const string paramName = "testParam";
        const string message = "Test error message";

        // Act & Assert - should not throw
        value.ThrowArgumentNullExceptionIfNull(paramName, message);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull works with reference types.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNull_WorksWithReferenceTypes()
    {
        // Arrange
        object? value = null;
        const string paramName = "objectParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.ThrowArgumentNullExceptionIfNull(paramName));
        Assert.Equal(paramName, exception.ParamName);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull works with nullable value types.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNull_WorksWithNullableValueTypes()
    {
        // Arrange
        int? value = null;
        const string paramName = "intParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.ThrowArgumentNullExceptionIfNull(paramName));
        Assert.Equal(paramName, exception.ParamName);
    }

    /// <summary>
    /// Test that ThrowArgumentNullExceptionIfNull does not throw with non-null nullable value types.
    /// </summary>
    [Fact]
    public void ThrowArgumentNullExceptionIfNull_DoesNotThrowWithNonNullNullableValueTypes()
    {
        // Arrange
        int? value = 42;
        const string paramName = "intParam";

        // Act & Assert - should not throw
        value.ThrowArgumentNullExceptionIfNull(paramName);
    }
}
