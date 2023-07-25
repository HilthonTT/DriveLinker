namespace DriveLinker.Core.Services;
public class SettingsService : ISettingsService
{
    private const string CacheName = nameof(SettingsService);
    private const string DbName = "Settings.db3";
    private readonly IMemoryCache _cache;
    private SQLiteAsyncConnection _asyncDb;
    private SQLiteConnection _db;

    public SettingsService(IMemoryCache cache)
    {
        _cache = cache;
        InitializeAsyncDb();
        InitializeDb();
    }

    private void InitializeAsyncDb()
    {
        if (_asyncDb is not null)
        {
            return;
        }

        string dbPath = GetDbPath();

        _asyncDb = new(dbPath);
        _asyncDb.CreateTableAsync<Settings>();
    }

    private void InitializeDb()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = GetDbPath();

        _db = new(dbPath);
        _db.CreateTable<Settings>();
    }

    public async Task<Settings> GetSettingsAsync()
    {
        var output = _cache.Get<Settings>(CacheName);
        if (output is null)
        {
            output = await _asyncDb.Table<Settings>().FirstOrDefaultAsync();
            output ??= new();

            _cache.Set(CacheName, output);
        }

        return output;
    }

    public Settings GetSettings()
    {
        var output = _cache.Get<Settings>(CacheName);
        if (output is null)
        {
            output = _db.Table<Settings>().FirstOrDefault();
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
        return await _asyncDb.InsertAsync(settings);
    }

    public async Task<int> UpdateSettingsAsync(Settings settings)
    {
        return await _asyncDb.UpdateAsync(settings);
    }

    public async Task<int> DeleteSettingsAsync(Settings settings)
    {
        return await _asyncDb.DeleteAsync(settings);
    }

    private static string GetDbPath()
    {
        string dbPath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);

        return dbPath;
    }
}

