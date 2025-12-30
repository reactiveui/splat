// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the RectangleMathExtensions class.
/// </summary>
[TestFixture]
public class RectangleMathExtensionsTests
{
    /// <summary>
    /// Test that Center method calculates correctly.
    /// </summary>
    [Test]
    public void Center_CalculatesCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var center = rect.Center();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(center.X, Is.EqualTo(25.0f)); // 10 + 30/2 = 25
            Assert.That(center.Y, Is.EqualTo(40.0f)); // 20 + 40/2 = 40
        }
    }

    /// <summary>
    /// Test Divide method from left edge.
    /// </summary>
    [Test]
    public void Divide_FromLeftEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Left);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(slice.Width, Is.EqualTo(30.0f));
            Assert.That(slice.X, Is.Zero);
            Assert.That(remainder.Width, Is.EqualTo(70.0f));
            Assert.That(remainder.X, Is.EqualTo(30.0f));
        }
    }

    /// <summary>
    /// Test Divide method from right edge.
    /// </summary>
    [Test]
    public void Divide_FromRightEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Right);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(slice.Width, Is.EqualTo(30.0f));
            Assert.That(slice.X, Is.EqualTo(70.0f));
            Assert.That(remainder.Width, Is.EqualTo(70.0f));
            Assert.That(remainder.X, Is.Zero);
        }
    }

    /// <summary>
    /// Test Divide method from top edge.
    /// </summary>
    [Test]
    public void Divide_FromTopEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Top);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(slice.Height, Is.EqualTo(20.0f));
            Assert.That(slice.Y, Is.Zero);
            Assert.That(remainder.Height, Is.EqualTo(30.0f));
            Assert.That(remainder.Y, Is.EqualTo(20.0f));
        }
    }

    /// <summary>
    /// Test Divide method from bottom edge.
    /// </summary>
    [Test]
    public void Divide_FromBottomEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Bottom);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(slice.Height, Is.EqualTo(20.0f));
            Assert.That(slice.Y, Is.EqualTo(30.0f));
            Assert.That(remainder.Height, Is.EqualTo(30.0f));
            Assert.That(remainder.Y, Is.Zero);
        }
    }

    /// <summary>
    /// Test DivideWithPadding method.
    /// </summary>
    [Test]
    public void DivideWithPadding_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.DivideWithPadding(30.0f, 10.0f, RectEdge.Left);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(slice.Width, Is.EqualTo(30.0f));
            Assert.That(slice.X, Is.Zero);
            Assert.That(remainder.Width, Is.EqualTo(90.0f)); // 100 - 10 = 90 (after padding)
            Assert.That(remainder.X, Is.EqualTo(10.0f)); // starts after padding
        }
    }

    /// <summary>
    /// Test InvertWithin method.
    /// </summary>
    [Test]
    public void InvertWithin_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);
        var container = new RectangleF(0.0f, 0.0f, 200.0f, 100.0f);

        // Act
        var inverted = rect.InvertWithin(container);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(inverted.X, Is.EqualTo(10.0f)); // X unchanged
            Assert.That(inverted.Y, Is.EqualTo(40.0f)); // 100 - (20 + 40) = 40
            Assert.That(inverted.Width, Is.EqualTo(30.0f)); // Width unchanged
            Assert.That(inverted.Height, Is.EqualTo(40.0f)); // Height unchanged
        }
    }

    /// <summary>
    /// Test Copy method with all parameters.
    /// </summary>
    [Test]
    public void Copy_WithAllParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, y: 25.0f, width: 35.0f, height: 45.0f);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(copy.X, Is.EqualTo(15.0f));
            Assert.That(copy.Y, Is.EqualTo(25.0f));
            Assert.That(copy.Width, Is.EqualTo(35.0f));
            Assert.That(copy.Height, Is.EqualTo(45.0f));
        }
    }

    /// <summary>
    /// Test Copy method with partial parameters.
    /// </summary>
    [Test]
    public void Copy_WithPartialParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, height: 50.0f);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(copy.X, Is.EqualTo(15.0f));
            Assert.That(copy.Y, Is.EqualTo(20.0f)); // unchanged
            Assert.That(copy.Width, Is.EqualTo(30.0f)); // unchanged
            Assert.That(copy.Height, Is.EqualTo(50.0f));
        }
    }

    /// <summary>
    /// Test Copy method with top parameter.
    /// </summary>
    [Test]
    public void Copy_WithTopParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(top: 5.0f);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(copy.X, Is.EqualTo(10.0f)); // unchanged
            Assert.That(copy.Y, Is.EqualTo(5.0f)); // set to top value
            Assert.That(copy.Width, Is.EqualTo(30.0f)); // unchanged
            Assert.That(copy.Height, Is.EqualTo(40.0f)); // unchanged
        }
    }

    /// <summary>
    /// Test Copy method with bottom parameter.
    /// </summary>
    [Test]
    public void Copy_WithBottomParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(bottom: 80.0f);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(copy.X, Is.EqualTo(10.0f)); // unchanged
            Assert.That(copy.Y, Is.EqualTo(20.0f)); // unchanged
            Assert.That(copy.Width, Is.EqualTo(30.0f)); // unchanged
            Assert.That(copy.Height, Is.EqualTo(100.0f)); // 20 + 80 = 100
        }
    }

    /// <summary>
    /// Test Copy method throws when Y and Top are both specified.
    /// </summary>
    [Test]
    public void Copy_WithYAndTop_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => rect.Copy(y: 25.0f, top: 5.0f));
    }

    /// <summary>
    /// Test Copy method throws when Height and Bottom are both specified.
    /// </summary>
    [Test]
    public void Copy_WithHeightAndBottom_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => rect.Copy(height: 50.0f, bottom: 80.0f));
    }

    /// <summary>
    /// Test Divide with invalid edge throws ArgumentException.
    /// </summary>
    [Test]
    public void Divide_WithInvalidEdge_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => rect.Divide(30.0f, (RectEdge)999));
    }
}
