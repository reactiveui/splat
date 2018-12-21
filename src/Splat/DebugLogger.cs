// Taken from https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Debug

// We need to define the DEBUG symbol because we want the logger
// to work even when this package is compiled on release. Otherwise,
// the call to Debug.WriteLine will not be in the release binary
#define DEBUG

namespace Splat
{
    public class DebugLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level) return;
            System.Diagnostics.Debug.WriteLine(message);
        }

        public LogLevel Level { get; set; }
    }
}
