using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Sanchez.AOC.Models;
using Sanchez.AOC.Internal;

namespace Sanchez.AOC;

public sealed class AOC
{
    static readonly string _cacheDir = "cache";
    static readonly string _fileLocation = Path.Combine(_cacheDir, "storage.json");

    readonly ICollection<ISolution> _solutions;
    readonly ICollection<UserAccount> _accounts;

    private AOC(ICollection<ISolution> solutions, StorageFile storage)
    {
        _solutions = solutions;
        _accounts = storage.Accounts;
    }

    static StorageFile LoadStorage()
    {
        if (!Directory.Exists(_cacheDir))
            Directory.CreateDirectory(_cacheDir);

        if (File.Exists(_fileLocation))
        {
            StorageFile? content = JsonSerializer.Deserialize<StorageFile>(File.ReadAllText(_fileLocation));
            if (content != null)
                return content;
        }

        return new StorageFile()
        {
            Accounts = new List<UserAccount>()
        };
    }

    void Save()
    {
        if (!Directory.Exists(_cacheDir))
            Directory.CreateDirectory(_cacheDir);

        File.WriteAllText(_fileLocation, JsonSerializer.Serialize(new StorageFile()
        {
            Accounts = _accounts
        }));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static AOC Scan(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

        List<ISolution> solutions = new();
        StorageFile storage = LoadStorage();

        Type solutionType = typeof(ISolution);
        foreach (Assembly assembly in assemblies)
        {
            foreach (Type scanType in assembly.GetExportedTypes())
                if (solutionType.IsAssignableFrom(scanType))
                {
                    ISolution? solution = Activator.CreateInstance(scanType) as ISolution;
                    if (solution != null)
                        solutions.Add(solution);
                }
        }

        return new AOC(solutions, storage);
    }

    public AOC WithAccount(string name)
    {
        // todo: Actually add the logic here to open up the web-browser

        UserAccount? existingAccount = _accounts
            .Where(x => x.Name == name)
            .FirstOrDefault();
        if (existingAccount == null)
        {
            try
            {
                string token = Connection.CreateAccessToken().ConfigureAwait(false).GetAwaiter().GetResult();
                _accounts.Add(new UserAccount(name, token));
            }
            catch (Exception ex)
            {

            }
        }
        Save();

        return this;
    }

    async Task VerifyAnswer(int year, int day, int part, string answer)
    {
        // todo: need to send the answer here to the aoc website for an answer
    }

    async Task ProcessAnswer(int year, int day, int part, Func<string> answer)
    {

    }

    void Execute(Func<ISolution, bool> filter)
    {
        ICollection<ISolution> solutions = _solutions
            .Where(filter)
            .ToList();

        List<Task> runningTasks = new();

        foreach (ISolution solution in solutions)
        {
            runningTasks.Add(Task.Run(async () =>
            {
                string part1 = solution.Part1();
                await VerifyAnswer(solution.Year, solution.Day, 1, part1);

                string part2 = solution.Part2();
                await VerifyAnswer(solution.Year, solution.Day, 2, part2);
            }));
        }

        Task.WhenAll(runningTasks).Wait();
    }

    public void TryExecute(int year)
    {
        Execute(x => x.Year == year);
    }

    public void TryExecute(int year, int day)
    {
        Execute(x => x.Year == year && x.Day == day);
    }

    public void Execute()
    {
        Execute(x => true);
    }
}

