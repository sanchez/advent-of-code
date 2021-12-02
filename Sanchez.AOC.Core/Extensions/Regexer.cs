using System;
using System.Text.RegularExpressions;

namespace Sanchez.AOC.Core.Extensions
{
    public static class Regexer
    {
        public static bool GetSingleInt(string pattern, string input, out int res)
        {
            var re = new Regex(input);
            var match = re.Match(pattern);

            if (match.Success)
            {
                res = int.Parse(match.Groups[1].Value);
                return true;
            }

            res = default;
            return false;
        }

        public static int GetSingleInt(string pattern, string input)
        {
            if (GetSingleInt(pattern, input, out int res))
            {
                return res;
            }

            throw new Exception("Not a valid number");
        }
    }
}
