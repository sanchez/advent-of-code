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

        record Player(int Weight, int TurnCount, int Position, int Score)
        {
            public bool HasWon => Score >= 21;
        }

        IList<Player> SimulatePlayer(List<(int Position, int Weight)>[] positionLookup, Player startingPlayer)
        {
            var completedPlayer = new List<Player>();
            var onGoingPlayer = new List<Player>
            {
                startingPlayer
            };

            while (onGoingPlayer.Count > 0)
            {
                var newPlayers = new List<Player>();
                foreach (var player in onGoingPlayer)
                    foreach (var possible in positionLookup[player.Position])
                    {
                        var newPlayer = player with
                        {
                            TurnCount = player.TurnCount + 1,
                            Position = possible.Position,
                            Score = player.Score + possible.Position,
                            Weight = player.Weight + possible.Weight
                        };

                        if (newPlayer.HasWon)
                            completedPlayer.Add(newPlayer);
                        else newPlayers.Add(newPlayer);
                    }
                onGoingPlayer = newPlayers;
            }

            return completedPlayer;
        }

        ICollection<Player> ExpandPlayer(ICollection<Player> player)
        {
            var size = player.Select(x => x.Weight).Sum();
            var res = new List<Player>(size);

            foreach (var x in player)
                for (var i = 0; i < x.Weight; i++)
                    res.Add(x);

            return res;
        }

        public string Part2()
        {
            return "";

            //var possibleRollsScore = new List<int>();
            //for (var a = 1; a < 4; a++)
            //    for (var b = 1; b < 4; b++)
            //        for (var c = 1; c < 4; c++)
            //            possibleRollsScore.Add(a + b + c);
            //var weightedPossibleScore = possibleRollsScore
            //    .GroupBy(x => x)
            //    .Select(x => (x.Key, x.Count()))
            //    .ToArray();

            //var positionLookup = new List<(int Position, int Weight)>[11];
            //positionLookup[0] = new List<(int Position, int Weight)>();

            //for (var i = 1; i < 11; i++)
            //{
            //    var possiblePosition = new List<(int Position, int Weight)>();

            //    foreach (var roll in weightedPossibleScore)
            //    {
            //        var newPosition = i + roll.Key;
            //        if (newPosition > 10)
            //            newPosition -= 10;

            //        possiblePosition.Add((newPosition, roll.Item2));
            //    }

            //    positionLookup[i] = possiblePosition;
            //}

            //var playerOne = ExpandPlayer(SimulatePlayer(positionLookup, new(0, 0, 4, 0)));
            //var playerTwo = ExpandPlayer(SimulatePlayer(positionLookup, new(0, 0, 8, 0)));

            //long playerOneWins = 0;
            //long playerTwoWins = 0;
            //foreach (var pOne in playerOne)
            //    foreach (var pTwo in playerTwo)
            //    {
            //        //var weight = pOne.Weight * pTwo.Weight;
            //        var weight = 1;
            //        if (pOne.TurnCount <= pTwo.TurnCount)
            //            playerOneWins += weight;
            //        else playerTwoWins += weight;
            //    }

            //if (playerOneWins != 444356092776315)
            //    return "Player One";
            //if (playerTwoWins != 341960390180808)
            //    return "Player Two";

            //return "";
        }
    }
}
