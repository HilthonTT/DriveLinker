namespace DriveLinker.Core.Encryption.Interfaces;

public interface IRecoveryKeyGenerator
{
    /// <summary>Creates a new set of recovery keys for <paramref name="account"/>, replacing any existing set.</summary>
    Task<List<string>> GenerateRecoveryKeysAsync(Account account, int keyLength = 12);

    /// <summary>Returns the stored recovery keys for <paramref name="account"/> (empty if none were generated).</summary>
    Task<List<string>> GetRecoveryKeysAsync(Account account);

    /// <summary>Removes every recovery key stored for <paramref name="account"/>.</summary>
    void DeleteRecoveryKeys(Account account);
}
