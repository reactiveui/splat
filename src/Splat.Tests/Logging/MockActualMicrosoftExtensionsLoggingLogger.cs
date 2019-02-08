using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Mock Logger for Testing Microsoft.Extensions.Logging.
    /// </summary>
    public sealed class MockActualMicrosoftExtensionsLoggingLogger : global::Microsoft.Extensions.Logging.ILogger
    {
        private readonly List<string> _memoryTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockActualMicrosoftExtensionsLoggingLogger"/> class.
        /// </summary>
        /// <param name="memoryTarget">Memory target used for checking results.</param>
        public MockActualMicrosoftExtensionsLoggingLogger(List<string> memoryTarget)
        {
            _memoryTarget = memoryTarget;
        }

        /// <inheritdoc/>
        public void Log<TState>(
            global::Microsoft.Extensions.Logging.LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            _memoryTarget.Add(formatter(state, exception));
        }

        /// <inheritdoc/>
        public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
