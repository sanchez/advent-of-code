namespace Sanchez.AOC.Models
{
    public class SolutionContainer
    {
        protected readonly Dictionary<int, Dictionary<int, ISolution>> _solutions;

        public SolutionContainer(Dictionary<int, Dictionary<int, ISolution>> solutions)
        {
            _solutions = solutions;
        }

        public ISolution? GetSolution(int year, int day)
        {
            if (_solutions.TryGetValue(year, out Dictionary<int, ISolution>? days))
                if (days.TryGetValue(day, out ISolution? solution))
                    return solution;

            return null;
        }

        public ICollection<int> GetYears()
        {
            return _solutions.Keys;
        }

        public ICollection<int> GetDays(int year)
        {
            if (_solutions.TryGetValue(year, out Dictionary<int, ISolution>? solutions))
                return solutions.Keys;
            return Array.Empty<int>();
        }
    }
}

