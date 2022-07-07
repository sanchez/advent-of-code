using System;
using System.Text.Json;
using Sanchez.AOC.IServices;
using Sanchez.AOC.Models;

namespace Sanchez.AOC.Services;

public class StorageService : IStorageService
{
    static readonly string _cacheDir = "cache";
    static readonly string _fileLocation = Path.Combine(_cacheDir, "storage.json");

    StorageFile? _loadedFile = null;

    async Task<StorageFile> LoadStorage()
    {
        if (!Directory.Exists(_cacheDir))
            Directory.CreateDirectory(_cacheDir);

        if (File.Exists(_fileLocation))
        {
            string fileContents = await File.ReadAllTextAsync(_fileLocation);
            _loadedFile = JsonSerializer.Deserialize<StorageFile>(fileContents);
        }

        if (_loadedFile == null)
        {
            _loadedFile = new StorageFile()
            {
                Accounts = new List<UserAccount>()
            };
            await SaveStorage();
        }

        return _loadedFile;
    }

    async Task SaveStorage()
    {
        if (_loadedFile == null)
            return;

        if (!Directory.Exists(_cacheDir))
            Directory.CreateDirectory(_cacheDir);

        string fileContents = JsonSerializer.Serialize(_loadedFile);
        await File.WriteAllTextAsync(_fileLocation, fileContents);
    }
}

