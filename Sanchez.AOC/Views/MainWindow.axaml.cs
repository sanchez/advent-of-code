using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Sanchez.AOC.ViewModels;

namespace Sanchez.AOC.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}