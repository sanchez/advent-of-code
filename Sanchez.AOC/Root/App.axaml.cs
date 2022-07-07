using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Sanchez.AOC.ViewModels;
using Sanchez.AOC.Views;

namespace Sanchez.AOC.Root;

public partial class App : Application
{
    private readonly IServiceProvider _services;
    private readonly Window _mainWindow;

    public App()
    {
    }

    public App(IServiceProvider services, Window mainWindow)
    {
        _services = services;
        _mainWindow = mainWindow;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
