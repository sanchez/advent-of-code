using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using Sanchez.AOC.Challenges._2021.Shared;
using Sanchez.AOC.Core;
using Sanchez.AOC.Core.Extensions;
using Sanchez.Geometry;

namespace Sanchez.AOC.Challenges._2021.Challenges
{
    public class Day19 : ISolution
    {
        class FloatingComparer : IEqualityComparer<double>
        {
            public bool Equals(double x, double y) => Math.Abs(x - y) <= 10.0;
            public int GetHashCode([DisallowNull] double obj) => obj.GetHashCode();
        }

        class Beacon : Dictionary<Scanner, Point3d>
        {
        }

        class Scanner : IEnumerable<Beacon>
        {
            readonly int _id;
            readonly IList<Beacon> _beacons;
            ICollection<double> _signature;

            public Scanner(int id)
            {
                _id = id;
                _beacons = new List<Beacon>();
            }

            public int Id => _id;

            public void Add(Beacon b) => _beacons.Add(b);
            public void Add(IEnumerable<Beacon> b)
            {
                foreach (var x in b)
                    _beacons.Add(x);
            }

            public void GenerateSignature()
            {
                var mappedBeacons = _beacons
                    .Select(x => x[this])
                    .ToList();

                _signature = new List<double>();
                foreach (var a in mappedBeacons)
                    foreach (var b in mappedBeacons)
                    {
                        if (a == b) continue;

                        _signature.Add((a - b).Magnitude());
                    }
            }
            public ICollection<double> Signature => _signature;

            public static bool HasOverlap(Scanner a, Scanner b)
            {
                var res = a.Signature.Intersect(b.Signature, new FloatingComparer()).ToList();
                Console.WriteLine(res.Count);
                return res.Count >= 66;
            }

            public override string ToString() => $"Scanner {_id}";
            public IEnumerator<Beacon> GetEnumerator() => _beacons.GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public string Part1()
        {
            // My idea is that two points will have the same distance between them regardless of the origin of the scanner, so find the distances between all the points then you should be able to work backwards from there and get all the scanners that have 12 beacons in common

            var input =
                InputLoader.Load()
                .NewLinedInput();

            var scanners = new List<Scanner>();

            Scanner scanner = null;
            foreach (var line in input)
            {
                var scannerLine = Regex.Match(line, @"^--- scanner (\d+) ---$");

                if (scannerLine.Success)
                {
                    if (scanner != null)
                        scanners.Add(scanner);
                    scanner = new Scanner(int.Parse(scannerLine.Groups[1].Value));
                }
                else
                {
                    var beaconLine = Regex.Match(line, @"^(-?\d+),(-?\d+),(-?\d+)$");
                    var beacon = new Beacon()
                    {
                        { scanner, new(
                            int.Parse(beaconLine.Groups[1].Value),
                            int.Parse(beaconLine.Groups[2].Value),
                            int.Parse(beaconLine.Groups[3].Value)) }
                    };
                    scanner.Add(beacon);
                }
            }
            if (scanner != null)
                scanners.Add(scanner);

            foreach (var s in scanners)
                s.GenerateSignature();

            var linkedScanners = scanners
                .Combination()
                .Select(x => (x.Item1, x.Item2, Scanner.HasOverlap(x.Item1, x.Item2)))
                //.Where(x => x.Item3)
                .ToList();

            return "";
        }

        public string Part2()
        {
            return "";
        }
    }
}
