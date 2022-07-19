using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Sanchez.AOC.Models;

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Sanchez.AOC.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICollection<int> Years { get; }
        [Reactive] public int SelectedYear { get; set; }

        [ObservableAsProperty] public ICollection<SolutionAnswer> Days { get; }
        [Reactive] public SolutionAnswer? SelectedDay { get; set; }

        public MainWindowViewModel(SolutionContainer solutions)
        {
            Years = solutions.GetYears().OrderBy(x => x).ToList();
            SelectedYear = Years.LastOrDefault();

            this.WhenAnyValue(x => x.SelectedYear)
                .Select(x => solutions.GetSolutions(x))
                .Do(_ =>
                {
                    SelectedDay = null;
                })
                .ToPropertyEx(this, x => x.Days)
                .DisposeWith(_disposable);
        }
    }
}

