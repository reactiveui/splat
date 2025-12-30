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
    private const float Eps = 1e-5f;

    /// <summary>
    /// Test that Floor method correctly floors point values.
    /// </summary>
    [Test]
    public void Floor_CorrectlyFloorsPoint()
    {
        var point = new Point(3, 4);

        var result = point.Floor();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(3.0f));
            Assert.That(result.Y, Is.EqualTo(4.0f));
        }
    }

    /// <summary>
    /// Test that Floor method handles negative values.
    /// </summary>
    [Test]
    public void Floor_HandlesNegativeValues()
    {
        var point = new Point(-3, -4);

        var result = point.Floor();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(-3.0f));
            Assert.That(result.Y, Is.EqualTo(-4.0f));
        }
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns true when points are within epsilon.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_ReturnsTrue_WhenPointsAreWithinEpsilon()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(1.1f, 2.1f);
        const float epsilon = 0.2f;

        var result = point1.WithinEpsilonOf(point2, epsilon);

        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Test that WithinEpsilonOf returns false when points are not within epsilon.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_ReturnsFalse_WhenPointsAreNotWithinEpsilon()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(5.0f, 6.0f);
        const float epsilon = 0.5f;

        var result = point1.WithinEpsilonOf(point2, epsilon);

        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Test that WithinEpsilonOf handles identical points.
    /// </summary>
    [Test]
    public void WithinEpsilonOf_HandlesIdenticalPoints()
    {
        var point = new PointF(1.0f, 2.0f);
        const float epsilon = 0.1f;

        var result = point.WithinEpsilonOf(point, epsilon);

        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Test that DotProduct calculates correctly.
    /// </summary>
    [Test]
    public void DotProduct_CalculatesCorrectly()
    {
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(2.0f, 1.0f);

        var result = point1.DotProduct(point2);

        Assert.That(result, Is.EqualTo(10.0f).Within(Eps)); // (3*2) + (4*1) = 10
    }

    /// <summary>
    /// Test that DotProduct handles zero vectors.
    /// </summary>
    [Test]
    public void DotProduct_HandlesZeroVectors()
    {
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(0.0f, 0.0f);

        var result = point1.DotProduct(point2);

        Assert.That(result, Is.Zero);
    }

    /// <summary>
    /// Test that ScaledBy scales point correctly.
    /// </summary>
    [Test]
    public void ScaledBy_ScalesPointCorrectly()
    {
        var point = new PointF(2.0f, 3.0f);
        const float factor = 2.5f;

        var result = point.ScaledBy(factor);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(5.0f).Within(Eps));
            Assert.That(result.Y, Is.EqualTo(7.5f).Within(Eps));
        }
    }

    /// <summary>
    /// Test that ScaledBy handles zero factor.
    /// </summary>
    [Test]
    public void ScaledBy_HandlesZeroFactor()
    {
        var point = new PointF(2.0f, 3.0f);
        const float factor = 0.0f;

        var result = point.ScaledBy(factor);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.Zero);
            Assert.That(result.Y, Is.Zero);
        }
    }

    /// <summary>
    /// Test that ScaledBy handles negative factor.
    /// </summary>
    [Test]
    public void ScaledBy_HandlesNegativeFactor()
    {
        var point = new PointF(2.0f, 3.0f);
        const float factor = -2.0f;

        var result = point.ScaledBy(factor);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(-4.0f));
            Assert.That(result.Y, Is.EqualTo(-6.0f));
        }
    }

    /// <summary>
    /// Test that Length calculates magnitude correctly.
    /// </summary>
    [Test]
    public void Length_CalculatesMagnitudeCorrectly()
    {
        var point = new PointF(3.0f, 4.0f);

        var result = point.Length();

        Assert.That(result, Is.EqualTo(5.0f).Within(Eps));
    }

    /// <summary>
    /// Test that Length handles zero vector.
    /// </summary>
    [Test]
    public void Length_HandlesZeroVector()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.Length();

        Assert.That(result, Is.Zero);
    }

    /// <summary>
    /// Test that Normalize creates unit vector correctly.
    /// </summary>
    [Test]
    public void Normalize_CreatesUnitVectorCorrectly()
    {
        var point = new PointF(3.0f, 4.0f);

        var result = point.Normalize();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(0.6f).Within(Eps));
            Assert.That(result.Y, Is.EqualTo(0.8f).Within(Eps));
            Assert.That(result.Length(), Is.EqualTo(1.0f).Within(Eps));
        }
    }

    /// <summary>
    /// Test that Normalize handles zero vector correctly.
    /// </summary>
    [Test]
    public void Normalize_HandlesZeroVectorCorrectly()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.Normalize();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.Zero);
            Assert.That(result.Y, Is.Zero);
        }
    }

    /// <summary>
    /// Test that AngleInDegrees calculates angle correctly.
    /// </summary>
    [Test]
    public void AngleInDegrees_CalculatesAngleCorrectly()
    {
        var point = new PointF(1.0f, 1.0f);

        var result = point.AngleInDegrees();

        Assert.That(result, Is.EqualTo(45.0f).Within(1.0f));
    }

    /// <summary>
    /// Test that AngleInDegrees handles negative coordinates.
    /// </summary>
    [Test]
    public void AngleInDegrees_HandlesNegativeCoordinates()
    {
        var point = new PointF(-1.0f, 1.0f);

        var result = point.AngleInDegrees();

        Assert.That(result, Is.EqualTo(135.0f).Within(1.0f));
    }

    /// <summary>
    /// Test that AngleInDegrees handles zero vector.
    /// </summary>
    [Test]
    public void AngleInDegrees_HandlesZeroVector()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.AngleInDegrees();

        Assert.That(result, Is.Zero);
    }

    /// <summary>
    /// Test that ProjectAlong projects correctly.
    /// </summary>
    [Test]
    public void ProjectAlong_ProjectsCorrectly()
    {
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(1.0f, 0.0f); // Unit vector along X-axis

        var result = point.ProjectAlong(direction);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(3.0f).Within(Eps));
            Assert.That(result.Y, Is.Zero.Within(Eps));
        }
    }

    /// <summary>
    /// Test that ProjectAlong handles zero direction vector.
    /// </summary>
    [Test]
    public void ProjectAlong_HandlesZeroDirection()
    {
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(0.0f, 0.0f);

        var result = point.ProjectAlong(direction);

        // When direction is zero, normalized direction is also zero, result should be zero
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.Zero);
            Assert.That(result.Y, Is.Zero);
        }
    }

    /// <summary>
    /// Test that ProjectAlongAngle projects correctly.
    /// </summary>
    [Test]
    public void ProjectAlongAngle_ProjectsCorrectly()
    {
        var point = new PointF(3.0f, 4.0f);
        const float angle = 0.0f; // Along X-axis

        var result = point.ProjectAlongAngle(angle);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.EqualTo(3.0f).Within(Eps));
            Assert.That(result.Y, Is.Zero.Within(Eps));
        }
    }

    /// <summary>
    /// Test that ProjectAlongAngle works with different angles.
    /// </summary>
    [Test]
    public void ProjectAlongAngle_WorksWithDifferentAngles()
    {
        var point = new PointF(3.0f, 4.0f);
        const float angle = 90.0f; // Along Y-axis

        var result = point.ProjectAlongAngle(angle);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.X, Is.Zero.Within(Eps));
            Assert.That(result.Y, Is.EqualTo(4.0f).Within(Eps));
        }
    }

    /// <summary>
    /// Test that DistanceTo calculates distance correctly.
    /// </summary>
    [Test]
    public void DistanceTo_CalculatesDistanceCorrectly()
    {
        var point1 = new PointF(0.0f, 0.0f);
        var point2 = new PointF(3.0f, 4.0f);

        var result = point1.DistanceTo(point2);

        Assert.That(result, Is.EqualTo(5.0f).Within(Eps));
    }

    /// <summary>
    /// Test that DistanceTo handles same points.
    /// </summary>
    [Test]
    public void DistanceTo_HandlesSamePoints()
    {
        var point = new PointF(1.0f, 2.0f);

        var result = point.DistanceTo(point);

        Assert.That(result, Is.Zero);
    }

    /// <summary>
    /// Test that DistanceTo is symmetric.
    /// </summary>
    [Test]
    public void DistanceTo_IsSymmetric()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(4.0f, 6.0f);

        var distance1 = point1.DistanceTo(point2);
        var distance2 = point2.DistanceTo(point1);

        Assert.That(distance1, Is.EqualTo(distance2).Within(Eps));
    }
}
