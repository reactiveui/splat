// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>Base class for a logger the provides allocation free logging.</summary>
/// <seealso cref="IAllocationFreeLogger" />
/// <remarks>
/// Initializes a new instance of the <see cref="AllocationFreeLoggerBase"/> class.
/// </remarks>
/// <param name="inner">The <see cref="ILogger" /> to wrap in this class.</param>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Existing API")]
[SuppressMessage("Design", "SST1496:An abstract type declares nothing abstract", Justification = "Deliberate shared base class providing implementation, not a contract.")]
public abstract partial class AllocationFreeLoggerBase(ILogger inner) : IAllocationFreeLogger
{
    /// <inheritdoc />
    public LogLevel Level => inner.Level;

    /// <inheritdoc />
    public bool IsDebugEnabled => Level <= LogLevel.Debug;

    /// <inheritdoc />
    public bool IsInfoEnabled => Level <= LogLevel.Info;

    /// <inheritdoc />
    public bool IsWarnEnabled => Level <= LogLevel.Warn;

    /// <inheritdoc />
    public bool IsErrorEnabled => Level <= LogLevel.Error;

    /// <inheritdoc />
    public bool IsFatalEnabled => Level <= LogLevel.Fatal;

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => inner.Write(message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => inner.Write(exception, message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write(message, type, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write(exception, message, type, logLevel);
}
