using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    internal class BarViewModel(IScreen screen) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Bar";

        public IScreen HostScreen { get; } = screen;
    }
}
