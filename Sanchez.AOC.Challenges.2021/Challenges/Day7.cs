using Sanchez.AOC.Core;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day7 : ISolution
    {
        public string Part1()
        {
            var input =
                InputLoader.Load()
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x)).ToList();

            var least = input.Min();
            var most = input.Max();

            var usage = new List<int>();
            for (var i = least; i <= most; i++)
            {
                var fuelSpent = input.Select(x => Math.Abs(x - i)).Sum();
                usage.Add(fuelSpent);
            }

            var leastFuel =
                usage.Min();
            return leastFuel.ToString();
        }

        public string Part2()
        {
            int FuelBurn(int diff)
            {
                if (diff == 0) return 0;
                return FuelBurn(diff - 1) + diff;
            }

            var input =
                InputLoader.Load()
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x)).ToList();

            var least = input.Min();
            var most = input.Max();

            var usage = new List<int>();
            for (var i = least; i <= most; i++)
            {
                var fuelSpent = input.Select(x =>
                {
                    var dist = Math.Abs(x - i);
                    var fuelCost = FuelBurn(dist);
                    return fuelCost;
                }).Sum();
                usage.Add(fuelSpent);
            }

            var leastFuel =
                usage.Min();
            return leastFuel.ToString();
        }
    }
}
