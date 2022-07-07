using System;
namespace Sanchez.AOC;

public interface ISolution
{
    int Year { get; }
    int Day { get; }

    string Part1();
    string Part2();
}