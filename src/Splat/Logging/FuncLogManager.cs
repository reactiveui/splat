using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A log manager which will generate the <see cref="IFullLogger"/> by using the specified Func.
    /// </summary>
    public class FuncLogManager : ILogManager
    {
        private readonly Func<Type, IFullLogger> _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncLogManager"/> class.
        /// </summary>
        /// <param name="getLoggerFunc">The function which will be used to generate the <see cref="IFullLogger"/>.</param>
        public FuncLogManager(Func<Type, IFullLogger> getLoggerFunc)
        {
            _inner = getLoggerFunc;
        }

        /// <inheritdoc />
        public IFullLogger GetLogger(Type type)
        {
            return _inner(type);
        }
    }
}
