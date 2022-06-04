using System.Reflection;
using System.Runtime.CompilerServices;
using Sanchez.AOC.Models;

namespace Sanchez.AOC;

public sealed class AOC
{
    readonly ICollection<ISolution> _solutions;
    readonly ICollection<UserAccount> _accounts;

    private AOC(ICollection<ISolution> solutions)
    {
        _solutions = solutions;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static AOC Scan(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

        List<ISolution> solutions = new();

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

        return new AOC(solutions);
    }

    public AOC WithAccount(string name)
    {
        // todo: Actually add the logic here to open up the web-browser

        _accounts.Add(new UserAccount(name, ""));

        return this;
    }

    public void TryExecute(int year)
    {

    }

    public void TryExecute(int year, int day)
    {

    }

    public void Execute()
    {

    }
}

