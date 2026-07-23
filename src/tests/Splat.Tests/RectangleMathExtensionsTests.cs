// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>Tests for the rectangle math extension methods.</summary>
public class RectangleMathExtensionsTests
{
    /// <summary>The X coordinate of the sample test rectangle.</summary>
    private const float RectX = 10.0F;

    /// <summary>The Y coordinate of the sample test rectangle.</summary>
    private const float RectY = 20.0F;

    /// <summary>The width of the sample test rectangle.</summary>
    private const float RectWidth = 30.0F;

    /// <summary>The height of the sample test rectangle.</summary>
    private const float RectHeight = 40.0F;

    /// <summary>The X coordinate of the origin used for container rectangles.</summary>
    private const float OriginX = 0.0F;

    /// <summary>The Y coordinate of the origin used for container rectangles.</summary>
    private const float OriginY = 0.0F;

    /// <summary>The width of the wide rectangle used for divide tests.</summary>
    private const float WideWidth = 100.0F;

    /// <summary>The height of the wide rectangle used for divide tests.</summary>
    private const float WideHeight = 50.0F;

    /// <summary>The horizontal amount sliced off in divide tests.</summary>
    private const float SliceAmount = 30.0F;

    /// <summary>The vertical amount sliced off in divide tests.</summary>
    private const float VerticalSliceAmount = 20.0F;

    /// <summary>The padding amount applied in divide-with-padding tests.</summary>
    private const float Padding = 10.0F;

    /// <summary>The divisor used to compute the centre point of a rectangle.</summary>
    private const float Half = 2.0F;

    /// <summary>Test that Center method calculates correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Center_CalculatesCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);

