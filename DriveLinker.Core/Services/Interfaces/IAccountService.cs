namespace DriveLinker.Core.Services.Interfaces;

public interface IAccountService
{
    Task<int> CreateAccountAsync(Account account);
    Task<int> DeleteAccountAsync(Account account);
    Task<Account> GetAccountAsync(int id);
    Task<int> UpdateAccountAsync(Account account);
}