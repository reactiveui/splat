using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    internal static class AssemblyFinder
    {
        /// <summary>
        /// Attempt to find the type based on the specified string.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to if we find it.</typeparam>
        /// <param name="fullTypeName">The name of the full type.</param>
        /// <returns>The created object or the default value.</returns>
        [SuppressMessage("Globalization", "CA1307: Use IFormatProvider", Justification = "string.Replace does not have a IFormatProvider on all .NET platforms")]
        public static T AttemptToLoadType<T>(string fullTypeName)
        {
            var thisType = typeof(AssemblyFinder);

            var toSearch = new[]
            {
                thisType.AssemblyQualifiedName.Replace(thisType.FullName + ", ", string.Empty),
                thisType.AssemblyQualifiedName.Replace(thisType.FullName + ", ", string.Empty).Replace(".Portable", string.Empty),
            }.Select(x => new AssemblyName(x)).ToArray();

            foreach (var assembly in toSearch)
            {
                var fullName = fullTypeName + ", " + assembly.FullName;
                var type = Type.GetType(fullName, false);
                if (type == null)
                {
                    continue;
                }

                return (T)Activator.CreateInstance(type);
            }

            return default(T);
        }
    }
}
