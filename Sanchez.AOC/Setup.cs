using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Sanchez.AOC.IServices;
using Sanchez.AOC.Root;
using Sanchez.AOC.Services;
using Sanchez.AOC.Views;
using Splat;

namespace Sanchez.AOC;

public class Setup
{
    Dictionary<int, Dictionary<int, ISolution>> solutionDir = new();

    private Setup()
    {
    }

    public static Setup Init()
    {
        return new Setup();
    }

    public Setup AddSolution(ISolution solution)
    {
        if (!solutionDir.ContainsKey(solution.Year))
            solutionDir[solution.Year] = new();

        solutionDir[solution.Year][solution.Day] = solution;

        return this;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Setup UseAssemblyScanning(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

        Type solutionType = typeof(ISolution);

        foreach (Assembly assembly in assemblies)
            foreach (Type scanType in assembly.GetExportedTypes())
                if (solutionType.IsAssignableFrom(scanType))
                {
                    ISolution? solution = Activator.CreateInstance(scanType) as ISolution;
                    if (solution != null)
                        AddSolution(solution);
                }

        return this;
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