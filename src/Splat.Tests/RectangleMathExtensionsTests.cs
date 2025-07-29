// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the RectangleMathExtensions class.
/// </summary>
public class RectangleMathExtensionsTests
{
    /// <summary>
    /// Test that Center method calculates correctly.
    /// </summary>
    [Fact]
    public void Center_CalculatesCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var center = rect.Center();

        // Assert
        Assert.Equal(25.0f, center.X); // 10 + 30/2 = 25
        Assert.Equal(40.0f, center.Y); // 20 + 40/2 = 40
    }

    /// <summary>
    /// Test Divide method from left edge.
    /// </summary>
    [Fact]
    public void Divide_FromLeftEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Left);

        // Assert
        Assert.Equal(30.0f, slice.Width);
        Assert.Equal(0.0f, slice.X);
        Assert.Equal(70.0f, remainder.Width);
        Assert.Equal(30.0f, remainder.X);
    }

    /// <summary>
    /// Test Divide method from right edge.
    /// </summary>
    [Fact]
    public void Divide_FromRightEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(30.0f, RectEdge.Right);

        // Assert
        Assert.Equal(30.0f, slice.Width);
        Assert.Equal(70.0f, slice.X);
        Assert.Equal(70.0f, remainder.Width);
        Assert.Equal(0.0f, remainder.X);
    }

    /// <summary>
    /// Test Divide method from top edge.
    /// </summary>
    [Fact]
    public void Divide_FromTopEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Top);

        // Assert
        Assert.Equal(20.0f, slice.Height);
        Assert.Equal(0.0f, slice.Y);
        Assert.Equal(30.0f, remainder.Height);
        Assert.Equal(20.0f, remainder.Y);
    }

    /// <summary>
    /// Test Divide method from bottom edge.
    /// </summary>
    [Fact]
    public void Divide_FromBottomEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.Divide(20.0f, RectEdge.Bottom);

        // Assert
        Assert.Equal(20.0f, slice.Height);
        Assert.Equal(30.0f, slice.Y);
        Assert.Equal(30.0f, remainder.Height);
        Assert.Equal(0.0f, remainder.Y);
    }

    /// <summary>
    /// Test DivideWithPadding method.
    /// </summary>
    [Fact]
    public void DivideWithPadding_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act
        var (slice, remainder) = rect.DivideWithPadding(30.0f, 10.0f, RectEdge.Left);

        // Assert
        Assert.Equal(30.0f, slice.Width);
        Assert.Equal(0.0f, slice.X);
        Assert.Equal(90.0f, remainder.Width); // 100 - 10 = 90 (after padding)
        Assert.Equal(10.0f, remainder.X); // starts after padding
    }

    /// <summary>
    /// Test InvertWithin method.
    /// </summary>
    [Fact]
    public void InvertWithin_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);
        var container = new RectangleF(0.0f, 0.0f, 200.0f, 100.0f);

        // Act
        var inverted = rect.InvertWithin(container);

        // Assert
        Assert.Equal(10.0f, inverted.X); // X unchanged
        Assert.Equal(40.0f, inverted.Y); // 100 - (20 + 40) = 40
        Assert.Equal(30.0f, inverted.Width); // Width unchanged
        Assert.Equal(40.0f, inverted.Height); // Height unchanged
    }

    /// <summary>
    /// Test Copy method with all parameters.
    /// </summary>
    [Fact]
    public void Copy_WithAllParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, y: 25.0f, width: 35.0f, height: 45.0f);

        // Assert
        Assert.Equal(15.0f, copy.X);
        Assert.Equal(25.0f, copy.Y);
        Assert.Equal(35.0f, copy.Width);
        Assert.Equal(45.0f, copy.Height);
    }

    /// <summary>
    /// Test Copy method with partial parameters.
    /// </summary>
    [Fact]
    public void Copy_WithPartialParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(x: 15.0f, height: 50.0f);

        // Assert
        Assert.Equal(15.0f, copy.X);
        Assert.Equal(20.0f, copy.Y); // unchanged
        Assert.Equal(30.0f, copy.Width); // unchanged
        Assert.Equal(50.0f, copy.Height);
    }

    /// <summary>
    /// Test Copy method with top parameter.
    /// </summary>
    [Fact]
    public void Copy_WithTopParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(top: 5.0f);

        // Assert
        Assert.Equal(10.0f, copy.X); // unchanged
        Assert.Equal(5.0f, copy.Y); // set to top value
        Assert.Equal(30.0f, copy.Width); // unchanged
        Assert.Equal(40.0f, copy.Height); // unchanged
    }

    /// <summary>
    /// Test Copy method with bottom parameter.
    /// </summary>
    [Fact]
    public void Copy_WithBottomParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 30.0f, 40.0f);

        // Act
        var copy = rect.Copy(bottom: 80.0f);

        // Assert
        Assert.Equal(10.0f, copy.X); // unchanged
        Assert.Equal(20.0f, copy.Y); // unchanged
        Assert.Equal(30.0f, copy.Width); // unchanged
        Assert.Equal(100.0f, copy.Height); // 20 + 80 = 100
    }

    /// <summary>
    /// Test Copy method throws when Y and Top are both specified.
    /// </summary>
    [Fact]
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
    [Fact]
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
    [Fact]
    public void Divide_WithInvalidEdge_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 50.0f);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => rect.Divide(30.0f, (RectEdge)999));
    }
}
