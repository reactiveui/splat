using System;
using Splat;

namespace UnitTests
{
    public partial class LoggingTests
    {
        public class LoggerFixture : ILogger
        {
            private readonly Action<string, LogLevel> _onWrite;

            public LoggerFixture(Action<string, LogLevel> onWrite)
            {
                _onWrite = onWrite;
            }
            public void Write(string message, LogLevel logLevel)
            {
                _onWrite.Invoke(message, logLevel);
            }

            public LogLevel Level { get; set; }
        }
    }
}