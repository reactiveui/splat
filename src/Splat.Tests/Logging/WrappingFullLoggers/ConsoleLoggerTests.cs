// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that verify the <see cref="ConsoleLoggerTests"/> class.
/// </summary>
internal class ConsoleLoggerTests : FullLoggerTestBase
{
    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var outputWriter = new ConsoleWriter();
        Console.SetOut(outputWriter);
        return (new WrappingFullLogger(new WrappingLogLevelLogger(new ConsoleLogger { Level = minimumLogLevel, ExceptionMessageFormat = "{0} {1}" })), outputWriter);
    }

    private sealed class ConsoleWriter : TextWriter, IMockLogTarget
    {
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        public override Encoding Encoding => Encoding.UTF8;

        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        public override void WriteLine(string? value)
        {
            var colonIndex = value!.IndexOf(':', StringComparison.InvariantCulture);
            var level = Enum.Parse<LogLevel>(value.AsSpan(0, colonIndex));
            var message = value.Substring(colonIndex + 1).Trim();
            _logs.Add((level, message));
            base.WriteLine(value);
        }
    }
}
