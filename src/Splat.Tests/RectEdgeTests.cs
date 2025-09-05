// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the RectEdge enum and its usage.
/// </summary>
[TestFixture]
public class RectEdgeTests
{
    /// <summary>
    /// Test that all RectEdge values work with Divide method.
    /// </summary>
    /// <param name="edge">The edge to test.</param>
    [TestCase(RectEdge.Left)]
    [TestCase(RectEdge.Top)]
    [TestCase(RectEdge.Right)]
    [TestCase(RectEdge.Bottom)]
    public void RectEdge_AllValues_WorkWithDivide(RectEdge edge)
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 100.0f);

        // Act & Assert - should not throw
        RectangleF slice = default, remainder = default;
        Assert.DoesNotThrow(() =>
        {
            var result = rect.Divide(25.0f, edge);
            slice = result.Item1;
            remainder = result.Item2;
        });

        // Basic validation
        using (Assert.EnterMultipleScope())
        {
            Assert.That(slice.Width > 0 || slice.Height > 0, Is.True);
            Assert.That(remainder.Width, Is.GreaterThanOrEqualTo(0f));
            Assert.That(remainder.Height, Is.GreaterThanOrEqualTo(0f));
        }
    }

    /// <summary>
    /// Test that RectEdge enum has expected values.
    /// </summary>
    [Test]
    public void RectEdge_HasExpectedValues()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((int)RectEdge.Left, Is.Zero);
            Assert.That((int)RectEdge.Top, Is.EqualTo(1));
            Assert.That((int)RectEdge.Right, Is.EqualTo(2));
            Assert.That((int)RectEdge.Bottom, Is.EqualTo(3));
        }
    }

    /// <summary>
    /// Test that RectEdge enum has all expected names.
    /// </summary>
    [Test]
    public void RectEdge_HasAllExpectedNames()
    {
        var expectedNames = new[] { "Left", "Top", "Right", "Bottom" };
        var actualNames = Enum.GetNames<RectEdge>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actualNames, Has.Length.EqualTo(expectedNames.Length));
            foreach (var expectedName in expectedNames)
            {
                Assert.That(actualNames, Does.Contain(expectedName));
            }
        }
    }

    /// <summary>
    /// Test that each RectEdge value can be parsed from string.
    /// </summary>
    /// <param name="edgeName">The name of the edge to parse.</param>
    /// <param name="expectedEdge">The expected edge value.</param>
    [TestCase("Left", RectEdge.Left)]
    [TestCase("Top", RectEdge.Top)]
    [TestCase("Right", RectEdge.Right)]
    [TestCase("Bottom", RectEdge.Bottom)]
    public void RectEdge_CanBeParsedFromString(string edgeName, RectEdge expectedEdge)
    {
        var parsed = Enum.Parse<RectEdge>(edgeName);
        Assert.That(parsed, Is.EqualTo(expectedEdge));
    }

    /// <summary>
    /// Test that each RectEdge value produces different results with Divide.
    /// </summary>
    [Test]
    public void RectEdge_ProducesDifferentResultsWithDivide()
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
        using (Assert.EnterMultipleScope())
        {
            Assert.That(leftResult.Item1.X, Is.Not.EqualTo(rightResult.Item1.X));
            Assert.That(topResult.Item1.Y, Is.Not.EqualTo(bottomResult.Item1.Y));

            // Left and Right should affect X coordinates differently
            Assert.That(leftResult.Item1.X, Is.EqualTo(rect.X));
            Assert.That(rightResult.Item1.X, Is.Not.EqualTo(rect.X));

            // Top and Bottom should affect Y coordinates differently
            Assert.That(topResult.Item1.Y, Is.EqualTo(rect.Y));
            Assert.That(bottomResult.Item1.Y, Is.Not.EqualTo(rect.Y));
        }
    }
}
