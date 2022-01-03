using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using MoreLinq;
using Sanchez.AOC.Core;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day21 : ISolution
    {
        //delegate int Dice();

        //class Player
        //{
        //    int _position;
        //    int _id;
        //    int _score;

        //    public int Id => _id;
        //    public int Score => _score;

        //    public Player(int id, int startingPosition)
        //    {
        //        _id = id;
        //        _position = startingPosition;
        //    }

        //    public bool Turn(Dice d)
        //    {
        //        var totalRoll = d() + d() + d();
        //        _position += totalRoll;
        //        while (_position >= 11)
        //            _position -= 10;
        //        _score += _position;

        //        return _score >= 1000;
        //    }
        //}

        public string Part1()
        {
            //Player p1 = new(1, 9);
            //Player p2 = new(2, 6);

            //int diceRollCount = 0;
            //int diceValue = 1;
            //Dice deterministicDice = () =>
            //{
            //    diceRollCount++;

            //    var val = diceValue;
            //    if (++diceValue > 100)
            //        diceValue = 1;
            //    return val;
            //};

            //for (var i = 0; true; i++)
            //{
            //    if (p1.Turn(deterministicDice))
            //        return (p2.Score * diceRollCount).ToString();

            //    if (p2.Turn(deterministicDice))
            //        return (p1.Score * diceRollCount).ToString();
            //}
            return "";
        }

        record Player(int Position, int Score)
        {
            public bool HasWon => Score >= 21;
        }
        record Game(Player One, Player Two)
        {
            public bool HasWon => One.HasWon || Two.HasWon;
        }

        public string Part2()
        {
            var possibleRollsScore = new List<int>();
            for (var a = 1; a < 4; a++)
                for (var b = 1; b < 4; b++)
                    for (var c = 1; c < 4; c++)
                        possibleRollsScore.Add(a + b + c);

            var allPlayerScores = new List<Player>()
            {
                new(4, 0)
            };

            while (!allPlayerScores.All(x => x.HasWon))
            {
                Console.WriteLine(allPlayerScores.Count);
                var newPlayerScores = new List<Player>();
                foreach (var player in allPlayerScores)
                {
                    if (player.HasWon)
                    {
                        newPlayerScores.Add(player);
                    }
                    else
                    {
                        foreach (var score in possibleRollsScore)
                        {
                            var newPosition = player.Position + score;
                            if (newPosition > 10)
                                newPosition -= 10;
                            var newScore = player.Score + newPosition;
                            newPlayerScores.Add(new(newPosition, newScore));
                        }
                    }
                }
                allPlayerScores = newPlayerScores;
            }

            return "";
        }
    }
}
