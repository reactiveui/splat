using System;

namespace Splat.Host
{

    /// <summary>
    /// Used by ReactiveUI when first starting up, it will seek out classes
    /// inside our own ReactiveUI projects. The implemented methods will
    /// register with Splat their dependencies.
    /// </summary>
    public interface IStartupRegistration
    {
        /// <summary>
        /// Register platform dependencies inside Splat.
        /// </summary>
        /// <param name="registerFunction">A method the deriving class will class to register the type.</param>
        void Register(Action<Func<object>, Type> registerFunction);
    }
}
