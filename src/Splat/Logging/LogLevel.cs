using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Represents the minimum log level a <see cref="ILogger"/> will start emitting from.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The log message is for debuging purposes.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// The log message is for information purposes.
        /// </summary>
        Info,

        /// <summary>
        /// The log message is for warning purposes.
        /// </summary>
        Warn,

        /// <summary>
        /// The log message is for error purposes.
        /// </summary>
        Error,

        /// <summary>
        /// The log message is for fatal purposes.
        /// </summary>
        Fatal,
    }
}
