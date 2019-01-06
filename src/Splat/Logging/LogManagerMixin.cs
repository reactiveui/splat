using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Extension methods associated with the logging module.
    /// </summary>
    public static class LogManagerMixin
    {
        /// <summary>
        /// Gets a <see cref="IFullLogger"/> for the specified <see cref="ILogManager"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ILogManager"/> to use.</typeparam>
        /// <param name="logManager">The log manager to get the logger from.</param>
        /// <returns>A logger for the specified type.</returns>
        public static IFullLogger GetLogger<T>(this ILogManager logManager)
        {
            return logManager.GetLogger(typeof(T));
        }
    }
}
