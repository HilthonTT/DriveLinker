namespace DriveLinker.Core.Services;

/// <summary>
/// Stores drives in the SQLCipher <c>Drive.db4</c> database. The password, IP address and username of
/// every drive are additionally AES-encrypted at rest; callers always receive decrypted copies.
/// </summary>
public class DriveService : IDriveService
{
    private const string CacheName = nameof(DriveService);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string DriveCacheNamePrefix = $"{CacheName}_Drive_";
    private const string DbName = "Drive.db4";
    private readonly IMemoryCache _cache;
    private readonly IEncryption _encryption;
    private readonly IPasswordGenerator _passwordGenerator;
    private SQLiteAsyncConnection _db;

    public DriveService(
        IMemoryCache cache,
        IEncryption encryption,
        IPasswordGenerator passwordGenerator)
    {
        _cache = cache;
        _encryption = encryption;
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
        await _db.CreateTableAsync<Drive>();
    }

    public async Task<List<Drive>> GetAllAccountDrivesAsync(int accountId)
    {
        await InitializeDb();

        string key = CacheNamePrefix + accountId;
        var output = _cache.Get<List<Drive>>(key);
        if (output is null)
        {
            var drives = await _db.Table<Drive>().Where(d => d.AccountId == accountId).ToListAsync();
            output = (await drives.SelectAsync(DecryptDrive)).ToList();

            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<Drive> GetDriveAsync(int id)
    {
        await InitializeDb();

        string key = DriveCacheNamePrefix + id;
        var output = _cache.Get<Drive>(key);
        if (output is null)
        {
            var drive = await _db.FindAsync<Drive>(id);
            if (drive is null)
            {
                return null;
            }

            output = await DecryptDrive(drive);
            _cache.Set(key, output);
        }

        return output;
    }

    public async Task<int> CreateDriveAsync(Drive drive)
    {
        await InitializeDb();

        RemoveCache(drive);

        var encryptedDrive = await EncryptDrive(drive);
        int rows = await _db.InsertAsync(encryptedDrive);

        drive.Id = encryptedDrive.Id;
        return rows;
    }

    public async Task<int> CreateAllDrivesAsync(List<Drive> drives)
    {
        if (drives is null || drives.Count == 0)
        {
            return 0;
        }

        await InitializeDb();

        var encryptedDrives = new List<Drive>();
        foreach (var drive in drives)
        {
            RemoveCache(drive);
            encryptedDrives.Add(await EncryptDrive(drive));
        }

        return await _db.InsertAllAsync(encryptedDrives, true);
    }

    public async Task<int> UpdateDriveAsync(Drive drive)
    {
        await InitializeDb();

        RemoveCache(drive);

        var encryptedDrive = await EncryptDrive(drive);
        return await _db.UpdateAsync(encryptedDrive);
    }

    public async Task<int> UpdateAllDrivesAsync(List<Drive> drives)
    {
        if (drives is null || drives.Count == 0)
        {
            return 0;
        }

        await InitializeDb();

        var encryptedDrives = new List<Drive>();
        foreach (var drive in drives)
        {
            RemoveCache(drive);
            encryptedDrives.Add(await EncryptDrive(drive));
        }

        return await _db.UpdateAllAsync(encryptedDrives, true);
    }

    public async Task<int> DeleteDriveAsync(Drive drive)
    {
        await InitializeDb();

        RemoveCache(drive);
        return await _db.DeleteAsync(drive);
    }

    /// <summary>
    /// Returns an encrypted <b>copy</b> of <paramref name="drive"/> with a fresh key and IV.
    /// The original instance is left untouched because it is usually bound to the UI and used
    /// to run <c>net use</c>; encrypting it in place would replace the visible credentials
    /// with ciphertext.
    /// </summary>
    private async Task<Drive> EncryptDrive(Drive drive)
    {
        var (key, iv) = _encryption.GenerateKeyAndIv();

        return new Drive
        {
            Id = drive.Id,
            AccountId = drive.AccountId,
            Letter = drive.Letter,
            DriveName = drive.DriveName,
            ButtonColor = drive.ButtonColor,
            DriveType = drive.DriveType,
            DriveFormat = drive.DriveFormat,
            DateCreated = drive.DateCreated,
            Key = key,
            Iv = iv,
            Password = await _encryption.AesEncryptAsync(drive.Password, key, iv),
            IpAddress = await _encryption.AesEncryptAsync(drive.IpAddress, key, iv),
            UserName = await _encryption.AesEncryptAsync(drive.UserName, key, iv),
        };
    }

    private async Task<Drive> DecryptDrive(Drive drive)
    {
        string key = drive.Key;
        string iv = drive.Iv;

        drive.Password = await _encryption.AesDecryptAsync(drive.Password, key, iv);
        drive.IpAddress = await _encryption.AesDecryptAsync(drive.IpAddress, key, iv);
        drive.UserName = await _encryption.AesDecryptAsync(drive.UserName, key, iv);

        return drive;
    }

    private async Task<string> FetchPasswordAsync()
    {
        return await _passwordGenerator.FetchPasswordAsync();
    }

    private static string GetDbPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);
    }

    private void RemoveCache(Drive drive)
    {
        _cache.Remove(CacheNamePrefix + drive.AccountId);
        _cache.Remove(DriveCacheNamePrefix + drive.Id);
    }
}
