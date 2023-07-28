﻿namespace DriveLinker.Core.Services;
public class DriveService : IDriveService
{
    private const string CacheName = nameof(DriveService);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string DbName = "Drive.db4";
    private readonly List<DriveInfo> _driveInfos = DriveInfo.GetDrives().ToList();
    private readonly IMemoryCache _cache;
    private readonly IAesEncryption _encryption;
    private SQLiteAsyncConnection _db;

    public DriveService(
        IMemoryCache cache,
        IAesEncryption encryption)
    {
        _cache = cache;
        _encryption = encryption;
    }

    private async Task InitializeDb()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = GetDbPath();
        string password = await FetchPasswordAsync();

        var options = new SQLiteConnectionString(dbPath, true, key: password);

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
            output = await _db.Table<Drive>().Where(d => d.AccountId == accountId).ToListAsync();
            var decryptedDrives = await output.SelectAsync(DecryptDrive);

            _cache.Set(key, decryptedDrives.ToList());
        }

        return output;
    }

    public async Task<Drive> GetDriveAsync(int id)
    {
        await InitializeDb();

        var output = _cache.Get<Drive>(id);
        if (output is null)
        {
            var drive = await _db.FindAsync<Drive>(id);
            output = await DecryptDrive(drive);

            _cache.Set(id, output);
        }

        return output;
    }

    public async Task<int> CreateDriveAsync(Drive drive)
    {
        await InitializeDb();

        RemoveCache(drive.AccountId);

        drive = await EncryptDrive(drive);
        return await _db.InsertAsync(drive);
    }

    public async Task<int> UpdateDriveAsync(Drive drive)
    {
        await InitializeDb();
        RemoveCache(drive.AccountId);

        drive = await EncryptDrive(drive);
        return await _db.UpdateAsync(drive);
    }

    public async Task<int> DeleteDriveAsync(Drive drive)
    {
        await InitializeDb();

        RemoveCache(drive.AccountId);
        return await _db.DeleteAsync(drive);
    }

    private async Task<Drive> EncryptDrive(Drive drive)
    {
        drive.Key = _encryption.GetKey();
        drive.Iv = _encryption.GetIV();

        drive.Password = await _encryption.AesEncryptAsync(drive.Password);
        drive.IpAddress = await _encryption.AesEncryptAsync(drive.IpAddress);
        drive.UserName = await _encryption.AesEncryptAsync(drive.UserName);

        return drive;
    }

    private async Task<Drive> DecryptDrive(Drive drive)
    {
        string key = drive.Key;
        string iv = drive.Iv;

        drive.Password = await _encryption.AesDecryptAsync(drive.Password, key, iv);
        drive.IpAddress = await _encryption.AesDecryptAsync(drive.IpAddress, key, iv);
        drive.UserName = await _encryption.AesDecryptAsync(drive.UserName, key, iv);

        drive.DriveInfo = GetDriveInfo(drive);

        return drive;
    }

    private DriveInfo GetDriveInfo(Drive drive)
    {
        var driveInfo = _driveInfos.FirstOrDefault(d => d.VolumeLabel
            .Contains(drive.DriveName, StringComparison.InvariantCultureIgnoreCase) && 
            d.RootDirectory.Name.Contains(drive.Letter, StringComparison.InvariantCultureIgnoreCase));

        return driveInfo;
    }

    private static async Task<string> FetchPasswordAsync()
    {
        var password = await PasswordGenerator.FetchPasswordAsync();
        return password;
    }

    private static string GetDbPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);
    }

    private void RemoveCache(int accountId)
    {
        string key = CacheNamePrefix + accountId;
        _cache.Remove(key);
    }
}
