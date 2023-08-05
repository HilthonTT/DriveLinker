namespace DriveLinker.Core.Services.Interfaces;
public interface IDriveService
{
    Task<int> CreateAllDrivesAsync(List<Drive> drives);
    Task<int> CreateDriveAsync(Drive drive);
    Task<int> DeleteDriveAsync(Drive drive);
    Task<List<Drive>> GetAllAccountDrivesAsync(int accountId);
    Task<Drive> GetDriveAsync(int id);
    Task<int> UpdateAllDrivesAsync(List<Drive> drive);
    Task<int> UpdateDriveAsync(Drive drive);
}