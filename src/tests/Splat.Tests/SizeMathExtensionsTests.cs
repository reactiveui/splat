// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>Tests for the size math extension methods.</summary>
public class SizeMathExtensionsTests
{
    /// <summary>Test that WithinEpsilonOf returns true when sizes are within epsilon.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsTrue_WhenSizesAreWithinEpsilon()
    {
        // Arrange
        const float size1Width = 10.0F;
        const float size1Height = 20.0F;
        const float size2Width = 10.1F;
        const float size2Height = 20.1F;
        var size1 = new SizeF(size1Width, size1Height);
        var size2 = new SizeF(size2Width, size2Height);
        const float epsilon = 0.2F;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that WithinEpsilonOf returns false when sizes are not within epsilon.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsFalse_WhenSizesAreNotWithinEpsilon()
    {
        // Arrange
        const float size1Width = 10.0F;
        const float size1Height = 20.0F;
        const float size2Width = 15.0F;
        const float size2Height = 25.0F;
        var size1 = new SizeF(size1Width, size1Height);
        var size2 = new SizeF(size2Width, size2Height);
        const float epsilon = 0.5F;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>Test that WithinEpsilonOf handles identical sizes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_HandlesIdenticalSizes()
    {
        // Arrange
        const float sizeWidth = 10.0F;
        const float sizeHeight = 20.0F;
        var size = new SizeF(sizeWidth, sizeHeight);
        const float epsilon = 0.1F;

        // Act
        var result = size.WithinEpsilonOf(size, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that WithinEpsilonOf calculates distance correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_CalculatesDistanceCorrectly()
    {
        // Arrange
        var size1 = new SizeF(0.0F, 0.0F);
        const float size2Width = 3.0F;
        const float size2Height = 4.0F;
        var size2 = new SizeF(size2Width, size2Height);
        const float epsilon = 5.1F; // Distance is 5.0

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that WithinEpsilonOf returns false when distance exceeds epsilon.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsFalse_WhenDistanceExceedsEpsilon()
    {
        // Arrange
        var size1 = new SizeF(0.0F, 0.0F);
        const float size2Width = 3.0F;
        const float size2Height = 4.0F;
        var size2 = new SizeF(size2Width, size2Height);
        const float epsilon = 4.9F; // Distance is 5.0

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>Test that WithinEpsilonOf handles negative sizes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_HandlesNegativeSizes()
    {
        // Arrange
        const float size1Width = -10.0F;
        const float size1Height = -20.0F;
        const float size2Width = -10.1F;
        const float size2Height = -20.1F;
        var size1 = new SizeF(size1Width, size1Height);
        var size2 = new SizeF(size2Width, size2Height);
        const float epsilon = 0.2F;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that ScaledBy scales size correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_ScalesSizeCorrectly()
    {
        // Arrange
        const float width = 4.0F;
        const float height = 6.0F;
        var size = new SizeF(width, height);
        const float factor = 2.5F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(width * factor);
            await Assert.That(result.Height).IsEqualTo(height * factor);
        }
    }

    /// <summary>Test that ScaledBy handles zero factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesZeroFactor()
    {
        // Arrange
        const float width = 4.0F;
        const float height = 6.0F;
        var size = new SizeF(width, height);
        const float factor = 0.0F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(0);
            await Assert.That(result.Height).IsEqualTo(0);
        }
    }

    /// <summary>Test that ScaledBy handles negative factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesNegativeFactor()
    {
        // Arrange
        const float width = 4.0F;
        const float height = 6.0F;
        var size = new SizeF(width, height);
        const float factor = -2.0F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(width * factor);
            await Assert.That(result.Height).IsEqualTo(height * factor);
        }
    }

    /// <summary>Test that ScaledBy handles fractional factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesFractionalFactor()
    {
        // Arrange
        const float width = 10.0F;
        const float height = 20.0F;
        var size = new SizeF(width, height);
        const float factor = 0.5F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(width * factor);
            await Assert.That(result.Height).IsEqualTo(height * factor);
        }
    }

    /// <summary>Test that ScaledBy handles very large factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesVeryLargeFactor()
    {
        // Arrange
        const float width = 1.0F;
        const float height = 2.0F;
        var size = new SizeF(width, height);
        const float factor = 1000.0F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(width * factor);
            await Assert.That(result.Height).IsEqualTo(height * factor);
        }
    }

    /// <summary>Test that ScaledBy handles very small factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesVerySmallFactor()
    {
        // Arrange
        const float width = 100.0F;
        const float height = 200.0F;
        var size = new SizeF(width, height);
        const float factor = 0.001F;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(width * factor);
            await Assert.That(result.Height).IsEqualTo(height * factor);
        }
    }
}
