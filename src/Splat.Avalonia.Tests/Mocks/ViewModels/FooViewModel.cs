using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    internal sealed class FooViewModel(IScreen screen) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Foo";

        public IScreen HostScreen { get; } = screen;
    }
}
