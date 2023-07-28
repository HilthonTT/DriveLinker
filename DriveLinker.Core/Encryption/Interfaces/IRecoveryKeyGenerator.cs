namespace DriveLinker.Core.Encryption.Interfaces;

public interface IRecoveryKeyGenerator
{
    Task<string> GenerateRecoveryKeyAsync(Account account, int keyLength = 12);
    Task<string> GetRecoveryKeyAsync(Account account);
}