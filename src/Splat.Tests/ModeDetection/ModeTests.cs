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
}
