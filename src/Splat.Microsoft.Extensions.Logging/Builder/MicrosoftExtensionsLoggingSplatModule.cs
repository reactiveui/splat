// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

using Splat.Microsoft.Extensions.Logging;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Microsoft Extensions Logging logger.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MicrosoftExtensionsLoggingSplatModule" /> class.
/// </remarks>
/// <seealso cref="IModule" />
public sealed class MicrosoftExtensionsLoggingSplatModule(ILoggerFactory loggerFactory) : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(loggerFactory);
}
