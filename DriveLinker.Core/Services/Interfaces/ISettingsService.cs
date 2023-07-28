namespace DriveLinker.Core.Services.Interfaces;
public interface ISettingsService
{
    Task<int> CreateSettingsAsync(Settings settings);
    Task<int> DeleteSettingsAsync(Settings settings);
    Task<Settings> GetAccountSettingsAsync(int accountId);
    Task<int> SetSettingsAsync(Settings settings);
    Task<int> UpdateSettingsAsync(Settings settings);
}