using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Represents a log target where messages can be written to.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the level at which the target will emit messages.
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        /// Writes a message to the target.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="logLevel">The severity level of the log message.</param>
        void Write([Localizable(false)] string message, LogLevel logLevel);
    }
}
