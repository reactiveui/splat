// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

public class RectangleMathExtensionsTests
{
    /// <summary>
    /// Test that Center method calculates correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Center_CalculatesCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var center = rect.Center();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(center.X).IsEqualTo(25.0f); // 10 + 30/2 = 25
            await Assert.That(center.Y).IsEqualTo(40.0f); // 20 + 40/2 = 40
        }
    }

    /// <summary>
    /// Test Divide method from left edge.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromLeftEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Left);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(30.0f);
            await Assert.That(slice.X).IsEqualTo(0);
            await Assert.That(remainder.Width).IsEqualTo(70.0f);
            await Assert.That(remainder.X).IsEqualTo(30.0f);
        }
    }

    /// <summary>
    /// Test Divide method from right edge.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromRightEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Right);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(30.0f);
            await Assert.That(slice.X).IsEqualTo(70.0f);
            await Assert.That(remainder.Width).IsEqualTo(70.0f);
            await Assert.That(remainder.X).IsEqualTo(0);
        }
    }

    /// <summary>
    /// Test Divide method from top edge.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromTopEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Top);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Height).IsEqualTo(20.0f);
            await Assert.That(slice.Y).IsEqualTo(0);
            await Assert.That(remainder.Height).IsEqualTo(30.0f);
            await Assert.That(remainder.Y).IsEqualTo(20.0f);
        }
    }

    /// <summary>
    /// Test Divide method from bottom edge.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromBottomEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Bottom);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Height).IsEqualTo(20.0f);
            await Assert.That(slice.Y).IsEqualTo(30.0f);
            await Assert.That(remainder.Height).IsEqualTo(30.0f);
            await Assert.That(remainder.Y).IsEqualTo(0);
        }
    }

    /// <summary>
    /// Test DivideWithPadding method.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DivideWithPadding_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.DivideWithPadding(30.0f, 10.0f, RectEdge.Left);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(30.0f);
            await Assert.That(slice.X).IsEqualTo(0);
            await Assert.That(remainder.Width).IsEqualTo(90.0f); // 100 - 10 = 90 (after padding)
            await Assert.That(remainder.X).IsEqualTo(10.0f); // starts after padding
        }
    }

    /// <summary>
    /// Test InvertWithin method.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvertWithin_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);
        var container = new RectangleF(0.0f, 0.0f, 200.0f, 100.0f);

        // Act
        var inverted = rect.InvertWithin(container);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(inverted.X).IsEqualTo(10.0f); // X unchanged
            await Assert.That(inverted.Y).IsEqualTo(40.0f); // 100 - (20 + 40) = 40
            await Assert.That(inverted.Width).IsEqualTo(30.0f); // Width unchanged
            await Assert.That(inverted.Height).IsEqualTo(40.0f); // Height unchanged
        }
    }

    /// <summary>
    /// Test Copy method with all parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithAllParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, y: 25.0f, width: 35.0f, height: 45.0f);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(15.0f);
            await Assert.That(copy.Y).IsEqualTo(25.0f);
            await Assert.That(copy.Width).IsEqualTo(35.0f);
            await Assert.That(copy.Height).IsEqualTo(45.0f);
        }
    }

    /// <summary>
    /// Test Copy method with partial parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithPartialParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, height: 50.0f);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(15.0f);
            await Assert.That(copy.Y).IsEqualTo(20.0f); // unchanged
            await Assert.That(copy.Width).IsEqualTo(30.0f); // unchanged
            await Assert.That(copy.Height).IsEqualTo(50.0f);
        }
    }

    /// <summary>
    /// Test Copy method with top parameter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithTopParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(top: 5.0f);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(10.0f); // unchanged
            await Assert.That(copy.Y).IsEqualTo(5.0f); // set to top value
            await Assert.That(copy.Width).IsEqualTo(30.0f); // unchanged
            await Assert.That(copy.Height).IsEqualTo(40.0f); // unchanged
        }
    }

    /// <summary>
    /// Test Copy method with bottom parameter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithBottomParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(bottom: 80.0f);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(10.0f); // unchanged
            await Assert.That(copy.Y).IsEqualTo(20.0f); // unchanged
            await Assert.That(copy.Width).IsEqualTo(30.0f); // unchanged
            await Assert.That(copy.Height).IsEqualTo(100.0f); // 20 + 80 = 100
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
