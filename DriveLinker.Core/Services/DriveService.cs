using DriveLinker.Core.Encryption.Interfaces;
using DriveLinker.Core.Models;
using DriveLinker.Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using SQLite;

namespace DriveLinker.Core.Services;
public class DriveService : IDriveService
{
    private const string CacheName = nameof(DriveService);
    private const string DbName = "Drive.db3";
    private readonly IMemoryCache _cache;
    private readonly IAesEncryption _encryption;
    private SQLiteAsyncConnection _db;

    public DriveService(IMemoryCache cache, IAesEncryption encryption)
    {
        _cache = cache;
        _encryption = encryption;
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
        _db.CreateTableAsync<Drive>();
    }

    public async Task<List<Drive>> GetAllDrivesAsync()
    {
        var output = _cache.Get<List<Drive>>(CacheName);
        if (output is null)
        {
            output = await _db.Table<Drive>().ToListAsync();
            _cache.Set(CacheName, output);
        }

        return output;
    }

    public async Task<Drive> GetDriveAsync(int id)
    {
        var output = _cache.Get<Drive>(id);
        if (output is null)
        {
            output = await _db.FindAsync<Drive>(id);
            _cache.Set(id, output);
        }

        return output;
    }

    public async Task<int> CreateDriveAsync(Drive drive)
    {
        _cache.Remove(CacheName);

        drive = MapEncryptionData(drive);
        return await _db.InsertAsync(drive);
    }

    public async Task<int> UpdateDriveAsync(Drive drive)
    {
        _cache.Remove(CacheName);
        return await _db.UpdateAsync(drive);
    }

    public async Task<int> DeleteDriveAsync(Drive drive)
    {
        _cache.Remove(CacheName);
        return await _db.DeleteAsync(drive);
    }

    private Drive MapEncryptionData(Drive drive)
    {
        drive.Key = _encryption.GetKey();
        drive.Iv = _encryption.GetIV();

        return drive;
    }
}
