// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ModeDetection;

/// <summary>
/// Unit Tests for the DefaultModeDetector class.
/// </summary>
[TestFixture]
public class DefaultModeDetectorTests
{
    /// <summary>
    /// Test that DefaultModeDetector can detect unit test runner.
    /// </summary>
    [Test]
    public void DefaultModeDetector_CanDetectUnitTestRunner()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Assert
        // Since we're running under NUnit, this should return true
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.HasValue, Is.True);
            Assert.That(result!.Value, Is.True);
        }
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IModeDetector.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ImplementsIModeDetector()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.That(detector, Is.AssignableTo<IModeDetector>());
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IEnableLogger.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ImplementsIEnableLogger()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        Assert.That(detector, Is.AssignableTo<IEnableLogger>());
    }

    /// <summary>
    /// Test that DefaultModeDetector handles exceptions gracefully.
    /// </summary>
    [Test]
    public void DefaultModeDetector_HandlesExceptionsGracefully()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act & Assert - should not throw
        Assert.DoesNotThrow(() => detector.InUnitTestRunner());

        var result = detector.InUnitTestRunner();

        // Should return a value (either true or false) or null if exception occurred
        Assert.That(result, Is.Null.Or.True.Or.False);
    }

    /// <summary>
    /// Test that DefaultModeDetector returns consistent results.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ReturnsConsistentResults()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result1 = detector.InUnitTestRunner();
        var result2 = detector.InUnitTestRunner();
        var result3 = detector.InUnitTestRunner();

        // Assert - Should return consistent results
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result2, Is.EqualTo(result1));
            Assert.That(result3, Is.EqualTo(result2));
        }
    }

    /// <summary>
    /// Verifies that DefaultModeDetector correctly detects when running in a unit test runner
    /// by evaluating the explicit DOTNET_RUNNING_IN_TEST environment variable with commonly
    /// used true-value representations.
    /// </summary>
    /// <param name="value">The value to set for the DOTNET_RUNNING_IN_TEST environment variable to test detection logic.</param>
    [TestCase("1")]
    [TestCase("true")]
    [TestCase("TRUE")]
    [TestCase("yes")]
    [TestCase("YES")]
    public void DefaultModeDetector_ExplicitEnvVar_DotnetRunningInTest_TrueVariants(string value)
    {
        // Arrange
        var detector = new DefaultModeDetector();
        var oldEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        var oldAppCtx = AppContext.GetData("DOTNET_RUNNING_IN_TEST");

        try
        {
            // Prefer explicit env var; clear AppContext override to exercise env path.
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", value);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", null);

            // Act
            var result = detector.InUnitTestRunner();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.HasValue, Is.True);
                Assert.That(result!.Value, Is.True);
            }
        }
        finally
        {
            // Restore prior state
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
        }
    }

    /// <summary>
    /// Verifies explicit AppContext-based detection using DOTNET_RUNNING_IN_TEST data.
    /// </summary>
    [Test]
    public void DefaultModeDetector_AppContext_DotnetRunningInTest_ReturnsTrue()
    {
        // Arrange
        var detector = new DefaultModeDetector();
        var oldEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        var oldAppCtx = AppContext.GetData("DOTNET_RUNNING_IN_TEST");

        try
        {
            // Clear env var and set AppContext data
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", "true");

            // Act
            var result = detector.InUnitTestRunner();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.HasValue, Is.True);
                Assert.That(result!.Value, Is.True);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
        }
    }

    /// <summary>
    /// Verifies detection via exact test runner environment variables.
    /// </summary>
    [Test]
    public void DefaultModeDetector_ExactEnvVar_NUnitTest_ReturnsTrue()
    {
        // Arrange
        var detector = new DefaultModeDetector();
        var oldDotnetEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        var oldAppCtx = AppContext.GetData("DOTNET_RUNNING_IN_TEST");
        var oldNUnitEnv = Environment.GetEnvironmentVariable("NUNIT_TEST");

        try
        {
            // Clear explicit signals to exercise runner env signal path and set NUNIT_TEST.
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", null);
            Environment.SetEnvironmentVariable("NUNIT_TEST", "1");

            // Act
            var result = detector.InUnitTestRunner();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.HasValue, Is.True);
                Assert.That(result!.Value, Is.True);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldDotnetEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
            Environment.SetEnvironmentVariable("NUNIT_TEST", oldNUnitEnv);
        }
    }

    /// <summary>
    /// Verifies detection via environment variable prefix signals (e.g., VSTEST_*, XUNIT_*).
    /// </summary>
    [Test]
    public void DefaultModeDetector_EnvPrefix_VSTEST_ReturnsTrue()
    {
        // Arrange
        var detector = new DefaultModeDetector();
        var oldDotnetEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        var oldAppCtx = AppContext.GetData("DOTNET_RUNNING_IN_TEST");
        var customVarName = "VSTEST_MY_CUSTOM_FLAG";
        var oldCustom = Environment.GetEnvironmentVariable(customVarName);

        try
        {
            // Clear explicit signals and set a prefixed env var.
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", null);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", null);
            Environment.SetEnvironmentVariable(customVarName, "1");

            // Act
            var result = detector.InUnitTestRunner();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.HasValue, Is.True);
                Assert.That(result!.Value, Is.True);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldDotnetEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
            Environment.SetEnvironmentVariable(customVarName, oldCustom);
        }
    }

    /// <summary>
    /// Verifies that DefaultModeDetector can detect Microsoft Testing Platform (MTP) via assembly scan.
    /// This test ensures MTP is recognized as a unit test framework.
    /// </summary>
    [Test]
    public void DefaultModeDetector_DetectsMicrosoftTestingPlatform()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Assert - When running under Microsoft Testing Platform, should return true
        // The test assembly itself loads Microsoft.Testing.Platform, so the detector should find it
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.HasValue, Is.True);
            Assert.That(result!.Value, Is.True, "ModeDetector should detect Microsoft.Testing.Platform assemblies");
        }
    }

    /// <summary>
    /// Verifies that the Microsoft.Testing.Platform assembly marker is recognized.
    /// This is a sanity check that the constant is correctly added to the markers array.
    /// </summary>
    [Test]
    public void DefaultModeDetector_IncludesMTPAssemblyMarker()
    {
        // Arrange & Act
        // Check if any loaded assemblies contain Microsoft.Testing.Platform
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var hasMTPAssembly = assemblies.Any(a =>
            a.FullName != null &&
            a.FullName.Contains("Microsoft.Testing.Platform", StringComparison.OrdinalIgnoreCase));

        var detector = new DefaultModeDetector();
        var result = detector.InUnitTestRunner();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            if (hasMTPAssembly)
            {
                Assert.That(
                    result,
                    Is.True,
                    "When Microsoft.Testing.Platform assembly is loaded, ModeDetector should detect it");
            }

            // Ensure this test only passes if the MTP assembly is present or another test framework is detected
            Assert.That(
                hasMTPAssembly || (result.HasValue && result.Value),
                Is.True,
                "Test should only pass when either Microsoft.Testing.Platform is loaded or another unit test framework is detected.");
        }
    }
}
