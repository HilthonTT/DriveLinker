﻿namespace DriveLinker.Core.Services.Interfaces;
public interface IDriveService
{
    Task<int> CreateDriveAsync(Drive drive);
    Task DeleteAllAsync();
    Task<int> DeleteDriveAsync(Drive drive);
    Task<List<Drive>> GetAllDrivesAsync();
    Task<Drive> GetDriveAsync(int id);
    Task<int> UpdateDriveAsync(Drive drive);
}