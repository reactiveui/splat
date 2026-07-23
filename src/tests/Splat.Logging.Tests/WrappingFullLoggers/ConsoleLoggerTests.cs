// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="ConsoleLoggerTests"/> class.</summary>
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

    /// <summary>A <see cref="TextWriter"/> that captures console log output for assertions.</summary>
    private sealed class ConsoleWriter : TextWriter, IMockLogTarget
    {
        /// <summary>The captured log entries with their log level and message.</summary>
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        /// <inheritdoc/>
        public override Encoding Encoding => Encoding.UTF8;

        /// <inheritdoc/>
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
            catch (Exception ex)
            {
                // Ignore improperly formatted lines
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
