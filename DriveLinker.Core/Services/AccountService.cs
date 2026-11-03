namespace DriveLinker.Core.Services;
public class AccountService : IAccountService
{
    private const string CacheName = nameof(AccountService);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string DbName = "Account.db4";
    private readonly IMemoryCache _cache;
    private readonly ISettingsService _settingsService;
    private readonly IDriveService _driveService;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IAccount _account;
    private SQLiteAsyncConnection _db;

    public AccountService(
        IMemoryCache cache,
        ISettingsService settingsService,
        IDriveService driveService,
        IPasswordGenerator passwordGenerator,
        IAccount account)
    {
        _cache = cache;
        _settingsService = settingsService;
        _driveService = driveService;
        _passwordGenerator = passwordGenerator;
        _account = account;
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
        await _db.CreateTableAsync<Account>();
    }

    public async Task<Account> GetAccountAsync(int id)
    {
        await InitializeDb();

        string key = CacheNamePrefix + id;

        var output = _cache.Get<Account>(key);
        if (output is null)
        {
            output = await _db.FindAsync<Account>(id);

            // Never cache a miss: the account may be created moments later.
            if (output is not null)
            {
                _cache.Set(key, output);
            }
        }

        return output;
    }

    public async Task<Account> GetAccountByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        await InitializeDb();

        string key = GetUsernameCacheKey(username);

        var output = _cache.Get<Account>(key);
        if (output is null)
        {
            var accounts = await _db.Table<Account>().ToListAsync();

            output = accounts.FirstOrDefault(a =>
                string.Equals(a.Username, username, StringComparison.OrdinalIgnoreCase));

            // Never cache a miss. Registration and username changes look a name up *before*
            // it exists; caching that null made the new account impossible to log into until
            // the app was restarted.
            if (output is not null)
            {
                _cache.Set(key, output);
            }
        }

        return output;
    }

    public async Task<List<Drive>> GetAccountDrivesAsync()
    {
        return await _driveService.GetAllAccountDrivesAsync(_account.Id);
    }

    public async Task<Settings> GetAccountSettingsAsync()
    {
        return await _settingsService.GetAccountSettingsAsync(_account.Id);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        await InitializeDb();

        await _db.InsertAsync(account);
        InvalidateAccountCache(account.Username, account.Id);

        return account;
    }

    public async Task<int> UpdateAccountAsync(Account account, string previousUsername = null)
    {
        await InitializeDb();

        InvalidateAccountCache(previousUsername, account.Id);
        InvalidateAccountCache(account.Username, account.Id);

        return await _db.UpdateAsync(account);
    }

    public async Task<int> DeleteAccountAsync(Account account)
    {
        await InitializeDb();

        var settings = await _settingsService.GetAccountSettingsAsync(account.Id);
        await _settingsService.DeleteSettingsAsync(settings);

        foreach (var drive in await _driveService.GetAllAccountDrivesAsync(account.Id))
        {
            await _driveService.DeleteDriveAsync(drive);
        }

        InvalidateAccountCache(account.Username, account.Id);
        return await _db.DeleteAsync(account);
    }

    public async Task DeleteAllAccountDrivesAsync()
    {
        await InitializeDb();
        var drives = await GetAccountDrivesAsync();

        foreach (var drive in drives)
        {
            await _driveService.DeleteDriveAsync(drive);
        }
    }

    public async Task<int> UpdateAllAccountDrivesAsync(
        string username,
        string password,
        string ipAddress)
    {
        await InitializeDb();
        var drives = await GetAccountDrivesAsync();
        var updatedDrives = new List<Drive>();
        
        foreach (var drive in drives)
        {
            drive.UserName = username;
            drive.Password = password;
            drive.IpAddress = ipAddress;

            updatedDrives.Add(drive);
        }

        return await _driveService.UpdateAllDrivesAsync(updatedDrives);
    }

    private void InvalidateAccountCache(string username, int id)
    {
        if (string.IsNullOrWhiteSpace(username) is false)
        {
            _cache.Remove(GetUsernameCacheKey(username));
        }

        _cache.Remove(CacheNamePrefix + id);
    }

    private static string GetUsernameCacheKey(string username)
    {
        return $"{CacheNamePrefix}Username_{username.ToUpperInvariant()}";
    }

    private async Task<string> FetchPasswordAsync()
    {
        var password = await _passwordGenerator.FetchPasswordAsync();
        return password;
    }

    private static string GetDbPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);
    }
}
