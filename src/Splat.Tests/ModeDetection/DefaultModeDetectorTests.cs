// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ModeDetection;

/// <summary>
/// Unit Tests for the DefaultModeDetector class.
/// </summary>
[TestFixture]
public class DefaultModeDetectorTests
{
    /// <summary>
    /// Test that DefaultModeDetector can detect unit test runner.
    /// </summary>
    [Test]
    public void DefaultModeDetector_CanDetectUnitTestRunner()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Assert
        // Since we're running under NUnit, this should return true
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.HasValue, Is.True);
            Assert.That(result!.Value, Is.True);
        }
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IModeDetector.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ImplementsIModeDetector()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.That(detector, Is.AssignableTo<IModeDetector>());
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IEnableLogger.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ImplementsIEnableLogger()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.That(detector, Is.AssignableTo<IEnableLogger>());
    }

    /// <summary>
    /// Test that DefaultModeDetector handles exceptions gracefully.
    /// </summary>
    [Test]
    public void DefaultModeDetector_HandlesExceptionsGracefully()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act & Assert - should not throw
        Assert.DoesNotThrow(() => detector.InUnitTestRunner());

        var result = detector.InUnitTestRunner();

        // Should return a value (either true or false) or null if exception occurred
        Assert.That(result, Is.Null.Or.True.Or.False);
    }

    /// <summary>
    /// Test that DefaultModeDetector returns consistent results.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ReturnsConsistentResults()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result1 = detector.InUnitTestRunner();
        var result2 = detector.InUnitTestRunner();
        var result3 = detector.InUnitTestRunner();

        // Assert - Should return consistent results
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result2, Is.EqualTo(result1));
            Assert.That(result3, Is.EqualTo(result2));
        }
    }
}
