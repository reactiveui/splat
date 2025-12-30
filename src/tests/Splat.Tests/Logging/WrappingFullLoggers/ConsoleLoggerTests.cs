// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that verify the <see cref="ConsoleLoggerTests"/> class.
/// </summary>
[InheritsTests]
[NotInParallel]
internal sealed class ConsoleLoggerTests : FullLoggerTestBase
{
    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var outputWriter = new ConsoleWriter();
        return (new WrappingFullLogger(new WrappingLogLevelLogger(new ConsoleLogger(outputWriter) { Level = minimumLogLevel, ExceptionMessageFormat = "{0} {1}" })), outputWriter);
    }

    private sealed class ConsoleWriter : TextWriter, IMockLogTarget
    {
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        public override Encoding Encoding => Encoding.UTF8;

        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        public override void WriteLine(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            try
            {
#if NET8_0_OR_GREATER
                var colonIndex = value.IndexOf(':', StringComparison.InvariantCulture);
                if (colonIndex == -1)
                {
                    // No colon found - not a properly formatted log message
                    return;
                }

                var level = Enum.Parse<LogLevel>(value.AsSpan(0, colonIndex));
#else
                var colonIndex = value.IndexOf(':');
                if (colonIndex == -1)
                {
                    // No colon found - not a properly formatted log message
                    return;
                }

                var level = (LogLevel)Enum.Parse(typeof(LogLevel), value.Substring(0, colonIndex));
#endif
                var message = value.Substring(colonIndex + 1).Trim();
                _logs.Add((level, message));
            }
            catch
            {
                // Ignore improperly formatted lines
            }
        }
    }
}
