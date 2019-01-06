using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A simple dependency resolver which takes Funcs for all its actions.
    /// GetService is always implemented via GetServices().LastOrDefault().
    /// </summary>
    public class FuncDependencyResolver : IMutableDependencyResolver
    {
        private readonly Func<Type, string, IEnumerable<object>> _innerGetServices;
        private readonly Action<Func<object>, Type, string> _innerRegister;
        private readonly Dictionary<Tuple<Type, string>, List<Action<IDisposable>>> _callbackRegistry = new Dictionary<Tuple<Type, string>, List<Action<IDisposable>>>();

        private IDisposable _inner;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncDependencyResolver"/> class.
        /// </summary>
        /// <param name="getAllServices">A func which will return all the services contained for the specified service type and contract.</param>
        /// <param name="register">A func which will be called when a service type and contract are registered.</param>
        /// <param name="toDispose">A optional disposable which is called when this resolver is disposed.</param>
        public FuncDependencyResolver(
            Func<Type, string, IEnumerable<object>> getAllServices,
            Action<Func<object>, Type, string> register = null,
            IDisposable toDispose = null)
        {
            _innerGetServices = getAllServices;
            _innerRegister = register;
            _inner = toDispose ?? ActionDisposable.Empty;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            return (GetServices(serviceType, contract) ?? Enumerable.Empty<object>()).LastOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _innerGetServices(serviceType, contract);
        }

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (_innerRegister == null)
            {
                throw new NotImplementedException();
            }

            _innerRegister(factory, serviceType, contract);

            var pair = Tuple.Create(serviceType, contract ?? string.Empty);

            if (_callbackRegistry.ContainsKey(pair))
            {
                List<Action<IDisposable>> toRemove = null;

                foreach (var callback in _callbackRegistry[pair])
                {
                    var remove = false;
                    var disp = new ActionDisposable(() => remove = true);

                    callback(disp);

                    if (remove)
                    {
                        if (toRemove == null)
                        {
                            toRemove = new List<Action<IDisposable>>();
                        }

                        toRemove.Add(callback);
                    }
                }

                if (toRemove != null)
                {
                    foreach (var c in toRemove)
                    {
                        _callbackRegistry[pair].Remove(c);
                    }
                }
            }
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            var pair = Tuple.Create(serviceType, contract ?? string.Empty);

            if (!_callbackRegistry.ContainsKey(pair))
            {
                _callbackRegistry[pair] = new List<Action<IDisposable>>();
            }

            _callbackRegistry[pair].Add(callback);

            return new ActionDisposable(() => _callbackRegistry[pair].Remove(callback));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of all managed memory from this class.
        /// </summary>
        /// <param name="isDisposing">If we are currently disposing managed resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                Interlocked.Exchange(ref _inner, ActionDisposable.Empty).Dispose();
            }

            _isDisposed = true;
        }
    }
}
