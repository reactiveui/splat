using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Splat.Tests.Mocks
{
    /// <summary>
    /// A <see cref="TextWriter"/> implementation of <see cref="ILogger"/> for testing.
    /// </summary>
    /// <seealso cref="Splat.ILogger" />
    public class TextLogger : ILogger, IDisposable
    {
        private readonly StringBuilder _stringBuilder;
        private TextWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLogger"/> class.
        /// </summary>
        public TextLogger()
        {
            _stringBuilder = new StringBuilder();

            _writer = new StringWriter(_stringBuilder);
        }

        /// <summary>
        /// Gets the value of the text writer.
        /// </summary>
        public string Value => _stringBuilder.ToString();

        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            _writer.WriteLine(message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            _writer.WriteLine($"{type.Name}: {message}");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }
            }
        }
    }
}
