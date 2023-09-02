using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUIDemo.ViewModels;

namespace ReactiveUIDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
