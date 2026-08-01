// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// An allocation free logger which wraps all the possible logging methods available.
/// Often not needed for your own loggers.
/// A <see cref="WrappingFullLogger"/> will wrap simple loggers into a full logger.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Existing API")]
public partial interface IAllocationFreeLogger : IAllocationFreeErrorLogger
{
    /// <summary>Gets a value indicating whether the logger currently emits debug logs.</summary>
    bool IsDebugEnabled { get; }

    /// <summary>Gets a value indicating whether the logger currently emits information logs.</summary>
    bool IsInfoEnabled { get; }

    /// <summary>Gets a value indicating whether the logger currently emits warning logs.</summary>
    bool IsWarnEnabled { get; }

    /// <summary>Gets a value indicating whether the logger currently emits error logs.</summary>
    bool IsErrorEnabled { get; }

    /// <summary>Gets a value indicating whether the logger currently emits fatal logs.</summary>
    bool IsFatalEnabled { get; }
}
