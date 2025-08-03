// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the RectEdge enum and its usage.
/// </summary>
public class RectEdgeTests
{
    /// <summary>
    /// Test that all RectEdge values work with Divide method.
    /// </summary>
    /// <param name="edge">The edge to test.</param>
    [Theory]
    [InlineData(RectEdge.Left)]
    [InlineData(RectEdge.Top)]
    [InlineData(RectEdge.Right)]
    [InlineData(RectEdge.Bottom)]
    public void RectEdge_AllValues_WorkWithDivide(RectEdge edge)
    {
        // Arrange
        var rect = new RectangleF(0.0f, 0.0f, 100.0f, 100.0f);

        // Act & Assert - should not throw
        var (slice, remainder) = rect.Divide(25.0f, edge);

        // Basic validation
        Assert.True(slice.Width > 0 || slice.Height > 0);
        Assert.True(remainder.Width >= 0 && remainder.Height >= 0);
    }

    /// <summary>
    /// Test that RectEdge enum has expected values.
    /// </summary>
    [Fact]
    public void RectEdge_HasExpectedValues()
    {
        // Assert
        Assert.Equal(0, (int)RectEdge.Left);
        Assert.Equal(1, (int)RectEdge.Top);
        Assert.Equal(2, (int)RectEdge.Right);
        Assert.Equal(3, (int)RectEdge.Bottom);
    }

    /// <summary>
    /// Test that RectEdge enum has all expected names.
    /// </summary>
    [Fact]
    public void RectEdge_HasAllExpectedNames()
    {
        // Arrange
        var expectedNames = new[] { "Left", "Top", "Right", "Bottom" };
        var actualNames = Enum.GetNames<RectEdge>();

        // Assert
        Assert.Equal(expectedNames.Length, actualNames.Length);
        foreach (var expectedName in expectedNames)
        {
            Assert.Contains(expectedName, actualNames);
        }
    }

    /// <summary>
    /// Test that each RectEdge value can be parsed from string.
    /// </summary>
    /// <param name="edgeName">The name of the edge to parse.</param>
    /// <param name="expectedEdge">The expected edge value.</param>
    [Theory]
    [InlineData("Left", RectEdge.Left)]
    [InlineData("Top", RectEdge.Top)]
    [InlineData("Right", RectEdge.Right)]
    [InlineData("Bottom", RectEdge.Bottom)]
    public void RectEdge_CanBeParsedFromString(string edgeName, RectEdge expectedEdge)
    {
        // Act
        var parsed = Enum.Parse<RectEdge>(edgeName);

        // Assert
        Assert.Equal(expectedEdge, parsed);
    }

    /// <summary>
    /// Test that each RectEdge value produces different results with Divide.
    /// </summary>
    [Fact]
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
        Assert.NotEqual(leftResult.Item1.X, rightResult.Item1.X);
        Assert.NotEqual(topResult.Item1.Y, bottomResult.Item1.Y);

        // Left and Right should affect X coordinates differently
        Assert.Equal(rect.X, leftResult.Item1.X);
        Assert.NotEqual(rect.X, rightResult.Item1.X);

        // Top and Bottom should affect Y coordinates differently
        Assert.Equal(rect.Y, topResult.Item1.Y);
        Assert.NotEqual(rect.Y, bottomResult.Item1.Y);
    }
}
