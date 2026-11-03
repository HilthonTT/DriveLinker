namespace DriveLinker.Core.Encryption;

/// <summary>
/// Generates and stores the per-account recovery keys used to reset a forgotten login password.
/// </summary>
public class RecoveryKeyGenerator : IRecoveryKeyGenerator
{
    private readonly IMemoryCache _cache;
    private const string KeyPrefix = $"{nameof(RecoveryKeyGenerator)}_";
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int MaxKeyCount = 5;

    public RecoveryKeyGenerator(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<List<string>> GenerateRecoveryKeysAsync(Account account, int keyLength = 12)
    {
        var recoveryKeys = new List<string>();

        for (int i = 0; i < MaxKeyCount; i++)
        {
            // Cryptographically secure, unbiased selection.
            string recoveryKey = RandomNumberGenerator.GetString(Characters, keyLength);
            recoveryKeys.Add(recoveryKey);

            await SecureStorage.SetAsync(GetRecoveryCacheName(account, i), recoveryKey);
        }

        // Without this, GetRecoveryKeysAsync kept returning the previous (cached) set.
        _cache.Set(GetAccountCacheName(account), recoveryKeys);

        return recoveryKeys;
    }

    public async Task<List<string>> GetRecoveryKeysAsync(Account account)
    {
        string accountKey = GetAccountCacheName(account);
        var output = _cache.Get<List<string>>(accountKey);

        if (output is not null)
        {
            return output;
        }

        output = new();
        for (int i = 0; i < MaxKeyCount; i++)
        {
            string recoveryKey = await SecureStorage.GetAsync(GetRecoveryCacheName(account, i));

            if (recoveryKey is not null)
            {
                output.Add(recoveryKey);
            }
        }

        _cache.Set(accountKey, output);

        return output;
    }

    public void DeleteRecoveryKeys(Account account)
    {
        for (int i = 0; i < MaxKeyCount; i++)
        {
            SecureStorage.Remove(GetRecoveryCacheName(account, i));
        }

        _cache.Remove(GetAccountCacheName(account));
    }

    private static string GetRecoveryCacheName(Account account, int number)
    {
        return $"{KeyPrefix}{account.Id}_{number}";
    }

    private static string GetAccountCacheName(Account account)
    {
        return $"{KeyPrefix}{account.Id}";
    }
}
