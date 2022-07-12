using Sanchez.AOC.Models;

namespace Sanchez.AOC.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICollection<int> Years { get; }
        public int SelectedYear { get; set; }

        public MainWindowViewModel(SolutionContainer solutions)
        {
            Years = solutions.GetYears();
            SelectedYear = Years.FirstOrDefault();
        }

        public string Greeting => "Welcome to Avalonia";
    }
}

