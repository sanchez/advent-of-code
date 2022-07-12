using Sanchez.AOC.ViewModels;

namespace Sanchez.AOC.Root
{
    public class DesignData
    {
        public class FakeSolution : ISolution
        {
            public int Year { get; }
            public int Day { get; }

            public FakeSolution(int year, int day)
            {
                Year = year;
                Day = day;
            }

            public string Part1()
            {
                return "hello";
            }

            public string Part2()
            {
                return "world";
            }
        }

        public static Dictionary<int, Dictionary<int, ISolution>> FakeSolutions
        {
            get
            {
                Dictionary<int, Dictionary<int, ISolution>> sols = new()
                {
                    [2020] = new(),
                    [2021] = new(),
                    [2022] = new(),
                };

                sols[2020][1] = new FakeSolution(2020, 1);
                sols[2020][2] = new FakeSolution(2020, 2);
                sols[2020][3] = new FakeSolution(2020, 3);

                return sols;
            }
        }

        public static MainWindowViewModel MainWindow => new(new(FakeSolutions));
    }
}
