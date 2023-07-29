namespace DriveLinker.Core.Encryption.Interfaces;

public interface IRecoveryKeyGenerator
{
    Task<List<string>> GenerateRecoveryKeysAsync(Account account, int keyLength = 12);
    Task<List<string>> GetRecoveryKeysAsync(Account account);
}