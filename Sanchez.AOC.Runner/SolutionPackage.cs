using Sanchez.AOC.Core;

namespace Sanchez.AOC.Runner
{
    public class SolutionPackage
    {
        public int Year { get; set; }
        public int Day { get; set; }
        public ISolution Solution { get; set; }
    }
}
