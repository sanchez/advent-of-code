using System;
using Sanchez.AOC.Models;

namespace Sanchez.AOC.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected readonly SolutionContainer _solutions;

        public MainWindowViewModel(SolutionContainer solutions)
        {
            _solutions = solutions;
        }

        public string Greeting => "Welcome to Avalonia";
    }
}

