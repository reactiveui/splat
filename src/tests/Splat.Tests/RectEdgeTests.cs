// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>Tests for the <see cref="RectEdge"/> enumeration.</summary>
public class RectEdgeTests
{
    /// <summary>Test that all RectEdge values work with Divide method.</summary>
    /// <param name="edge">The edge to test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(RectEdge.Left)]
    [Arguments(RectEdge.Top)]
    [Arguments(RectEdge.Right)]
    [Arguments(RectEdge.Bottom)]
    public async Task RectEdge_AllValues_WorkWithDivide(RectEdge edge)
    {
        // Arrange
        const float rectSize = 100.0F;
        const float sliceAmount = 25.0F;
        var rect = new RectangleF(0.0F, 0.0F, rectSize, rectSize);

        // Act & Assert - should not throw
        RectangleF slice = default;
        RectangleF remainder = default;
        await Assert.That(() =>
        {
            var result = rect.Divide(sliceAmount, edge);
            slice = result.Item1;
            remainder = result.Item2;
        }).ThrowsNothing();

        // Basic validation
        using (Assert.Multiple())
        {
            await Assert.That(slice.Width > 0 || slice.Height > 0).IsTrue();
            await Assert.That(remainder.Width).IsGreaterThanOrEqualTo(0F);
            await Assert.That(remainder.Height).IsGreaterThanOrEqualTo(0F);
        }
    }

    /// <summary>Test that RectEdge enum has expected values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "TUnitAssertions0005:Assert.That(...) should not be used with a constant value", Justification = "Deliberately checking constant value")]
    public async Task RectEdge_HasExpectedValues()
    {
        using (Assert.Multiple())
        {
            const int leftOrdinal = 0;
            const int topOrdinal = leftOrdinal + 1;
            const int rightOrdinal = topOrdinal + 1;
            const int bottomOrdinal = rightOrdinal + 1;
            await Assert.That((int)RectEdge.Left).IsEqualTo(leftOrdinal);
            await Assert.That((int)RectEdge.Top).IsEqualTo(topOrdinal);
            await Assert.That((int)RectEdge.Right).IsEqualTo(rightOrdinal);
            await Assert.That((int)RectEdge.Bottom).IsEqualTo(bottomOrdinal);
        }
    }

    /// <summary>Test that RectEdge enum has all expected names.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RectEdge_HasAllExpectedNames()
    {
        var expectedNames = new[] { "Left", "Top", "Right", "Bottom" };
#if NET8_0_OR_GREATER
        var actualNames = Enum.GetNames<RectEdge>();
#else
        var actualNames = Enum.GetNames(typeof(RectEdge));
#endif

        using (Assert.Multiple())
        {
            await Assert.That(actualNames.Length).IsEqualTo(expectedNames.Length);
            foreach (var expectedName in expectedNames)
            {
                await Assert.That(actualNames).Contains(expectedName);
            }
        }
    }

    /// <summary>Test that each RectEdge value can be parsed from string.</summary>
    /// <param name="edgeName">The name of the edge to parse.</param>
    /// <param name="expectedEdge">The expected edge value.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("Left", RectEdge.Left)]
    [Arguments("Top", RectEdge.Top)]
    [Arguments("Right", RectEdge.Right)]
    [Arguments("Bottom", RectEdge.Bottom)]
    public async Task RectEdge_CanBeParsedFromString(string edgeName, RectEdge expectedEdge)
    {
#if NET8_0_OR_GREATER
        var parsed = Enum.Parse<RectEdge>(edgeName);
#else
        var parsed = (RectEdge)Enum.Parse(typeof(RectEdge), edgeName);
#endif
        await Assert.That(parsed).IsEqualTo(expectedEdge);
    }

    /// <summary>Test that each RectEdge value produces different results with Divide.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RectEdge_ProducesDifferentResultsWithDivide()
    {
        // Arrange
        const float rectX = 10.0F;
        const float rectY = 20.0F;
        const float rectWidth = 100.0F;
        const float rectHeight = 80.0F;
        var rect = new RectangleF(rectX, rectY, rectWidth, rectHeight);
        const float amount = 30.0F;

        // Act
        var leftResult = rect.Divide(amount, RectEdge.Left);
        var topResult = rect.Divide(amount, RectEdge.Top);
        var rightResult = rect.Divide(amount, RectEdge.Right);
        var bottomResult = rect.Divide(amount, RectEdge.Bottom);

        // Assert - Each should produce different slice positions
        using (Assert.Multiple())
        {
            await Assert.That(leftResult.Item1.X).IsNotEqualTo(rightResult.Item1.X);
            await Assert.That(topResult.Item1.Y).IsNotEqualTo(bottomResult.Item1.Y);

            // Left and Right should affect X coordinates differently
            await Assert.That(leftResult.Item1.X).IsEqualTo(rect.X);
            await Assert.That(rightResult.Item1.X).IsNotEqualTo(rect.X);

            // Top and Bottom should affect Y coordinates differently
            await Assert.That(topResult.Item1.Y).IsEqualTo(rect.Y);
            await Assert.That(bottomResult.Item1.Y).IsNotEqualTo(rect.Y);
        }
    }
}
