// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the PointMathExtensions class.
/// </summary>
[TestFixture]
public class PointMathExtensionsTests
{
    /// <summary>
    /// Test that Floor method correctly floors point values.
    /// </summary>
    [Test]
    public void Floor_CorrectlyFloorsPoint()
    {
        // Arrange
        var point = new Point(3, 4);

        // Act
        var result = point.Floor();

        // Assert
        Assert.That(result.X, Is.EqualTo(3.0f));
        Assert.That(result.Y, Is.EqualTo(4.0f));
    }

    /// <summary>
    /// Test that Floor method handles negative values.
    /// </summary>
    [Test]
    public void Floor_HandlesNegativeValues()
    {
        // Arrange
        var point = new Point(-3, -4);

        // Act
        var result = point.Floor();

        // Assert
        Assert.That(result.X, Is.EqualTo(-3.0f));
        Assert.That(result.Y, Is.EqualTo(-4.0f));
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns true when points are within epsilon.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_ReturnsTrue_WhenPointsAreWithinEpsilon()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(1.1f, 2.1f);
        const float epsilon = 0.2f;

        // Act
        var result = point1.WithinEpsilonOf(point2, epsilon);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns false when points are not within epsilon.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_ReturnsFalse_WhenPointsAreNotWithinEpsilon()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(5.0f, 6.0f);
        const float epsilon = 0.5f;

        // Act
        var result = point1.WithinEpsilonOf(point2, epsilon);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Test that WithinEpsilonOf handles identical points.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_HandlesIdenticalPoints()
    {
        // Arrange
        var point = new PointF(1.0f, 2.0f);
        const float epsilon = 0.1f;

        // Act
        var result = point.WithinEpsilonOf(point, epsilon);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Test that DotProduct calculates correctly.
    /// </summary>
    [Test]
    public void DotProduct_CalculatesCorrectly()
    {
        // Arrange
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(2.0f, 1.0f);

        // Act
        var result = point1.DotProduct(point2);

        // Assert
        Assert.That(result, Is.EqualTo(10.0f)); // (3*2) + (4*1) = 6 + 4 = 10
    }

    /// <summary>
    /// Test that DotProduct handles zero vectors.
    /// </summary>
    [Test]
    public void DotProduct_HandlesZeroVectors()
    {
        // Arrange
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(0.0f, 0.0f);

        // Act
        var result = point1.DotProduct(point2);

        // Assert
        Assert.That(result, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ScaledBy scales point correctly.
    /// </summary>
    [Test]
    public void ScaledBy_ScalesPointCorrectly()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = 2.5f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.That(result.X, Is.EqualTo(5.0f));
        Assert.That(result.Y, Is.EqualTo(7.5f));
    }

    /// <summary>
    /// Test that ScaledBy handles zero factor.
    /// </summary>
    [Test]
    public void ScaledBy_HandlesZeroFactor()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = 0.0f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.That(result.X, Is.EqualTo(0.0f));
        Assert.That(result.Y, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ScaledBy handles negative factor.
    /// </summary>
    [Test]
    public void ScaledBy_HandlesNegativeFactor()
    {
        // Arrange
        var point = new PointF(2.0f, 3.0f);
        const float factor = -2.0f;

        // Act
        var result = point.ScaledBy(factor);

        // Assert
        Assert.That(result.X, Is.EqualTo(-4.0f));
        Assert.That(result.Y, Is.EqualTo(-6.0f));
    }

    /// <summary>
    /// Test that Length calculates magnitude correctly.
    /// </summary>
    [Test]
    public void Length_CalculatesMagnitudeCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);

        // Act
        var result = point.Length();

        // Assert
        Assert.That(result, 5, Is.EqualTo(5.0f)); // sqrt(3^2 + 4^2) = sqrt(9 + 16) = sqrt(25) = 5
    }

    /// <summary>
    /// Test that Length handles zero vector.
    /// </summary>
    [Test]
    public void Length_HandlesZeroVector()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.Length();

        // Assert
        Assert.That(result, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that Normalize creates unit vector correctly.
    /// </summary>
    [Test]
    public void Normalize_CreatesUnitVectorCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);

        // Act
        var result = point.Normalize();

        // Assert
        Assert.That(result.X, 5, Is.EqualTo(0.6f));
        Assert.That(result.Y, 5, Is.EqualTo(0.8f));
        Assert.That(result.Length(, Is.EqualTo(1.0f)), 5);
    }

    /// <summary>
    /// Test that Normalize handles zero vector correctly.
    /// </summary>
    [Test]
    public void Normalize_HandlesZeroVectorCorrectly()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.Normalize();

        // Assert
        Assert.That(result.X, Is.EqualTo(0.0f));
        Assert.That(result.Y, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that AngleInDegrees calculates angle correctly.
    /// </summary>
    [Test]
    public void AngleInDegrees_CalculatesAngleCorrectly()
    {
        // Arrange
        var point = new PointF(1.0f, 1.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.That(result, 1, Is.EqualTo(45.0f));
    }

    /// <summary>
    /// Test that AngleInDegrees handles negative coordinates.
    /// </summary>
    [Test]
    public void AngleInDegrees_HandlesNegativeCoordinates()
    {
        // Arrange
        var point = new PointF(-1.0f, 1.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.That(result, 1, Is.EqualTo(135.0f));
    }

    /// <summary>
    /// Test that AngleInDegrees handles zero vector.
    /// </summary>
    [Test]
    public void AngleInDegrees_HandlesZeroVector()
    {
        // Arrange
        var point = new PointF(0.0f, 0.0f);

        // Act
        var result = point.AngleInDegrees();

        // Assert
        Assert.That(result, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ProjectAlong projects correctly.
    /// </summary>
    [Test]
    public void ProjectAlong_ProjectsCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(1.0f, 0.0f); // Unit vector along X-axis

        // Act
        var result = point.ProjectAlong(direction);

        // Assert
        Assert.That(result.X, Is.EqualTo(3.0f));
        Assert.That(result.Y, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ProjectAlong handles zero direction vector.
    /// </summary>
    [Test]
    public void ProjectAlong_HandlesZeroDirection()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(0.0f, 0.0f);

        // Act
        var result = point.ProjectAlong(direction);

        // Assert
        // When direction is zero, normalized direction is also zero, result should be zero
        Assert.That(result.X, Is.EqualTo(0.0f));
        Assert.That(result.Y, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ProjectAlongAngle projects correctly.
    /// </summary>
    [Test]
    public void ProjectAlongAngle_ProjectsCorrectly()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        const float angle = 0.0f; // Along X-axis

        // Act
        var result = point.ProjectAlongAngle(angle);

        // Assert
        Assert.That(result.X, 5, Is.EqualTo(3.0f));
        Assert.That(result.Y, 5, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that ProjectAlongAngle works with different angles.
    /// </summary>
    [Test]
    public void ProjectAlongAngle_WorksWithDifferentAngles()
    {
        // Arrange
        var point = new PointF(3.0f, 4.0f);
        const float angle = 90.0f; // Along Y-axis

        // Act
        var result = point.ProjectAlongAngle(angle);

        // Assert
        Assert.That(result.X, 5, Is.EqualTo(0.0f));
        Assert.That(result.Y, 5, Is.EqualTo(4.0f));
    }

    /// <summary>
    /// Test that DistanceTo calculates distance correctly.
    /// </summary>
    [Test]
    public void DistanceTo_CalculatesDistanceCorrectly()
    {
        // Arrange
        var point1 = new PointF(0.0f, 0.0f);
        var point2 = new PointF(3.0f, 4.0f);

        // Act
        var result = point1.DistanceTo(point2);

        // Assert
        Assert.That(result, 5, Is.EqualTo(5.0f)); // sqrt(3^2 + 4^2) = 5
    }

    /// <summary>
    /// Test that DistanceTo handles same points.
    /// </summary>
    [Test]
    public void DistanceTo_HandlesSamePoints()
    {
        // Arrange
        var point = new PointF(1.0f, 2.0f);

        // Act
        var result = point.DistanceTo(point);

        // Assert
        Assert.That(result, Is.EqualTo(0.0f));
    }

    /// <summary>
    /// Test that DistanceTo is symmetric.
    /// </summary>
    [Test]
    public void DistanceTo_IsSymmetric()
    {
        // Arrange
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(4.0f, 6.0f);

        // Act
        var distance1 = point1.DistanceTo(point2);
        var distance2 = point2.DistanceTo(point1);

        // Assert
        Assert.That(distance2, 5, Is.EqualTo(distance1));
    }
}
