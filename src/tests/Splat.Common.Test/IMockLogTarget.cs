// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Mocks;

/// <summary>A target with our desired logs.</summary>
public interface IMockLogTarget
{
    /// <summary>Gets the logs that have been sent.</summary>
    ICollection<(LogLevel logLevel, string message)> Logs { get; }
}
