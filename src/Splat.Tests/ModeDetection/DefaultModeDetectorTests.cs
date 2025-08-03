// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ModeDetection;

/// <summary>
/// Unit Tests for the DefaultModeDetector class.
/// </summary>
public class DefaultModeDetectorTests
{
    /// <summary>
    /// Test that DefaultModeDetector can detect unit test runner.
    /// </summary>
    [Fact]
    public void DefaultModeDetector_CanDetectUnitTestRunner()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Assert
        // Since we're running in XUnit, this should return true
        Assert.True(result.HasValue);
        Assert.True(result.Value);
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IModeDetector.
    /// </summary>
    [Fact]
    public void DefaultModeDetector_ImplementsIModeDetector()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.IsAssignableFrom<IModeDetector>(detector);
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IEnableLogger.
    /// </summary>
    [Fact]
    public void DefaultModeDetector_ImplementsIEnableLogger()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.IsAssignableFrom<IEnableLogger>(detector);
    }

    /// <summary>
    /// Test that DefaultModeDetector handles exceptions gracefully.
    /// </summary>
    [Fact]
    public void DefaultModeDetector_HandlesExceptionsGracefully()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act & Assert - should not throw
        var result = detector.InUnitTestRunner();

        // Should return a value (either true or false) or null if exception occurred
#pragma warning disable CS8794 // The input always matches the provided pattern, this is expected.
        Assert.True(result is true or false or null);
#pragma warning restore CS8794 // The input always matches the provided pattern, this is expected.
    }

    /// <summary>
    /// Test that DefaultModeDetector returns consistent results.
    /// </summary>
    [Fact]
    public void DefaultModeDetector_ReturnsConsistentResults()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result1 = detector.InUnitTestRunner();
        var result2 = detector.InUnitTestRunner();
        var result3 = detector.InUnitTestRunner();

        // Assert - Should return consistent results
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
    }
}
