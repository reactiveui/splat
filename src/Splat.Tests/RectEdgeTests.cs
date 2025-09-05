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
    [Test]
    [TestCase(RectEdge.Left)]
    [TestCase(RectEdge.Top)]
    [TestCase(RectEdge.Right)]
    [TestCase(RectEdge.Bottom)]
    public void RectEdge_AllValues_WorkWithDivide(RectEdge edge)
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 100.0f);

        // Act & Assert - should not throw
        var (slice, remainder) = rect.Divide(25.0f, edge);

        // Basic validation
        Assert.That(slice.Width > 0 || slice.Height > 0, Is.True);
        Assert.That(remainder.Width >= 0 && remainder.Height >= 0, Is.True);
    }

    /// <summary>
    /// Test that RectEdge enum has expected values.
    /// </summary>
    [Test]
    public void RectEdge_HasExpectedValues()
    {
        // Assert
        Assert.That((int, Is.EqualTo(0))RectEdge.Left);
        Assert.That((int, Is.EqualTo(1))RectEdge.Top);
        Assert.That((int, Is.EqualTo(2))RectEdge.Right);
        Assert.That((int, Is.EqualTo(3))RectEdge.Bottom);
    }

    /// <summary>
    /// Test that RectEdge enum has all expected names.
    /// </summary>
    [Test]
    public void RectEdge_HasAllExpectedNames()
    {
        // Arrange
        var expectedNames = new[] { "Left", "Top", "Right", "Bottom" };
        var actualNames = Enum.GetNames<RectEdge>();

        // Assert
        Assert.That(actualNames.Length, Is.EqualTo(expectedNames.Length));
        foreach (var expectedName in expectedNames)
        {
            Assert.That(actualNames, Does.Contain(expectedName));
        }
    }

    /// <summary>
    /// Test that each RectEdge value can be parsed from string.
    /// </summary>
    /// <param name="edgeName">The name of the edge to parse.</param>
    /// <param name="expectedEdge">The expected edge value.</param>
    [Test]
    [TestCase("Left", RectEdge.Left)]
    [TestCase("Top", RectEdge.Top)]
    [TestCase("Right", RectEdge.Right)]
    [TestCase("Bottom", RectEdge.Bottom)]
    public void RectEdge_CanBeParsedFromString(string edgeName, RectEdge expectedEdge)
    {
        // Act
        var parsed = Enum.Parse<RectEdge>(edgeName);

        // Assert
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
        Assert.That(rightResult.Item1.X, Is.Not.EqualTo(leftResult.Item1.X));
        Assert.That(bottomResult.Item1.Y, Is.Not.EqualTo(topResult.Item1.Y));

        // Left and Right should affect X coordinates differently
        Assert.That(leftResult.Item1.X, Is.EqualTo(rect.X));
        Assert.That(rightResult.Item1.X, Is.Not.EqualTo(rect.X));

        // Top and Bottom should affect Y coordinates differently
        Assert.That(topResult.Item1.Y, Is.EqualTo(rect.Y));
        Assert.That(bottomResult.Item1.Y, Is.Not.EqualTo(rect.Y));
    }
}
