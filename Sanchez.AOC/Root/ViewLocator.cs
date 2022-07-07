using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using Sanchez.AOC.ViewModels;
using System;

namespace Sanchez.AOC.Root;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModels", "Views");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }

    public static object GetViewModel(object view, IServiceProvider provider)
    {
        string name = view.GetType().FullName!.Replace("Views", "ViewModels");
        Type? type = Type.GetType(name);

        if (type != null)
            return ActivatorUtilities.CreateInstance(provider, type);

        return null;
    }

    public static IControl GetView(object viewModel, IServiceProvider provider)
    {
        string name = viewModel.GetType().FullName!.Replace("ViewModels", "Views");
        Type? type = Type.GetType(name);

        if (type != null)
            return (IControl)ActivatorUtilities.CreateInstance(provider, type);

        return new TextBlock { Text = "Not Found: " + name };
    }
}