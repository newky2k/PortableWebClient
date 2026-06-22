using System.Windows;

namespace TestHarness;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel _viewModel;

    public MainViewModel ViewModel
    {
        get { return _viewModel; }
        set { _viewModel = value; DataContext = _viewModel; }
    }

    public MainWindow()
    {
        InitializeComponent();

        ViewModel = new MainViewModel();
    }
}
