using System.Mvvm;
using System.Windows;

namespace TestHarness;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        MvvmManager.Init();

    }
}
