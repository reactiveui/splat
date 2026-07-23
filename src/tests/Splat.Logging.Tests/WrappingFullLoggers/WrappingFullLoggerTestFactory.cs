// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Builds a <see cref="WrappingFullLogger"/> over a <see cref="TextLogger"/> capture target so tests can assert the
/// exact level and message the wrapping logger forwards to its inner logger.
/// </summary>
internal static class WrappingFullLoggerTestFactory
{
    /// <summary>A synthetic level above <see cref="LogLevel.Fatal"/> used to disable every level, including Fatal.</summary>
    internal const LogLevel AboveFatal = LogLevel.Fatal + 1;

    /// <summary>Creates a <see cref="WrappingFullLogger"/> whose inner logger captures forwarded entries.</summary>
    /// <param name="level">The minimum level configured on the capture target.</param>
    /// <returns>The wrapping logger and the capture target it forwards to.</returns>
    internal static (WrappingFullLogger Logger, TextLogger Target) CreateLogger(LogLevel level)
    {
        var target = new TextLogger { Level = level };
        return (new WrappingFullLogger(target), target);
    }
}
