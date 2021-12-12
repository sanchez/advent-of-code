using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day10 : ISolution
    {
        class InvalidChunkLineException : Exception
        {
            public InvalidChunkLineException() : base("Found an invalid line exception") { }
        }

        bool IsOpeningChar(char c) => c switch
        {
            '(' or '[' or '{' or '<' => true,
            _ => false
        };

        bool IsClosingChar(char c) => c switch
        {
            ')' or ']' or '}' or '>' => true,
            _ => false
        };

        char CloseCounterPart(char c) => c switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => default
        };

        protected (string ResultInput, char ErrorChar) LoadChunk(string input)
        {
            if (input.Length == 0) return ("", default);
            if (input.Length == 1) return (input[0..], default);

            var firstChar = input[0];

            if (IsOpeningChar(firstChar))
            {
                // Open a chunk

                string closingStr = input[1..];
                do
                {
                    char errorChar;
                    (closingStr, errorChar) = LoadChunk(closingStr);
                    if (errorChar != default) return ("", errorChar);
                } while (closingStr.Length > 1 && IsOpeningChar(closingStr[0]));

                //if (closingStr.Length == 0) throw new InvalidChunkLineException();
                if (closingStr.Length == 0) return ("", default);

                if (closingStr.Length == 1 && IsOpeningChar(closingStr[0])) return ("", default);

                if (IsClosingChar(closingStr[0]))
                {
                    var closingChar = closingStr[0];
                    if (firstChar == '(' && closingChar != ')') return ("", closingChar);
                    if (firstChar == '[' && closingChar != ']') return ("", closingChar);
                    if (firstChar == '{' && closingChar != '}') return ("", closingChar);
                    if (firstChar == '<' && closingChar != '>') return ("", closingChar);

                    return (closingStr[1..], default);
                }
            }

            return (input, default);
        }

        protected string MissingChunkChars(string input, IList<char> errors)
        {
            // We filtered out the corrupt so this input is 100% valid
            if (input.Length == 0) return "";

            var firstChar = input[0];

            var closingInput = input[1..];
            while (closingInput.Length > 0 && IsOpeningChar(closingInput[0]))
            {
                closingInput = MissingChunkChars(closingInput, errors);
            }

            if (closingInput.Length == 0)
            {
                errors.Add(CloseCounterPart(firstChar));
                return "";
            }

            return closingInput[1..];
        }

        protected long Scoring(IEnumerable<char> input)
        {
            long rt = 0;

            foreach (var c in input)
            {
                rt *= 5;
                rt += c switch
                {
                    ')' => 1,
                    ']' => 2,
                    '}' => 3,
                    '>' => 4,
                    _ => 0
                };
            }

            return rt;
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select((x, i) => LoadChunk(x))
                .Where(x => x.ErrorChar != default)
                .Select(x => x.ErrorChar switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => 0
                })
                .Sum();

            return input.ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select((x, i) => (x, LoadChunk(x)))
                .Where(x => x.Item2.ErrorChar == default)
                .Select(x =>
                {
                    var errors = new List<char>();
                    MissingChunkChars(x.Item1, errors);
                    return errors;
                })
                .Select(x => Scoring(x))
                .Where(x => x != 0)
                .OrderByDescending(x => x)
                .ToList();

            //var test =
            //    "[({(<(())[]>[[{[]{<()<>>"
            //    //"[[]"
            //    .NewLinedInput()
            //    .Select(x => (x, LoadChunk(x)))
            //    .Where(x => x.Item2.ErrorChar == default)
            //    .Select(x =>
            //    {
            //        var errors = new List<char>();
            //        MissingChunkChars(x.Item1, errors);
            //        return errors;
            //    })
            //    .ToList();

            return input[input.Count / 2].ToString();
        }
    }
}