        // Act
        var center = rect.Center();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(center.X).IsEqualTo(RectX + (RectWidth / Half));
            await Assert.That(center.Y).IsEqualTo(RectY + (RectHeight / Half));
        }
    }

    /// <summary>Test Divide method from left edge.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromLeftEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);

        // Act
        var (slice, remainder) = rect.Divide(SliceAmount, RectEdge.Left);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(SliceAmount);
            await Assert.That(slice.X).IsEqualTo(OriginX);
            await Assert.That(remainder.Width).IsEqualTo(WideWidth - SliceAmount);
            await Assert.That(remainder.X).IsEqualTo(SliceAmount);
        }
    }

    /// <summary>Test Divide method from right edge.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromRightEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);

        // Act
        var (slice, remainder) = rect.Divide(SliceAmount, RectEdge.Right);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(SliceAmount);
            await Assert.That(slice.X).IsEqualTo(WideWidth - SliceAmount);
            await Assert.That(remainder.Width).IsEqualTo(WideWidth - SliceAmount);
            await Assert.That(remainder.X).IsEqualTo(OriginX);
        }
    }

    /// <summary>Test Divide method from top edge.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromTopEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);

        // Act
        var (slice, remainder) = rect.Divide(VerticalSliceAmount, RectEdge.Top);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Height).IsEqualTo(VerticalSliceAmount);
            await Assert.That(slice.Y).IsEqualTo(OriginY);
            await Assert.That(remainder.Height).IsEqualTo(WideHeight - VerticalSliceAmount);
            await Assert.That(remainder.Y).IsEqualTo(VerticalSliceAmount);
        }
    }

    /// <summary>Test Divide method from bottom edge.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Divide_FromBottomEdge_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);

        // Act
        var (slice, remainder) = rect.Divide(VerticalSliceAmount, RectEdge.Bottom);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Height).IsEqualTo(VerticalSliceAmount);
            await Assert.That(slice.Y).IsEqualTo(WideHeight - VerticalSliceAmount);
            await Assert.That(remainder.Height).IsEqualTo(WideHeight - VerticalSliceAmount);
            await Assert.That(remainder.Y).IsEqualTo(OriginY);
        }
    }

    /// <summary>Test DivideWithPadding method.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DivideWithPadding_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);

        // Act
        var (slice, remainder) = rect.DivideWithPadding(SliceAmount, Padding, RectEdge.Left);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(slice.Width).IsEqualTo(SliceAmount);
            await Assert.That(slice.X).IsEqualTo(OriginX);
            await Assert.That(remainder.Width).IsEqualTo(WideWidth - Padding); // remaining width after padding
            await Assert.That(remainder.X).IsEqualTo(Padding); // starts after padding
        }
    }

    /// <summary>Test InvertWithin method.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvertWithin_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float containerWidth = 200.0F;
        const float containerHeight = 100.0F;
        var container = new RectangleF(OriginX, OriginY, containerWidth, containerHeight);

        // Act
        var inverted = rect.InvertWithin(container);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(inverted.X).IsEqualTo(RectX); // X unchanged
            await Assert.That(inverted.Y).IsEqualTo(containerHeight - (RectY + RectHeight)); // inverted vertical position
            await Assert.That(inverted.Width).IsEqualTo(RectWidth); // Width unchanged
            await Assert.That(inverted.Height).IsEqualTo(RectHeight); // Height unchanged
        }
    }

    /// <summary>Test Copy method with all parameters.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithAllParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newX = 15.0F;
        const float newY = 25.0F;
        const float newWidth = 35.0F;
        const float newHeight = 45.0F;

        // Act
        var copy = rect.Copy(new() { X = newX, Y = newY, Width = newWidth, Height = newHeight });

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(newX);
            await Assert.That(copy.Y).IsEqualTo(newY);
            await Assert.That(copy.Width).IsEqualTo(newWidth);
            await Assert.That(copy.Height).IsEqualTo(newHeight);
        }
    }

    /// <summary>Test Copy method with partial parameters.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithPartialParameters_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newX = 15.0F;
        const float newHeight = 50.0F;

        // Act
        var copy = rect.Copy(new() { X = newX, Height = newHeight });

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(newX);
            await Assert.That(copy.Y).IsEqualTo(RectY); // unchanged
            await Assert.That(copy.Width).IsEqualTo(RectWidth); // unchanged
            await Assert.That(copy.Height).IsEqualTo(newHeight);
        }
    }

    /// <summary>Test Copy method with top parameter.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithTopParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newTop = 5.0F;

        // Act
        var copy = rect.Copy(new() { Top = newTop });

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(RectX); // unchanged
            await Assert.That(copy.Y).IsEqualTo(newTop); // set to top value
            await Assert.That(copy.Width).IsEqualTo(RectWidth); // unchanged
            await Assert.That(copy.Height).IsEqualTo(RectHeight); // unchanged
        }
    }

    /// <summary>Test Copy method with bottom parameter.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Copy_WithBottomParameter_WorksCorrectly()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newBottom = 80.0F;

        // Act
        var copy = rect.Copy(new() { Bottom = newBottom });

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(copy.X).IsEqualTo(RectX); // unchanged
            await Assert.That(copy.Y).IsEqualTo(RectY); // unchanged
            await Assert.That(copy.Width).IsEqualTo(RectWidth); // unchanged
            await Assert.That(copy.Height).IsEqualTo(RectY + newBottom); // height grows to reach the new bottom
        }
    }

    /// <summary>Test Copy method throws when Y and Top are both specified.</summary>
    [Test]
    public void Copy_WithYAndTop_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newY = 25.0F;
        const float newTop = 5.0F;

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => rect.Copy(new() { Y = newY, Top = newTop }));
    }

    /// <summary>Test Copy method throws when Height and Bottom are both specified.</summary>
    [Test]
    public void Copy_WithHeightAndBottom_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(RectX, RectY, RectWidth, RectHeight);
        const float newHeight = 50.0F;
        const float newBottom = 80.0F;

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => rect.Copy(new() { Height = newHeight, Bottom = newBottom }));
    }

    /// <summary>Test Divide with invalid edge throws ArgumentException.</summary>
    [Test]
    public void Divide_WithInvalidEdge_ThrowsArgumentException()
    {
        // Arrange
        var rect = new RectangleF(OriginX, OriginY, WideWidth, WideHeight);
        const int invalidEdge = 999;

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => rect.Divide(SliceAmount, (RectEdge)invalidEdge));
    }
}
