using Avalonia;
using Avalonia.ReactiveUI;
using Sanchez.AOC.Root;

namespace Sanchez.AOC;

public class Setup
{
    private Setup()
    {
    }

    public static Setup Init()
    {
        return new Setup();
    }

    public void Run(string[] args)
    {
        //Thread thread = new(() =>
        //{

        //});
        //thread.SetApartmentState(ApartmentState.STA);
        //thread.Start();

        AppBuilder builder = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        builder.StartWithClassicDesktopLifetime(args);
    }
}