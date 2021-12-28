using Sanchez.AOC.Core;

using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day06 : ISolution
    {
        public class LanternFish
        {
            protected int _lifeSpan;

            public int LifeSpan => _lifeSpan;

            public LanternFish(int lifeSpan)
            {
                _lifeSpan = lifeSpan;
            }

            public LanternFish Cycle()
            {
                if (_lifeSpan <= 0)
                {
                    _lifeSpan = 6;
                    return new LanternFish(8);
                }

                _lifeSpan--;
                return null;
            }
        }

        public string Part1()
        {
            var fish =
                InputLoader.Load()
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x))
                .Select(x => new LanternFish(x))
                .ToList();

            for (var i = 0; i < 80; i++)
            {
                var newFish = fish
                    .Select(x => x.Cycle())
                    .Where(x => x != null)
                    .ToList();
                fish.AddRange(newFish);
            }

            return fish.Count.ToString();
        }

        public string Part2()
        {
            var pool = new Dictionary<int, long>();
            for (var i = 0; i < 8; i++)
                pool[i] = 0;

            var input =
                InputLoader.Load()
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => byte.Parse(x))
                .GroupBy(x => x);
            foreach (var group in input)
                pool[group.Key] = group.Count();

            for (var cycle = 0; cycle < 256; cycle++)
            {
                var newFishCount = pool[0];
                for (var i = 1; i < pool.Count; i++)
                {
                    pool[i - 1] = pool[i];
                }
                pool[6] += newFishCount;
                pool[8] = newFishCount;
            }

            return pool.Select(x => x.Value).Sum().ToString();
        }
    }
}
