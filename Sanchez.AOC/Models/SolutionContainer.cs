namespace Sanchez.AOC.Models
{
    public class SolutionContainer
    {
        protected readonly Dictionary<int, Dictionary<int, SolutionAnswer>> _solutions;

        public SolutionContainer(Dictionary<int, Dictionary<int, ISolution>> solutions)
        {
            _solutions = new();
            foreach (int year in solutions.Keys)
            {
                _solutions[year] = new();
                foreach (int day in solutions[year].Keys)
                    _solutions[year][day] = new(solutions[year][day]);
            }
        }

        public SolutionAnswer? GetSolution(int year, int day)
        {
            if (_solutions.TryGetValue(year, out Dictionary<int, SolutionAnswer>? days))
                if (days.TryGetValue(day, out SolutionAnswer? solution))
                    return solution;

            return null;
        }

        public ICollection<int> GetYears()
        {
            return _solutions.Keys;
        }

        public ICollection<int> GetDays(int year)
        {
            if (_solutions.TryGetValue(year, out Dictionary<int, SolutionAnswer>? solutions))
                return solutions.Keys;
            return Array.Empty<int>();
        }

        public ICollection<SolutionAnswer> GetSolutions(int year)
        {
            if (_solutions.TryGetValue(year, out Dictionary<int, SolutionAnswer>? solutions))
                return solutions.Values;
            return Array.Empty<SolutionAnswer>();
        }
    }
}

