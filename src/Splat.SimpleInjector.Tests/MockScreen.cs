using ReactiveUI;

namespace Splat.Simplnjector
{
    /// <summary>
    /// Mock screen.
    /// </summary>
    /// <seealso cref="ReactiveUI.IScreen" />
    public class MockScreen : IScreen
    {
        /// <inheritdoc />
        public RoutingState Router { get; }
    }
}