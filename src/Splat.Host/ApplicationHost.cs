using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.HostBuilder.Splat.Host
{
    /// <summary>
    /// Provides convenience methods for <see cref="IApplicationHost"/> and <see cref="IApplicationHostBuilder"/>.
    /// </summary>
    public class ApplicationHost
    {
        /// <summary>
        /// Creates the default host builder.
        /// </summary>
        /// <returns>An application host builder.</returns>
        public static IApplicationHostBuilder CreateDefaultHostBuilder() => default;
    }
}
