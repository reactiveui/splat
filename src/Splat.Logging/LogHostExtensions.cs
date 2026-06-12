// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Extension methods for <see cref="LogHost"/>.</summary>
public static class LogHostExtensions
{
    /// <summary>Call this method to write log entries on behalf of the current class.</summary>
    /// <param name="logClassInstance">The class we are getting the logger for.</param>
    /// <typeparam name="T">The type to get the <see cref="IFullLogger"/> for.</typeparam>
    extension<T>(T logClassInstance)
        where T : IEnableLogger
    {
        /// <summary>Call this method to write log entries on behalf of the current class.</summary>
        /// <returns>The <see cref="IFullLogger"/> for the class type.</returns>
        public IFullLogger Log()
        {
            var factory = AppLocator.Current.GetService<ILogManager>();
            return factory switch
            {
                null => throw new InvalidOperationException("ILogManager is null. This should never happen, your dependency resolver is broken"),
                _ => factory.GetLogger<T>()
            };
        }
    }
}
