namespace DriveLinker.Core.Services.Interfaces;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(Account account);
    Task<int> DeleteAccountAsync(Account account);
    Task DeleteAllAccountDrivesAsync();
    Task<Account> GetAccountAsync(int id);
    Task<Account> GetAccountByUsernameAsync(string username);
    Task<List<Drive>> GetAccountDrivesAsync();
    Task<Settings> GetAccountSettingsAsync();
    Task<int> UpdateAccountAsync(Account account);
}