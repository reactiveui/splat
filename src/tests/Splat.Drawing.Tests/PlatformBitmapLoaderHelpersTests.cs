// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
#if ANDROID
namespace Splat.Tests.Drawing;

/// <summary>
/// Tests for Android bitmap loader helpers.
/// </summary>
public class PlatformBitmapLoaderHelpersTests
{
    /// <summary>
    /// Checks that two-byte image terminators are accepted.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasCorrectStreamEnd_ReturnsTrue_ForTwoByteTerminator()
    {
        using var stream = new MemoryStream([0xFF, 0xD9]);

        await Assert.That(PlatformBitmapLoaderHelpers.HasCorrectStreamEnd(stream)).IsTrue();
    }

    /// <summary>
    /// Checks that three-byte streams inspect the last two bytes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasCorrectStreamEnd_ReturnsTrue_WhenLastTwoBytesAreTerminator()
    {
        using var stream = new MemoryStream([0x00, 0xFF, 0xD9]);

        await Assert.That(PlatformBitmapLoaderHelpers.HasCorrectStreamEnd(stream)).IsTrue();
    }
}
#endif
