using System;
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

    public SolutionAnswer(ISolution solution)
    {
        _solution = solution;
        Status = SolutionStatus.NOT_STARTED;
    }
}

