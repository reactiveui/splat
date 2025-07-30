// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the PointMathExtensions class.
/// </summary>
public class PointMathExtensionsTests
{
    /// <summary>
    /// Test that Floor method correctly floors point values.
    /// </summary>
    [Fact]
    public void Floor_CorrectlyFloorsPoint()
    {
        // Arrange
        var point = new Point(3, 4);

        // Act
        var result = point.Floor();

        // Assert
        Assert.Equal(3.0f, result.X);
        Assert.Equal(4.0f, result.Y);
    }

    /// <summary>
    /// Test that Floor method handles negative values.
    /// </summary>
    [Fact]
    public void Floor_HandlesNegativeValues()
    {
        // Arrange
        var point = new Point(-3, -4);

        // Act
        var result = point.Floor();

        // Assert
        Assert.Equal(-3.0f, result.X);
        Assert.Equal(-4.0f, result.Y);
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns true when points are within epsilon.
    /// </summary>
    [Fact]
    public void WithinEpsilonOf_ReturnsTrue_WhenPointsAreWithinEpsilon()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(1.1f, 2.1f);
        const float epsilon = 0.2f;

        // Act
        var result = point1.WithinEpsilonOf(point2, epsilon);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns false when points are not within epsilon.
    /// </summary>
    [Fact]
    public void WithinEpsilonOf_ReturnsFalse_WhenPointsAreNotWithinEpsilon()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(5.0f, 6.0f);
        const float epsilon = 0.5f;

        // Act
        var result = point1.WithinEpsilonOf(point2, epsilon);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Test that WithinEpsilonOf handles identical points.
    /// </summary>
    [Fact]
    public void WithinEpsilonOf_HandlesIdenticalPoints()
    {
        // Arrange
        var point = new PointF(1.0f, 2.0f);
        const float epsilon = 0.1f;

        // Act
        var result = point.WithinEpsilonOf(point, epsilon);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Test that DotProduct calculates correctly.
    /// </summary>
    [Fact]
    public void DotProduct_CalculatesCorrectly()
    {
        // Arrange
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(2.0f, 1.0f);

        // Act
        var result = point1.DotProduct(point2);

        // Assert
        Assert.Equal(10.0f, result); // (3*2) + (4*1) = 6 + 4 = 10
    }

    /// <summary>
    /// Test that DotProduct handles zero vectors.
    /// </summary>
    [Fact]
    public void DotProduct_HandlesZeroVectors()
    {
        // Arrange
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(0.0f, 0.0f);

        // Act
        var result = point1.DotProduct(point2);

        // Assert
        Assert.Equal(0.0f, result);
    }

    /// <summary>
    /// Test that ScaledBy scales point correctly.
    /// </summary>
    [Fact]
    public void ScaledBy_ScalesPointCorrectly()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = 2.5f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.Equal(5.0f, result.X);
        Assert.Equal(7.5f, result.Y);
    }

    /// <summary>
    /// Test that ScaledBy handles zero factor.
    /// </summary>
    [Fact]
    public void ScaledBy_HandlesZeroFactor()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = 0.0f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.Equal(0.0f, result.X);
        Assert.Equal(0.0f, result.Y);
    }

