using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A manaager which will generate a <see cref="IFullLogger"/> for the specified type.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Generate a <see cref="IFullLogger"/> for the specified type.
        /// </summary>
        /// <param name="type">The type to generate the logger for.</param>
        /// <returns>The <see cref="IFullLogger"/> for the specified type.</returns>
        IFullLogger GetLogger(Type type);
    }
}
