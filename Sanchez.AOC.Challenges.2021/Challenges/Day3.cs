using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day3 : ISolution
    {
        uint BoolToInt(IEnumerable<bool> input)
        {
            uint resNum = 0;
            foreach (var x in input)
            {
                resNum <<= 1;
                if (x) resNum |= 1;
            }
            return resNum;
        }

        public string Part1()
        {
            var input = // Contains an array of each number's dominant count
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => x.Select(y => y == '1' ? 1 : -1))
                .Transpose()
                .Select(x => x.Sum() >= 0)
                .ToArray();

            var invertedInput =
                input.Select(x => !x);

            var inputNum = BoolToInt(input);
            var invertedInputNum = BoolToInt(invertedInput);

            return (invertedInputNum * inputNum).ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => x.Select(y => y == '1').ToArray())
                .ToList();

            var mostScore = input;
            var leastScore = input;
            var inputWidth = input.First().Length;

            for (var i = 0; i < inputWidth; i++)
            {
                if (mostScore.Count() > 1)
                {
                    var majority = true;
                    var oneCount = mostScore
                        .Where(x => x[i])
                        .Count();
                    var zeroCount = mostScore
                        .Where(x => !x[i])
                        .Count();
                    if (zeroCount > oneCount) majority = false;

                    mostScore = mostScore.Where(x => x[i] == majority).ToList();
                }

                if (leastScore.Count() > 1)
                {
                    var minority = false;
                    var oneCount = leastScore
                        .Where(x => x[i])
                        .Count();
                    var zeroCount = leastScore
                        .Where(x => !x[i])
                        .Count();
                    if (oneCount < zeroCount) minority = true;

                    leastScore = leastScore.Where(x => x[i] == minority).ToList();
                }
            }

            var mostScoreNum = BoolToInt(mostScore.First());
            var leastScoreNum = BoolToInt(leastScore.First());

            return (mostScoreNum * leastScoreNum).ToString();
        }
    }
}
