using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Contains helper methods to get access to the Default <see cref="IFullLogger"/>.
    /// </summary>
    public static class LogHost
    {
        /// <summary>
        /// Gets the default <see cref="IFullLogger"/> registered within the <see cref="Locator"/>.
        /// </summary>
        [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
        public static IFullLogger Default
        {
            get
            {
                var factory = Locator.Current.GetService<ILogManager>();
                if (factory == null)
                {
                    throw new LoggingException("ILogManager is null. This should never happen, your dependency resolver is broken");
                }

                return factory.GetLogger(typeof(LogHost));
            }
        }

        /// <summary>
        /// Call this method to write log entries on behalf of the current class.
        /// </summary>
        /// <typeparam name="T">The type to get the <see cref="IFullLogger"/> for.</typeparam>
        /// <param name="logClassInstance">The class we are getting the logger for.</param>
        /// <returns>The <see cref="IFullLogger"/> for the class type.</returns>
        public static IFullLogger Log<T>(this T logClassInstance)
            where T : IEnableLogger
        {
            var factory = Locator.Current.GetService<ILogManager>();
            if (factory == null)
            {
                throw new Exception("ILogManager is null. This should never happen, your dependency resolver is broken");
            }

            return factory.GetLogger<T>();
        }
    }
}
