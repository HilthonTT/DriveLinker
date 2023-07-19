using DriveLinker.Core.Models;
using DriveLinker.Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using SQLite;

namespace DriveLinker.Core.Services;
public class SettingsService : ISettingsService
{
    private const string CacheName = nameof(SettingsService);
    private const string DbName = "Settings.db3";
    private readonly IMemoryCache _cache;
    private SQLiteAsyncConnection _db;

    public SettingsService(IMemoryCache cache)
    {
        _cache = cache;
        InitializeDb();
    }

    private void InitializeDb()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);

        _db = new(dbPath);
        _db.CreateTableAsync<Settings>();
    }

    public async Task<Settings> GetSettingsAsync()
    {
        var output = _cache.Get<Settings>(CacheName);
        if (output is null)
        {
            output = await _db.Table<Settings>().FirstOrDefaultAsync();
            output ??= new();

            _cache.Set(CacheName, output);
        }

        return output;
    }

    public async Task<int> SetSettingsAsync(Settings settings)
    {
        if (settings.Id is 0)
        {
            return await CreateSettingsAsync(settings);
        }
        else
        {
            return await UpdateSettingsAsync(settings);
        }
    }

    public async Task<int> CreateSettingsAsync(Settings settings)
    {
        return await _db.InsertAsync(settings);
    }

    public async Task<int> UpdateSettingsAsync(Settings settings)
    {
        return await _db.UpdateAsync(settings);
    }

    public async Task<int> DeleteSettingsAsync(Settings settings)
    {
        return await _db.DeleteAsync(settings);
    }
}

