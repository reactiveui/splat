using System;
using System.Reactive;

namespace Splat
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihost?view=aspnetcore-2.2
    /// </summary>
    public interface IApplicationHost
    {
        /// <summary>
        /// Gets the fully composed locator.
        /// </summary>
        IDependencyResolver Locator { get; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns>An observable sequence notifying completion.</returns>
        IObservable<Unit> Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>An observable sequence notifying completion.</returns>
        IObservable<Unit> Stop();
    }
}
