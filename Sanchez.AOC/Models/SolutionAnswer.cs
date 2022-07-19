using Avalonia.Threading;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Sanchez.AOC.Enums;

namespace Sanchez.AOC.Models;

public class SolutionAnswer : ReactiveObject
{
    protected readonly ISolution _solution;

    [Reactive] public SolutionStatus Status { get; protected set; }

    public int Year => _solution.Year;
    public int Day => _solution.Day;

    [Reactive] public bool CanExecute { get; set; } = true;

    public SolutionAnswer(ISolution solution)
    {
        _solution = solution;
        Status = SolutionStatus.NOT_STARTED;
    }

    public void Execute()
    {
        Task.Run(async () =>
        {
            await SetStatus(SolutionStatus.RUNNING);
            try
            {
                await Task.Delay(500);
                await SetStatus(SolutionStatus.SUCCESS);
            }
            catch (Exception ex)
            {
                await SetStatus(SolutionStatus.FAILED);
            }
        });
    }

    Task SetStatus(SolutionStatus status)
    {
        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            CanExecute = status != SolutionStatus.RUNNING;
            Status = status;
        });
    }
}

