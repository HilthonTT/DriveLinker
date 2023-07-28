namespace DriveLinker.Core.Services;
public class SettingsService : ISettingsService
{
    private const string CacheName = nameof(SettingsService);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string DbName = "Settings.db4";
    private readonly IMemoryCache _cache;
    private readonly IPasswordGenerator _passwordGenerator;
    private SQLiteAsyncConnection _db;

    public SettingsService(
        IMemoryCache cache,
        IPasswordGenerator passwordGenerator)
    {
        _cache = cache;
        _passwordGenerator = passwordGenerator;
    }

    private async Task InitializeDb()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = GetDbPath();
        string password = await FetchPasswordAsync();

        var options = new SQLiteConnectionString(dbPath, true, password);

        _db = new(options);
        await _db.CreateTableAsync<Settings>();
    }

    public async Task<Settings> GetAccountSettingsAsync(int accountId)
    {
        await InitializeDb();

        string key = CacheNamePrefix + accountId;

        var output = _cache.Get<Settings>(key);
        if (output is null)
        {
            output = await _db.Table<Settings>().FirstOrDefaultAsync(s => s.AccountId == accountId);
            output ??= new();

            _cache.Set(key, output);
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
        await InitializeDb();

        RemoveCache(settings.AccountId);
        return await _db.InsertAsync(settings);
    }

    public async Task<int> UpdateSettingsAsync(Settings settings)
    {
        await InitializeDb();

        RemoveCache(settings.AccountId);
        return await _db.UpdateAsync(settings);
    }

    public async Task<int> DeleteSettingsAsync(Settings settings)
    {
        await InitializeDb();

        RemoveCache(settings.AccountId);
        return await _db.DeleteAsync(settings);
    }

    private async Task<string> FetchPasswordAsync()
    {
        var password = await _passwordGenerator.FetchPasswordAsync();
        return password;
    }

    private static string GetDbPath()
    {
        string dbPath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);

        return dbPath;
    }

    private void RemoveCache(int accountId)
    {
        string key = CacheNamePrefix + accountId;
        _cache.Remove(key);
    }
}

