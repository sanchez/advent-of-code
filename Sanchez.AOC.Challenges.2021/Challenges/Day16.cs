using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sanchez.AOC.Challenges._2021.Shared;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day16 : ISolution
    {
        string ToBinary(char c) => c switch
        {
            '0' => "0000",
            '1' => "0001",
            '2' => "0010",
            '3' => "0011",
            '4' => "0100",
            '5' => "0101",
            '6' => "0110",
            '7' => "0111",
            '8' => "1000",
            '9' => "1001",
            'A' => "1010",
            'B' => "1011",
            'C' => "1100",
            'D' => "1101",
            'E' => "1110",
            'F' => "1111",
            _ => ""
        };

        int AddUpVersions(Packet packet) => packet switch
        {
            OperatorPacket op => op.ChildrenPackets.Select(x => AddUpVersions(x)).Append(op.Version).Sum(),
            _ => packet.Version
        };

        public string Part1()
        {
            var rawInput =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany(x => x.SelectMany(c => ToBinary(c)))
                .Select(x => x == '1')
                .ToArray();
            var input = new PacketData(rawInput);

            var packet = input.ReadInPacket();

            return AddUpVersions(packet).ToString();
        }

        public string Part2()
        {
            var rawInput =
                InputLoader.Load()
                .NewLinedInput()
                .SelectMany(x => x.SelectMany(c => ToBinary(c)))
                .Select(x => x == '1')
                .ToArray();
            var input = new PacketData(rawInput);

            var packet = input.ReadInPacket();

            var res = packet.Solve();

            return res.ToString();
        }
    }
}
