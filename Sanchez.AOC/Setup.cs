using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Sanchez.AOC.IServices;
using Sanchez.AOC.Models;
using Sanchez.AOC.Root;
using Sanchez.AOC.Services;
using Sanchez.AOC.Views;
using Splat;

namespace Sanchez.AOC;

public class Setup
{
    IServiceCollection _serviceContainer = new ServiceCollection();
    Dictionary<int, Dictionary<int, ISolution>> _solutions = new();

    private Setup()
    {
    }

    public static Setup Init()
    {
        Setup setup = new();

        setup.LocateViewModels();

        return setup;
    }

    void LocateViewModels()
    {
        foreach (Type scanType in this.GetType().Assembly.GetTypes())
            if (scanType.FullName?.Contains("ViewModels") ?? false)
            {
                _serviceContainer.AddTransient(scanType);
            }
    }

    public Setup AddSolution(ISolution solution)
    {
        if (!_solutions.ContainsKey(solution.Year))
            _solutions[solution.Year] = new();

        _solutions[solution.Year][solution.Day] = solution;

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

        _serviceContainer.AddSingleton(new SolutionContainer(_solutions));

        IServiceProvider services = _serviceContainer.BuildServiceProvider();

        AppBuilder builder = AppBuilder.Configure(() => new App(services))
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        builder.StartWithClassicDesktopLifetime(args);
    }
}