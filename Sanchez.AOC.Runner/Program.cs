using Sanchez.AOC.Core;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sanchez.AOC.Runner
{
    internal class Program
    {
        const string fileLocation = "cache/lastRun.json";

        static Dictionary<int, Dictionary<int, ISolution>> SolutionMap = new();

        static void AddSolutionCollection<T>()
            where T : ISolutionStartup
        {
            var solutionAssembly = typeof(T).Assembly;

            var assemblyName = solutionAssembly.GetName().Name;
            var assemblyYearStr = assemblyName.Substring(assemblyName.LastIndexOf('.') + 1);

            var solutionNameRegex = new Regex(@"^Day(\d+)$");

            if (int.TryParse(assemblyYearStr, out var assemblyYear))
            {
                if (!SolutionMap.ContainsKey(assemblyYear))
                    SolutionMap[assemblyYear] = new();

                var solutionType = typeof(ISolution);
                foreach (var t in solutionAssembly.GetExportedTypes())
                {
                    if (solutionType.IsAssignableFrom(t))
                    {
                        var regexMatch = solutionNameRegex.Match(t.Name);
                        if (!regexMatch.Success) continue;
                        var day = int.Parse(regexMatch.Groups[1].Value);

                        SolutionMap[assemblyYear][day] = Activator.CreateInstance(t) as ISolution;
                    }
                }
            }
        }

        static async Task<SolutionPackage> LoadFromDisk()
        {
            if (File.Exists(fileLocation))
            {
                var fileContent = await File.ReadAllTextAsync(fileLocation);
                var res = JsonSerializer.Deserialize<LastRunInfo>(fileContent);
                if (res == null) return null;

                if (SolutionMap.TryGetValue(res.Year, out var yearSolution))
                    if (yearSolution.TryGetValue(res.Day, out var solution))
                        return new SolutionPackage()
                        {
                            Year = res.Year,
                            Day = res.Day,
                            Solution = solution
                        };
            }

            return null;
        }

        static async Task WriteToDisk(int year, int day)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fileLocation)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));

            var fileContent = JsonSerializer.Serialize(new LastRunInfo()
            {
                Year = year,
                Day = day
            });
            await File.WriteAllTextAsync(fileLocation, fileContent);
        }

        static int LoadDay(int year)
        {
            Console.WriteLine("Available Days:");
            foreach (var item in SolutionMap[year].Keys)
                Console.WriteLine($" - {item}");

            Console.Write("Please select a day: ");
            var dayInput = Console.ReadLine();
            Console.WriteLine();

            if (dayInput == "-1") return -1;

            if (int.TryParse(dayInput, out var day) && SolutionMap[year].ContainsKey(day))
            {
                return day;
            }

            Console.WriteLine($"Invalid day entered: '{dayInput}'");
            return LoadDay(year);
        }

        static int LoadYear()
        {
            Console.WriteLine("Available Years:");
            foreach (var item in SolutionMap.Keys)
                Console.WriteLine($" - {item}");

            Console.Write("Please select a year: ");
            var yearInput = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(yearInput, out var year) && SolutionMap.ContainsKey(year))
            {
                return year;
            }

            Console.WriteLine($"Invalid year entered: '{yearInput}'");
            return LoadYear();
        }

        static async Task InteractiveMode()
        {
            var year = LoadYear();
            var day = LoadDay(year);

            if (day == -1) return;

            await WriteToDisk(year, day);
            await ExecuteSolution(new SolutionPackage()
            {
                Year = year,
                Day = day,
                Solution = SolutionMap[year][day],
            });
        }

        static async Task ExecuteSolution(SolutionPackage solution)
        {
            try
            {
                var part1Answer = solution.Solution.Part1();
                Console.WriteLine($"Got answer for ({solution.Year}, {solution.Day}, 1) of {part1Answer}");
                var part2Answer = solution.Solution.Part2();
                Console.WriteLine($"Got answer for ({solution.Year}, {solution.Day}, 2) of {part2Answer}");
            }
            catch (Exception ex)
            {
                // TODO: Do something with the exception here
            }
        }

        static async Task Main(string[] args)
        {
            AddSolutionCollection<Challenges._2021.Startup>();

            var loadedSolution = await LoadFromDisk();
            if (loadedSolution == null)
            {
                Console.WriteLine("Found no cached version, entering interactive");
            }
            else
            {
                Console.WriteLine("Found cached version, running...");
                await ExecuteSolution(loadedSolution);
            }

            while (true)
            {
                await InteractiveMode();
            }
        }
    }
}
