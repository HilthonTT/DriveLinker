namespace DriveLinker.Core.Services.Interfaces;

public interface IAccountService
{
    Task<int> CreateAccountAsync(Account account);
    Task<int> DeleteAccountAsync(Account account);
    Task<Account> GetAccountAsync(int id);
    Task<Account> GetAccountByUsernameAsync(string username);
    Task<List<Drive>> GetAccountDrivesAsync(int accountId);
    Task<Settings> GetAccountSettingsService(int accountId);
    Task<int> UpdateAccountAsync(Account account);
}