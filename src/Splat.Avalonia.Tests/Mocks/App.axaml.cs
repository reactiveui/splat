using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUIDemo.ViewModels;
using ReactiveUIDemo.Views;
using Splat;

namespace ReactiveUIDemo
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            Locator.CurrentMutable.Register(() => new FooView(), typeof(IViewFor<FooViewModel>));
            Locator.CurrentMutable.Register(() => new BarView(), typeof(IViewFor<BarViewModel>));
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
