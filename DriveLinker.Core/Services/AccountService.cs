namespace DriveLinker.Core.Services;
public class AccountService : IAccountService
{
    private const string CacheName = nameof(AccountService);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string DbName = "Account.db4";
    private readonly IMemoryCache _cache;
    private readonly ISettingsService _settingsService;
    private readonly IDriveService _driveService;
    private SQLiteAsyncConnection _db;

    public AccountService(
        IMemoryCache cache,
        ISettingsService settingsService,
        IDriveService driveService)
    {
        _cache = cache;
        _settingsService = settingsService;
        _driveService = driveService;
        InitializeDb();
    }

    private void InitializeDb()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = GetDbPath();
        _db = new(dbPath);
        _db.CreateTableAsync<Account>();
    }

    public async Task<Account> GetAccountAsync(int id)
    {
        string key = CacheNamePrefix + id;

        var output = _cache.Get<Account>(key);
        if (output is null)
        {
            output = await _db.FindAsync<Account>(key);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<Account> GetAccountByUsernameAsync(string username)
    {
        string key = CacheNamePrefix + username;

        var output = _cache.Get<Account>(key);
        if (output is null)
        {
            output = await _db.Table<Account>().FirstOrDefaultAsync(a => a.Username == username);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<List<Drive>> GetAccountDrivesAsync(int accountId)
    {
        string key = $"{nameof(DriveService)}_{CacheNamePrefix}{accountId}";

        var output = _cache.Get<List<Drive>>(key);
        if (output is null)
        {
            output = await _driveService.GetAllAccountDrivesAsync(accountId);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<Settings> GetAccountSettingsService(int accountId)
    {
        string key = $"{nameof(SettingsService)}_{CacheNamePrefix}{accountId}";

        var output = _cache.Get<Settings>(key);
        if (output is null)
        {
            output = await _settingsService.GetAccountSettingsAsync(accountId);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<int> CreateAccountAsync(Account account)
    {
        await _db.InsertAsync(account);

        return account.Id;
    }

    public async Task<int> UpdateAccountAsync(Account account)
    {
        return await _db.UpdateAsync(account);
    }

    public async Task<int> DeleteAccountAsync(Account account)
    {
        var settings = await _settingsService.GetAccountSettingsAsync(account.Id);
        await _settingsService.DeleteSettingsAsync(settings);

        var drives = await _driveService.GetAllAccountDrivesAsync(account.Id);
        foreach (var drive in drives)
        {
            await _driveService.DeleteDriveAsync(drive);
        }

        return await _db.DeleteAsync(account);
    }

    private static string GetDbPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);
    }
}
