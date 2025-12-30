// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

public class RectEdgeTests
{
    /// <summary>
    /// Test that all RectEdge values work with Divide method.
    /// </summary>
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
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 100.0f);

        // Act & Assert - should not throw
        RectangleF slice = default, remainder = default;
        await Assert.That(() =>
        {
            var result = rect.Divide(25.0f, edge);
            slice = result.Item1;
            remainder = result.Item2;
        }).ThrowsNothing();

        // Basic validation
        using (Assert.Multiple())
        {
            await Assert.That(slice.Width > 0 || slice.Height > 0).IsTrue();
            await Assert.That(remainder.Width).IsGreaterThanOrEqualTo(0f);
            await Assert.That(remainder.Height).IsGreaterThanOrEqualTo(0f);
        }
    }

    /// <summary>
    /// Test that RectEdge enum has expected values.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "TUnitAssertions0005:Assert.That(...) should not be used with a constant value", Justification = "Deliberately checking constant value")]
    public async Task RectEdge_HasExpectedValues()
    {
        using (Assert.Multiple())
        {
            await Assert.That((int)RectEdge.Left).IsEqualTo(0);
            await Assert.That((int)RectEdge.Top).IsEqualTo(1);
            await Assert.That((int)RectEdge.Right).IsEqualTo(2);
            await Assert.That((int)RectEdge.Bottom).IsEqualTo(3);
        }
    }

    /// <summary>
    /// Test that RectEdge enum has all expected names.
    /// </summary>
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

    /// <summary>
    /// Test that each RectEdge value can be parsed from string.
    /// </summary>
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

    /// <summary>
    /// Test that each RectEdge value produces different results with Divide.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RectEdge_ProducesDifferentResultsWithDivide()
    {
        // Arrange
        var rect = new RectangleF(10.0f, 20.0f, 100.0f, 80.0f);
        const float amount = 30.0f;

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
