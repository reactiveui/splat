// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

public class SizeMathExtensionsTests
{
    /// <summary>
    /// Test that WithinEpsilonOf returns true when sizes are within epsilon.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsTrue_WhenSizesAreWithinEpsilon()
    {
        // Arrange
        var size1 = new SizeF(10.0f, 20.0f);
        var size2 = new SizeF(10.1f, 20.1f);
        const float epsilon = 0.2f;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns false when sizes are not within epsilon.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsFalse_WhenSizesAreNotWithinEpsilon()
    {
        // Arrange
        var size1 = new SizeF(10.0f, 20.0f);
        var size2 = new SizeF(15.0f, 25.0f);
        const float epsilon = 0.5f;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Test that WithinEpsilonOf handles identical sizes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_HandlesIdenticalSizes()
    {
        // Arrange
        var size = new SizeF(10.0f, 20.0f);
        const float epsilon = 0.1f;

        // Act
        var result = size.WithinEpsilonOf(size, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test that WithinEpsilonOf calculates distance correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_CalculatesDistanceCorrectly()
    {
        // Arrange
        var size1 = new SizeF(0.0f, 0.0f);
        var size2 = new SizeF(3.0f, 4.0f);
        const float epsilon = 5.1f; // Distance is 5.0

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns false when distance exceeds epsilon.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsFalse_WhenDistanceExceedsEpsilon()
    {
        // Arrange
        var size1 = new SizeF(0.0f, 0.0f);
        var size2 = new SizeF(3.0f, 4.0f);
        const float epsilon = 4.9f; // Distance is 5.0

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Test that WithinEpsilonOf handles negative sizes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_HandlesNegativeSizes()
    {
        // Arrange
        var size1 = new SizeF(-10.0f, -20.0f);
        var size2 = new SizeF(-10.1f, -20.1f);
        const float epsilon = 0.2f;

        // Act
        var result = size1.WithinEpsilonOf(size2, epsilon);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test that ScaledBy scales size correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_ScalesSizeCorrectly()
    {
        // Arrange
        var size = new SizeF(4.0f, 6.0f);
        const float factor = 2.5f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(10.0f);
            await Assert.That(result.Height).IsEqualTo(15.0f);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles zero factor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesZeroFactor()
    {
        // Arrange
        var size = new SizeF(4.0f, 6.0f);
        const float factor = 0.0f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(0);
            await Assert.That(result.Height).IsEqualTo(0);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles negative factor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesNegativeFactor()
    {
        // Arrange
        var size = new SizeF(4.0f, 6.0f);
        const float factor = -2.0f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(-8.0f);
            await Assert.That(result.Height).IsEqualTo(-12.0f);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles fractional factor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesFractionalFactor()
    {
        // Arrange
        var size = new SizeF(10.0f, 20.0f);
        const float factor = 0.5f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(5.0f);
            await Assert.That(result.Height).IsEqualTo(10.0f);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles very large factor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesVeryLargeFactor()
    {
        // Arrange
        var size = new SizeF(1.0f, 2.0f);
        const float factor = 1000.0f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(1000.0f);
            await Assert.That(result.Height).IsEqualTo(2000.0f);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles very small factor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesVerySmallFactor()
    {
        // Arrange
        var size = new SizeF(100.0f, 200.0f);
        const float factor = 0.001f;

        // Act
        var result = size.ScaledBy(factor);

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(result.Width).IsEqualTo(0.1f);
            await Assert.That(result.Height).IsEqualTo(0.2f);
        }
    }
}
