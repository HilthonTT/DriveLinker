namespace DriveLinker.Core.Services.Interfaces;
public interface IDriveService
{
    Task<int> CreateDriveAsync(Drive drive);
    void DeleteDb();
    Task<int> DeleteDriveAsync(Drive drive);
    Task<List<Drive>> GetAllAccountDrivesAsync(int accountId);
    Task<List<Drive>> GetAllDrivesAsync();
    Task<Drive> GetDriveAsync(int id);
    Task<int> UpdateDriveAsync(Drive drive);
}