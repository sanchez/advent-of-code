using Sanchez.AOC.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Sanchez.AOC.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICollection<int> Years { get; }

        [Reactive] public int SelectedYear { get; set; }
        [ObservableAsProperty] public ICollection<int> Days { get; }
        [Reactive] public int SelectedDay { get; set; }

        public MainWindowViewModel(SolutionContainer solutions)
        {
            Years = solutions.GetYears().OrderBy(x => x).ToList();
            SelectedYear = Years.LastOrDefault();

            this.WhenAnyValue(x => x.SelectedYear)
                .Select(x => solutions.GetDays(x))
                .ToPropertyEx(this, x => x.Days)
                .DisposeWith(_disposable);
        }
    }
}

