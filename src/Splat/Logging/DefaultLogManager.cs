using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// The default log manager provided by splat.
    /// This log manager will cache the loggers for each type,
    /// This will use the default registered <see cref="ILogger"/> inside the <see cref="Locator"/>.
    /// </summary>
    public class DefaultLogManager : ILogManager
    {
        private static readonly IFullLogger _nullLogger = new WrappingFullLogger(new NullLogger(), typeof(MemoizingMRUCache<Type, IFullLogger>));
        private readonly MemoizingMRUCache<Type, IFullLogger> _loggerCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogManager"/> class.
        /// </summary>
        /// <param name="dependencyResolver">A dependency resolver for testing purposes, will use the default Locator if null.</param>
        public DefaultLogManager(IDependencyResolver dependencyResolver = null)
        {
            dependencyResolver = dependencyResolver ?? Locator.Current;

            _loggerCache = new MemoizingMRUCache<Type, IFullLogger>(
                (type, _) =>
                {
                    var ret = dependencyResolver.GetService<ILogger>();
                    if (ret == null)
                    {
                        throw new LoggingException("Couldn't find an ILogger. This should never happen, your dependency resolver is probably broken.");
                    }

                    return new WrappingFullLogger(ret, type);
                }, 64);
        }

        /// <inheritdoc />
        public IFullLogger GetLogger(Type type)
        {
            if (type == typeof(MemoizingMRUCache<Type, IFullLogger>))
            {
                return _nullLogger;
            }

            lock (_loggerCache)
            {
                return _loggerCache.Get(type);
            }
        }
    }
}
