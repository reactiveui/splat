// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// An allocation free exception logger which wraps all the possible logging methods available.
/// Often not needed for your own loggers.
/// A <see cref="WrappingFullLogger"/> will wrap simple loggers into a full logger.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Existing API")]
public partial interface IAllocationFreeErrorLogger : ILogger;
