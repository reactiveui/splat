using Splat.ModeDetection;

namespace Splat.Tests.ModeDetection;

/// <summary>
/// Unit tests for the <see cref="Mode"/> class.
/// </summary>
public class ModeTests
{
    /// <summary>
    /// Tests the <see cref="Mode.Run"/> mode.
    /// </summary>
    [Fact]
    public void RunModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Run);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        Assert.False(inUnitTestRunner);
    }

    /// <summary>
    /// Tests the <see cref="Mode.Test"/> mode.
    /// </summary>
    [Fact]
    public void TestModeTest()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act
        var inUnitTestRunner = ModeDetector.InUnitTestRunner();

        // Assert
        Assert.True(inUnitTestRunner);
    }

    /// <summary>
    /// Tests that ModeDetector caches results properly.
    /// </summary>
    [Fact]
    public void ModeDetector_CachesResults()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);

        // Act - Call multiple times
        var result1 = ModeDetector.InUnitTestRunner();
        var result2 = ModeDetector.InUnitTestRunner();
        var result3 = ModeDetector.InUnitTestRunner();

        // Assert - Should all be the same (cached)
        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
    }

    /// <summary>
    /// Tests that overriding mode detector clears cache.
    /// </summary>
    [Fact]
    public void ModeDetector_OverrideClearsCache()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(Mode.Test);
        var result1 = ModeDetector.InUnitTestRunner();
        Assert.True(result1);

        // Act - Override with different mode
        ModeDetector.OverrideModeDetector(Mode.Run);
        var result2 = ModeDetector.InUnitTestRunner();

        // Assert - Should reflect new mode
        Assert.False(result2);
    }

    /// <summary>
    /// Tests that ModeDetector handles null detector gracefully.
    /// </summary>
    [Fact]
    public void ModeDetector_HandlesNullDetector()
    {
        // Arrange
        ModeDetector.OverrideModeDetector(null!);

        // Act
        var result = ModeDetector.InUnitTestRunner();

        // Assert - Should return false as fallback
        Assert.False(result);
    }
}
