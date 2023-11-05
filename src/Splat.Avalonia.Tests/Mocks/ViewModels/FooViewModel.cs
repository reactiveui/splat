using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    internal class FooViewModel(IScreen screen) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Foo";

        public IScreen HostScreen { get; } = screen;
    }
}
