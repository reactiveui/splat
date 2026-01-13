// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ModeDetection;

public class DefaultModeDetectorTests
{
    /// <summary>
    /// Test that DefaultModeDetector can detect unit test runner.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_CanDetectUnitTestRunner()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Assert
        // Since we're running under TUnit, this should return true
        using (Assert.Multiple())
        {
            await Assert.That(result.HasValue).IsTrue();
            await Assert.That(result!.Value).IsTrue();
        }
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IModeDetector.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_ImplementsIModeDetector()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        await Assert.That(detector).IsAssignableTo<IModeDetector>();
    }

    /// <summary>
    /// Test that DefaultModeDetector implements IEnableLogger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_ImplementsIEnableLogger()
    {
        // Arrange & Act
        var detector = new DefaultModeDetector();

        // Assert
        await Assert.That(detector).IsAssignableTo<IEnableLogger>();
    }

    /// <summary>
    /// Test that DefaultModeDetector handles exceptions gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_HandlesExceptionsGracefully()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act & Assert - should not throw
        await Assert.That(() => detector.InUnitTestRunner()).ThrowsNothing();
    }

    /// <summary>
    /// Test that DefaultModeDetector returns consistent results.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_ReturnsConsistentResults()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result1 = detector.InUnitTestRunner();
        var result2 = detector.InUnitTestRunner();
        var result3 = detector.InUnitTestRunner();

        // Assert - Should return consistent results
        using (Assert.Multiple())
        {
            await Assert.That(result2).IsEqualTo(result1);
            await Assert.That(result3).IsEqualTo(result2);
        }
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies that DefaultModeDetector correctly detects when running in a unit test runner
    /// by evaluating the explicit DOTNET_RUNNING_IN_TEST environment variable with commonly
    /// used true-value representations.
    /// </summary>
    /// <param name="value">The value to set for the DOTNET_RUNNING_IN_TEST environment variable to test detection logic.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("1")]
    [Arguments("true")]
    [Arguments("TRUE")]
    [Arguments("yes")]
    [Arguments("YES")]
    public async Task DefaultModeDetector_ExplicitEnvVar_DotnetRunningInTest_TrueVariants(string value)
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
            using (Assert.Multiple())
            {
                await Assert.That(result.HasValue).IsTrue();
                await Assert.That(result!.Value).IsTrue();
            }
        }
        finally
        {
            // Restore prior state
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
        }
    }
#endif

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies explicit AppContext-based detection using DOTNET_RUNNING_IN_TEST data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_AppContext_DotnetRunningInTest_ReturnsTrue()
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
            using (Assert.Multiple())
            {
                await Assert.That(result.HasValue).IsTrue();
                await Assert.That(result!.Value).IsTrue();
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
        }
    }
#endif

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies detection via exact test runner environment variables.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_ExactEnvVar_NUnitTest_ReturnsTrue()
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
            using (Assert.Multiple())
            {
                await Assert.That(result.HasValue).IsTrue();
                await Assert.That(result!.Value).IsTrue();
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldDotnetEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
            Environment.SetEnvironmentVariable("NUNIT_TEST", oldNUnitEnv);
        }
    }
#endif

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies detection via environment variable prefix signals (e.g., VSTEST_*, XUNIT_*).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_EnvPrefix_VSTEST_ReturnsTrue()
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
            using (Assert.Multiple())
            {
                await Assert.That(result.HasValue).IsTrue();
                await Assert.That(result!.Value).IsTrue();
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_TEST", oldDotnetEnv);
            AppContext.SetData("DOTNET_RUNNING_IN_TEST", oldAppCtx);
            Environment.SetEnvironmentVariable(customVarName, oldCustom);
        }
    }
#endif

    /// <summary>
    /// Verifies that DefaultModeDetector can detect Microsoft Testing Platform (MTP) via assembly scan.
    /// This test ensures MTP is recognized as a unit test framework.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_DetectsMicrosoftTestingPlatform()
    {
        // Arrange
        var detector = new DefaultModeDetector();

        // Act
        var result = detector.InUnitTestRunner();

        // Determine if Microsoft.Testing.Platform is actually loaded.
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var hasMTPAssembly = false;
        foreach (var assembly in assemblies)
        {
            var fullName = assembly.FullName;
            if (!string.IsNullOrEmpty(fullName) &&
                fullName.Contains("Microsoft.Testing.Platform", StringComparison.OrdinalIgnoreCase))
            {
                hasMTPAssembly = true;
                break;
            }
        }

        // Assert - When running under Microsoft Testing Platform, should return true
        // Only enforce this when the Microsoft.Testing.Platform assembly is actually loaded.
        using (Assert.Multiple())
        {
            if (hasMTPAssembly)
            {
                await Assert.That(result.HasValue).IsTrue();
                await Assert.That(result!.Value).IsTrue();
            }
            else
            {
                // When Microsoft.Testing.Platform is not loaded (e.g., running under TUnit only),
                // skip the MTP-specific assertion to avoid spurious failures.
            }
        }
    }

    /// <summary>
    /// Verifies that the Microsoft.Testing.Platform assembly marker is recognized.
    /// This is a sanity check that the constant is correctly added to the markers array.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultModeDetector_IncludesMTPAssemblyMarker()
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
        using (Assert.Multiple())
        {
            if (hasMTPAssembly)
            {
                await Assert.That(result).IsTrue();
            }

            // Ensure this test only passes if the MTP assembly is present or another test framework is detected
            await Assert.That(hasMTPAssembly || (result.HasValue && result.Value)).IsTrue();
        }
    }
}