    /// <summary>
    /// Test that ScaledBy handles negative factor.
    /// </summary>
    [Fact]
    public void ScaledBy_HandlesNegativeFactor()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = -2.0f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.Equal(-4.0f, result.X);
        Assert.Equal(-6.0f, result.Y);
    }

    /// <summary>
    /// Test that Length calculates magnitude correctly.
    /// </summary>
    [Fact]
    public void Length_CalculatesMagnitudeCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);

        // Act
        var result = point.Length();

        // Assert
        Assert.Equal(5.0f, result, 5); // sqrt(3^2 + 4^2) = sqrt(9 + 16) = sqrt(25) = 5
    }

    /// <summary>
    /// Test that Length handles zero vector.
    /// </summary>
    [Fact]
    public void Length_HandlesZeroVector()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.Length();

        // Assert
        Assert.Equal(0.0f, result);
    }

    /// <summary>
    /// Test that Normalize creates unit vector correctly.
    /// </summary>
    [Fact]
    public void Normalize_CreatesUnitVectorCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);

        // Act
        var result = point.Normalize();

        // Assert
        Assert.Equal(0.6f, result.X, 5);
        Assert.Equal(0.8f, result.Y, 5);
        Assert.Equal(1.0f, result.Length(), 5);
    }

    /// <summary>
    /// Test that Normalize handles zero vector correctly.
    /// </summary>
    [Fact]
    public void Normalize_HandlesZeroVectorCorrectly()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.Normalize();

        // Assert
        Assert.Equal(0.0f, result.X);
        Assert.Equal(0.0f, result.Y);
    }

    /// <summary>
    /// Test that AngleInDegrees calculates angle correctly.
    /// </summary>
    [Fact]
    public void AngleInDegrees_CalculatesAngleCorrectly()
    {
        // Arrange
        var point = new PointF(1.0f, 1.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.Equal(45.0f, result, 1);
    }

    /// <summary>
    /// Test that AngleInDegrees handles negative coordinates.
    /// </summary>
    [Fact]
    public void AngleInDegrees_HandlesNegativeCoordinates()
    {
        // Arrange
        var point = new PointF(-1.0f, 1.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.Equal(135.0f, result, 1);
    }

    /// <summary>
    /// Test that AngleInDegrees handles zero vector.
    /// </summary>
    [Fact]
    public void AngleInDegrees_HandlesZeroVector()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.Equal(0.0f, result);
    }

    /// <summary>
    /// Test that ProjectAlong projects correctly.
    /// </summary>
    [Fact]
    public void ProjectAlong_ProjectsCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(1.0f, 0.0f); // Unit vector along X-axis

        // Act
        var result = point.ProjectAlong(direction);

        // Assert
        Assert.Equal(3.0f, result.X);
        Assert.Equal(0.0f, result.Y);
    }

    /// <summary>
    /// Test that ProjectAlong handles zero direction vector.
    /// </summary>
    [Fact]
    public void ProjectAlong_HandlesZeroDirection()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(0.0f, 0.0f);

        // Act
        var result = point.ProjectAlong(direction);

        // Assert
        // When direction is zero, normalized direction is also zero, result should be zero
        Assert.Equal(0.0f, result.X);
        Assert.Equal(0.0f, result.Y);
    }

    /// <summary>
    /// Test that ProjectAlongAngle projects correctly.
    /// </summary>
    [Fact]
    public void ProjectAlongAngle_ProjectsCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        const float angle = 0.0f; // Along X-axis

        // Act
        var result = point.ProjectAlongAngle(angle);

        // Assert
        Assert.Equal(3.0f, result.X, 5);
        Assert.Equal(0.0f, result.Y, 5);
    }

    /// <summary>
    /// Test that ProjectAlongAngle works with different angles.
    /// </summary>
    [Fact]
    public void ProjectAlongAngle_WorksWithDifferentAngles()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        const float angle = 90.0f; // Along Y-axis

        // Act
        var result = point.ProjectAlongAngle(angle);

        // Assert
        Assert.Equal(0.0f, result.X, 5);
        Assert.Equal(4.0f, result.Y, 5);
    }

    /// <summary>
    /// Test that DistanceTo calculates distance correctly.
    /// </summary>
    [Fact]
    public void DistanceTo_CalculatesDistanceCorrectly()
    {
        // Arrange
        var point1 = new PointF(0.0f, 0.0f);
        var point2 = new PointF(3.0f, 4.0f);

        // Act
        var result = point1.DistanceTo(point2);

        // Assert
        Assert.Equal(5.0f, result, 5); // sqrt(3^2 + 4^2) = 5
    }

    /// <summary>
    /// Test that DistanceTo handles same points.
    /// </summary>
    [Fact]
    public void DistanceTo_HandlesSamePoints()
    {
        // Arrange
        var point = new PointF(1.0f, 2.0f);

        // Act
        var result = point.DistanceTo(point);

        // Assert
        Assert.Equal(0.0f, result);
    }

    /// <summary>
    /// Test that DistanceTo is symmetric.
    /// </summary>
    [Fact]
    public void DistanceTo_IsSymmetric()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(4.0f, 6.0f);

        // Act
        var distance1 = point1.DistanceTo(point2);
        var distance2 = point2.DistanceTo(point1);

        // Assert
        Assert.Equal(distance1, distance2, 5);
    }
}
