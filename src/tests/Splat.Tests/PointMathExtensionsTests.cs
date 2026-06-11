// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat.Tests;

/// <summary>Tests for the point math extension methods.</summary>
public class PointMathExtensionsTests
{
    /// <summary>The epsilon tolerance used for floating-point comparisons.</summary>
    private const float Eps = 1e-5f;

    /// <summary>The X component of the classic 3-4-5 vector.</summary>
    private const float VecX = 3.0f;

    /// <summary>The Y component of the classic 3-4-5 vector.</summary>
    private const float VecY = 4.0f;

    /// <summary>The magnitude (hypotenuse) of the 3-4-5 vector.</summary>
    private const float VecLength = 5.0f;

    /// <summary>Test that Floor method correctly floors point values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Floor_CorrectlyFloorsPoint()
    {
        var point = new Point((int)VecX, (int)VecY);

        var result = point.Floor();

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(VecX);
            await Assert.That(result.Y).IsEqualTo(VecY);
        }
    }

    /// <summary>Test that Floor method handles negative values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Floor_HandlesNegativeValues()
    {
        var point = new Point((int)-VecX, (int)-VecY);

        var result = point.Floor();

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(-VecX);
            await Assert.That(result.Y).IsEqualTo(-VecY);
        }
    }

    /// <summary>Test that WithinEpsilonOf returns true when points are within epsilon.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsTrue_WhenPointsAreWithinEpsilon()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(1.1f, 2.1f);
        const float epsilon = 0.2f;

        var result = point1.WithinEpsilonOf(point2, epsilon);

        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that WithinEpsilonOf returns false when points are not within epsilon.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_ReturnsFalse_WhenPointsAreNotWithinEpsilon()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(5.0f, 6.0f);
        const float epsilon = 0.5f;

        var result = point1.WithinEpsilonOf(point2, epsilon);

        await Assert.That(result).IsFalse();
    }

    /// <summary>Test that WithinEpsilonOf handles identical points.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithinEpsilonOf_HandlesIdenticalPoints()
    {
        var point = new PointF(1.0f, 2.0f);
        const float epsilon = 0.1f;

        var result = point.WithinEpsilonOf(point, epsilon);

        await Assert.That(result).IsTrue();
    }

    /// <summary>Test that DotProduct calculates correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DotProduct_CalculatesCorrectly()
    {
        var point1 = new PointF(VecX, VecY);
        const float otherX = 2.0f;
        const float otherY = 1.0f;
        var point2 = new PointF(otherX, otherY);

        var result = point1.DotProduct(point2);

        await Assert.That(result).IsEqualTo((VecX * otherX) + (VecY * otherY)).Within(Eps);
    }

    /// <summary>Test that DotProduct handles zero vectors.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DotProduct_HandlesZeroVectors()
    {
        var point1 = new PointF(3.0f, 4.0f);
        var point2 = new PointF(0.0f, 0.0f);

        var result = point1.DotProduct(point2);

        await Assert.That(result).IsEqualTo(0);
    }

    /// <summary>Test that ScaledBy scales point correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_ScalesPointCorrectly()
    {
        const float pointX = 2.0f;
        const float pointY = 3.0f;
        var point = new PointF(pointX, pointY);
        const float factor = 2.5f;

        var result = point.ScaledBy(factor);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(pointX * factor).Within(Eps);
            await Assert.That(result.Y).IsEqualTo(pointY * factor).Within(Eps);
        }
    }

    /// <summary>Test that ScaledBy handles zero factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesZeroFactor()
    {
        var point = new PointF(2.0f, 3.0f);
        const float factor = 0.0f;

        var result = point.ScaledBy(factor);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(0);
            await Assert.That(result.Y).IsEqualTo(0);
        }
    }

    /// <summary>Test that ScaledBy handles negative factor.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ScaledBy_HandlesNegativeFactor()
    {
        const float pointX = 2.0f;
        const float pointY = 3.0f;
        var point = new PointF(pointX, pointY);
        const float factor = -2.0f;

        var result = point.ScaledBy(factor);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(pointX * factor);
            await Assert.That(result.Y).IsEqualTo(pointY * factor);
        }
    }

    /// <summary>Test that Length calculates magnitude correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Length_CalculatesMagnitudeCorrectly()
    {
        var point = new PointF(VecX, VecY);

        var result = point.Length();

        await Assert.That(result).IsEqualTo(VecLength).Within(Eps);
    }

    /// <summary>Test that Length handles zero vector.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Length_HandlesZeroVector()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.Length();

        await Assert.That(result).IsEqualTo(0);
    }

    /// <summary>Test that Normalize creates unit vector correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Normalize_CreatesUnitVectorCorrectly()
    {
        var point = new PointF(VecX, VecY);

        var result = point.Normalize();

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(VecX / VecLength).Within(Eps);
            await Assert.That(result.Y).IsEqualTo(VecY / VecLength).Within(Eps);
            await Assert.That(result.Length()).IsEqualTo(1.0f).Within(Eps);
        }
    }

    /// <summary>Test that Normalize handles zero vector correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Normalize_HandlesZeroVectorCorrectly()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.Normalize();

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(0);
            await Assert.That(result.Y).IsEqualTo(0);
        }
    }

    /// <summary>Test that AngleInDegrees calculates angle correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AngleInDegrees_CalculatesAngleCorrectly()
    {
        var point = new PointF(1.0f, 1.0f);
        const float expectedAngle = 45.0f;
        const float angleTolerance = 1.0f;

        var result = point.AngleInDegrees();

        await Assert.That(result).IsEqualTo(expectedAngle).Within(angleTolerance);
    }

    /// <summary>Test that AngleInDegrees handles negative coordinates.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AngleInDegrees_HandlesNegativeCoordinates()
    {
        var point = new PointF(-1.0f, 1.0f);
        const float expectedAngle = 135.0f;
        const float angleTolerance = 1.0f;

        var result = point.AngleInDegrees();

        await Assert.That(result).IsEqualTo(expectedAngle).Within(angleTolerance);
    }

    /// <summary>Test that AngleInDegrees handles zero vector.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AngleInDegrees_HandlesZeroVector()
    {
        var point = new PointF(0.0f, 0.0f);

        var result = point.AngleInDegrees();

        await Assert.That(result).IsEqualTo(0);
    }

    /// <summary>Test that ProjectAlong projects correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ProjectAlong_ProjectsCorrectly()
    {
        var point = new PointF(VecX, VecY);
        var direction = new PointF(1.0f, 0.0f); // Unit vector along X-axis

        var result = point.ProjectAlong(direction);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(VecX).Within(Eps);
            await Assert.That(result.Y).IsEqualTo(0f).Within(Eps);
        }
    }

    /// <summary>Test that ProjectAlong handles zero direction vector.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ProjectAlong_HandlesZeroDirection()
    {
        var point = new PointF(3.0f, 4.0f);
        var direction = new PointF(0.0f, 0.0f);

        var result = point.ProjectAlong(direction);

        // When direction is zero, normalized direction is also zero, result should be zero
        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(0);
            await Assert.That(result.Y).IsEqualTo(0);
        }
    }

    /// <summary>Test that ProjectAlongAngle projects correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ProjectAlongAngle_ProjectsCorrectly()
    {
        var point = new PointF(VecX, VecY);
        const float angle = 0.0f; // Along X-axis

        var result = point.ProjectAlongAngle(angle);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(VecX).Within(Eps);
            await Assert.That(result.Y).IsEqualTo(0f).Within(Eps);
        }
    }

    /// <summary>Test that ProjectAlongAngle works with different angles.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ProjectAlongAngle_WorksWithDifferentAngles()
    {
        var point = new PointF(VecX, VecY);
        const float angle = 90.0f; // Along Y-axis

        var result = point.ProjectAlongAngle(angle);

        using (Assert.Multiple())
        {
            await Assert.That(result.X).IsEqualTo(0f).Within(Eps);
            await Assert.That(result.Y).IsEqualTo(VecY).Within(Eps);
        }
    }

    /// <summary>Test that DistanceTo calculates distance correctly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DistanceTo_CalculatesDistanceCorrectly()
    {
        var point1 = new PointF(0.0f, 0.0f);
        var point2 = new PointF(VecX, VecY);

        var result = point1.DistanceTo(point2);

        await Assert.That(result).IsEqualTo(VecLength).Within(Eps);
    }

    /// <summary>Test that DistanceTo handles same points.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DistanceTo_HandlesSamePoints()
    {
        var point = new PointF(1.0f, 2.0f);

        var result = point.DistanceTo(point);

        await Assert.That(result).IsEqualTo(0);
    }

    /// <summary>Test that DistanceTo is symmetric.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DistanceTo_IsSymmetric()
    {
        var point1 = new PointF(1.0f, 2.0f);
        var point2 = new PointF(4.0f, 6.0f);

        var distance1 = point1.DistanceTo(point2);
        var distance2 = point2.DistanceTo(point1);

        await Assert.That(distance1).IsEqualTo(distance2).Within(Eps);
    }
}
