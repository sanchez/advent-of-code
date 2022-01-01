using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using Sanchez.AOC.Challenges._2021.Shared;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day18 : ISolution
    {
        interface INumber
        {
            INumber Parent { get; set; }

            long Magnitude();
        }

        class LiteralNumber : INumber
        {
            public int Value { get; set; }

            public INumber Parent { get; set; }

            public LiteralNumber(int value)
            {
                Value = value;
            }

            public override string ToString() => $"{Value}";

            public long Magnitude() => Value;
        }

        class PairNumber : INumber
        {
            public INumber Left { get; set; }
            public INumber Right { get; set; }

            public INumber Parent { get; set; }

            public PairNumber(INumber left, INumber right)
            {
                Left = left;
                Right = right;
            }

            public override string ToString() => $"[{Left}, {Right}]";

            public long Magnitude() => (3 * Left.Magnitude()) + (2 * Right.Magnitude());
        }

        bool IsNumber(char[] input)
        {
            if (input[0] >= '0' && input[0] <= '9')
                return true;
            return false;
        }

        (INumber, char[]) ParseInput(char[] input)
        {
            if (IsNumber(input))
                return (new LiteralNumber(input[0] - '0'), input[1..]);

            if (input[0] == '[')
            {
                var (left, leftRemainder) = ParseInput(input[1..]);
                var (right, rightRemainder) = ParseInput(leftRemainder[1..]);
                var num = new PairNumber(left, right);
                left.Parent = num;
                right.Parent = num;
                return (num, rightRemainder[1..]);
            }

            throw new Exception("We shouldn't get here");
        }

        bool IsLiteralPair(INumber currentNum, out LiteralNumber left, out LiteralNumber right)
        {
            left = null;
            right = null;
            if (currentNum is PairNumber pn)
                if (pn.Left is LiteralNumber a && pn.Right is LiteralNumber b)
                {
                    left = a; right = b;
                    return true;
                }

            return false;
        }

        LiteralNumber FindLeftNumber(INumber currentPos)
        {
            if (currentPos == null) return null;

            var parent = currentPos.Parent as PairNumber;

            if (parent?.Right == currentPos) // the current position is right, therefore we can search left
            {
                var start = parent.Left;
                while (true)
                {
                    if (start is LiteralNumber x)
                        return x;

                    if (start is PairNumber pn)
                        start = pn.Right;
                }
            }

            return FindLeftNumber(parent);
        }

        LiteralNumber FindRightNumber(INumber currentPos)
        {
            if (currentPos == null) return null;

            var parent = currentPos.Parent as PairNumber;

            if (parent == null)
                return null;

            if (parent?.Left == currentPos) // the current position is left, therefore we can search right
            {
                var start = parent.Right;
                while (true)
                {
                    if (start is LiteralNumber x)
                        return x;

                    if (start is PairNumber pn)
                        start = pn.Left;
                }
            }

            return FindRightNumber(parent);
        }

        void ReplaceNumber(INumber currentNum, INumber newNumber)
        {
            var parent = currentNum.Parent as PairNumber;
            newNumber.Parent = parent;

            if (currentNum == parent.Left)
                parent.Left = newNumber;
            else if (currentNum == parent.Right)
                parent.Right = newNumber;
        }

        bool Explode(INumber currentNum, int depth = 0)
        {
            if (IsLiteralPair(currentNum, out LiteralNumber currentLeft, out LiteralNumber currentRight)
                && depth >= 4) // leftmost pair explodes
            {
                var leftNum = FindLeftNumber(currentNum);
                if (leftNum != null)
                    leftNum.Value += currentLeft.Value;

                var rightNum = FindRightNumber(currentNum);
                if (rightNum != null)
                    rightNum.Value += currentRight.Value;

                var newValue = new LiteralNumber(0)
                {
                    Parent = currentNum.Parent
                };

                ReplaceNumber(currentNum, newValue);

                return true;
            }

            if (currentNum is PairNumber nextPair)
            {
                if (Explode(nextPair.Left, depth + 1))
                    return true;

                if (Explode(nextPair.Right, depth + 1))
                    return true;
            }

            return false;
        }

        bool Split(INumber currentNum)
        {
            if (currentNum is LiteralNumber x && x.Value >= 10) // split this pair
            {
                var left = (int)Math.Floor(x.Value / 2.0);
                var leftNum = new LiteralNumber(left);
                var right = (int)Math.Ceiling(x.Value / 2.0);
                var rightNum = new LiteralNumber(right);
                var newPair = new PairNumber(leftNum, rightNum);
                leftNum.Parent = newPair;
                rightNum.Parent = newPair;

                ReplaceNumber(currentNum, newPair);

                return true;
            }

            if (currentNum is PairNumber nextPair)
            {
                if (Split(nextPair.Left))
                    return true;

                if (Split(nextPair.Right))
                    return true;
            }

            return false;
        }

        void RecurseReduce(INumber num)
        {
            while (true)
            {
                if (Explode(num))
                    continue;
                if (Split(num))
                    continue;
                break;
            }
        }

        public string Part1()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .Select(x => ParseInput(x.ToArray()).Item1)
                .Aggregate((a, b) =>
                {
                    var totalNum = new PairNumber(a, b);
                    a.Parent = totalNum;
                    b.Parent = totalNum;

                    RecurseReduce(totalNum);

                    return totalNum;
                });

            return input.Magnitude().ToString();
        }

        public string Part2()
        {
            var input =
                InputLoader.Load()
                .NewLinedInput()
                .ToList();

            long AddTwoNumbers(string a, string b)
            {
                var left = ParseInput(a.ToArray()).Item1;
                var right = ParseInput(b.ToArray()).Item1;

                var pair = new PairNumber(left, right);
                left.Parent = pair;
                right.Parent = pair;

                RecurseReduce(pair);

                return pair.Magnitude();
            }

            var largest = long.MinValue;
            foreach (var a in input)
                foreach (var b in input)
                {
                    var x = Math.Max(AddTwoNumbers(a, b), AddTwoNumbers(b, a));
                    if (x > largest)
                        largest = x;
                }

            return largest.ToString();
        }
    }
}
