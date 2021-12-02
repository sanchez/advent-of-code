using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sanchez.AOC.Core
{
    public class InputLoader
    {
        protected static async Task<string> LoadSession()
        {
            var fileName = "inputs/sessionKey.txt";
            var fileDir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            if (File.Exists(fileName))
            {
                return await File.ReadAllTextAsync(fileName);
            }
            else
            {
                Console.Write("Please enter session key: ");
                var key = Console.ReadLine();

                await File.WriteAllTextAsync(fileName, key);

                return key;
            }
        }

        protected static async Task<string> LoadFromWebsite(int year, int day)
        {
            var url = $"https://adventofcode.com/{year}/day/{day}/input";

            var sessionKey = await LoadSession();

            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer,
            };
            using var client = new HttpClient(handler);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            cookieContainer.Add(new Cookie("session", sessionKey)
            {
                Domain = "adventofcode.com"
            });

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        protected static async Task<string> LoadFromFile(int year, int day)
        {
            var fileName = $"inputs/{year}-{day}-input.txt";
            var dir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(fileName))
            {
                return await File.ReadAllTextAsync(fileName);
            }

            return null;
        }

        protected static async Task SaveToFile(int year, int day, string data)
        {
            var fileName = $"inputs/{year}-{day}-input.txt";
            var dir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            await File.WriteAllTextAsync(fileName, data);
        }

        protected static async Task<string> Load(int year, int day)
        {
            var data = await LoadFromFile(year, day);

            if (data == null)
            {
                data = await LoadFromWebsite(year, day);
                await SaveToFile(year, day, data);
            }

            return data;
        }

        public static string Load()
        {
            var stackFrame = new StackFrame(1);
            var method = stackFrame.GetMethod();

            var assemblyName = method.DeclaringType.Assembly.GetName().Name;
            var assemblyYearStr = assemblyName.Substring(assemblyName.LastIndexOf('.') + 1);
            var year = int.Parse(assemblyYearStr);

            var solutionNameRegex = new Regex(@"^Day(\d+)$");
            var methodName = solutionNameRegex.Match(method.DeclaringType.Name);

            if (!methodName.Success) return "";

            var day = int.Parse(methodName.Groups[1].Value);

            var loadTask = Load(year, day);
            loadTask.Wait();

            return loadTask.Result;
        }
    }
}
