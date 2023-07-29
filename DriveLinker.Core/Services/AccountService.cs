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
    private SQLiteAsyncConnection _db;

    public AccountService(
        IMemoryCache cache,
        ISettingsService settingsService,
        IDriveService driveService,
        IPasswordGenerator passwordGenerator)
    {
        _cache = cache;
        _settingsService = settingsService;
        _driveService = driveService;
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
        await _db.CreateTableAsync<Account>();
    }

    public async Task<Account> GetAccountAsync(int id)
    {
        await InitializeDb();

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
        await InitializeDb();

        string key = CacheNamePrefix + username;

        var output = _cache.Get<Account>(key);
        if (output is null)
        {
            var accounts = await _db.Table<Account>().ToListAsync();

            output = accounts
                .FirstOrDefault(a => a.Username
                .Equals(username, StringComparison.InvariantCultureIgnoreCase));

            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<List<Drive>> GetAccountDrivesAsync(int accountId)
    {
        await InitializeDb();

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
        await InitializeDb();

        string key = $"{nameof(SettingsService)}_{CacheNamePrefix}{accountId}";

        var output = _cache.Get<Settings>(key);
        if (output is null)
        {
            output = await _settingsService.GetAccountSettingsAsync(accountId);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        await InitializeDb();

        await _db.InsertAsync(account);

        return account;
    }

    public async Task<int> UpdateAccountAsync(Account account)
    {
        await InitializeDb();

        return await _db.UpdateAsync(account);
    }

    public async Task<int> DeleteAccountAsync(Account account)
    {
        await InitializeDb();

        var settings = await _settingsService.GetAccountSettingsAsync(account.Id);
        await _settingsService.DeleteSettingsAsync(settings);

        var drives = await _driveService.GetAllAccountDrivesAsync(account.Id);
        foreach (var drive in drives)
        {
            await _driveService.DeleteDriveAsync(drive);
        }

        return await _db.DeleteAsync(account);
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
