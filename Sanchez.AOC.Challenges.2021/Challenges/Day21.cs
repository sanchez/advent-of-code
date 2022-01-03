using System;
using System.Collections.Generic;
using System.Linq;
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
            var onGoingGames = new List<Game>()
            {
                new Game(new(4, 0), new(8, 0))
            };

            ICollection<Player> RollForPlayer(Player p)
            {
                var players = new List<Player>();
                for (var i = 1; i < 4; i++)
                {
                    var newPlayer = p with { Position = p.Position + i };
                    if (newPlayer.Position > 10)
                        newPlayer = newPlayer with { Position = p.Position - 10 };
                    players.Add(newPlayer);
                }
                return players;
            }

            Player CalculateScore(Player p) => p with { Score = p.Score + p.Position };

            long pOneWins = 0;
            long pTwoWins = 0;

            while (!onGoingGames.All(x => x.HasWon))
            {
                var newGames = new List<Game>();
                Console.WriteLine($"Games {onGoingGames.Count}");

                foreach (var game in onGoingGames)
                {
                    if (game.HasWon)
                    {
                        if (game.One.HasWon)
                            pOneWins++;
                        if (game.Two.HasWon)
                            pTwoWins++;
                    }
                    else
                    {
                        var resultingPlayerOne = new List<Player>();
                        foreach (var firstRoll in RollForPlayer(game.One))
                            foreach (var secondRoll in RollForPlayer(game.One))
                                foreach (var thirdRoll in RollForPlayer(game.One))
                                    resultingPlayerOne.Add(CalculateScore(thirdRoll));

                        foreach (var playerOne in resultingPlayerOne)
                        {
                            if (playerOne.HasWon)
                                newGames.Add(game with { One = playerOne });
                            else
                                foreach (var firstRoll in RollForPlayer(game.Two))
                                    foreach (var secondRoll in RollForPlayer(game.Two))
                                        foreach (var thirdRoll in RollForPlayer(game.Two))
                                            newGames.Add(game with { Two = CalculateScore(thirdRoll) });
                        }
                    }
                }

                onGoingGames = newGames;
            }

            return Math.Max(pOneWins, pTwoWins).ToString();
        }
    }
}
