using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    internal sealed class MainWindowViewModel : ReactiveObject
    {
        public RoutedViewHostPageViewModel RoutedViewHost { get; } = new();
    }
}
