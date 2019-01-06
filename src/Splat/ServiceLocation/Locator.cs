using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A Locator which will host the container for dependency injection based operations.
    /// </summary>
    public static class Locator
    {
        private static readonly List<Action> _resolverChanged = new List<Action>();

        [ThreadStatic]
        private static IDependencyResolver _unitTestDependencyResolver;
        private static IDependencyResolver _dependencyResolver;

        /// <summary>
        /// Initializes static members of the <see cref="Locator"/> class.
        /// This will by default register a <see cref="ModernDependencyResolver"/>.
        /// </summary>
        static Locator()
        {
            _dependencyResolver = new ModernDependencyResolver();

            RegisterResolverCallbackChanged(() =>
            {
                if (CurrentMutable == null)
                {
                    return;
                }

                CurrentMutable.InitializeSplat();
            });
        }

        /// <summary>
        /// Gets or sets the dependency resolver. This class is used throughout
        /// libraries for many internal operations as well as for general use
        /// by applications. If this isn't assigned on startup, a default, highly
        /// capable implementation will be used, and it is advised for most people
        /// to simply use the default implementation.
        /// </summary>
        /// <value>The dependency resolver.</value>
        public static IDependencyResolver Current
        {
            get => _unitTestDependencyResolver ?? _dependencyResolver;
            set
            {
                if (ModeDetector.InUnitTestRunner())
                {
                    _unitTestDependencyResolver = value;
                    _dependencyResolver = _dependencyResolver ?? value;
                }
                else
                {
                    _dependencyResolver = value;
                }

                var currentCallbacks = default(Action[]);
                lock (_resolverChanged)
                {
                    // NB: Prevent deadlocks should we reenter this setter from
                    // the callbacks
                    currentCallbacks = _resolverChanged.ToArray();
                }

                foreach (var block in currentCallbacks)
                {
                    block();
                }
            }
        }

        /// <summary>
        /// Gets or sets the mutable dependency resolver.
        /// The default resolver is also a mutable resolver, so this will be non-null.
        /// Use this to register new types on startup if you are using the default resolver.
        /// </summary>
        public static IMutableDependencyResolver CurrentMutable
        {
            get => Current as IMutableDependencyResolver;
            set => Current = value;
        }

        /// <summary>
        /// This method allows libraries to register themselves to be set up
        /// whenever the dependency resolver changes. Applications should avoid
        /// this method, it is usually used for libraries that depend on service
        /// location.
        /// </summary>
        /// <param name="callback">A callback that is invoked when the
        /// resolver is changed. This callback is also invoked immediately,
        /// to configure the current resolver.</param>
        /// <returns>When disposed, removes the callback. You probably can
        /// ignore this.</returns>
        public static IDisposable RegisterResolverCallbackChanged(Action callback)
        {
            lock (_resolverChanged)
            {
                _resolverChanged.Add(callback);
            }

            // NB: We always immediately invoke the callback to set up the
            // current resolver with whatever we've got
            callback();

            return new ActionDisposable(() =>
            {
                lock (_resolverChanged)
                {
                    _resolverChanged.Remove(callback);
                }
            });
        }
    }
}
