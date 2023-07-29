namespace DriveLinker.Core.Encryption;
public class RecoveryKeyGenerator : IRecoveryKeyGenerator
{
    private static readonly Random _random = new();
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
            string recoveryKey = new(Enumerable.Repeat(Characters, keyLength)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            recoveryKeys.Add(recoveryKey);

            await SaveRecoveryKey(account, recoveryKey, i);
        }
        
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
            string key = GetRecoveryCacheName(account, i);
            string recoveryKey = await SecureStorage.GetAsync(key);

            if (recoveryKey is not null)
            {
                output.Add(recoveryKey);
            }
        }

        _cache.Set(accountKey, output);
        
        return output;
    }

    private static async Task SaveRecoveryKey(Account account, string recoveryKey, int number)
    {
        string key = GetRecoveryCacheName(account, number);
        await SecureStorage.SetAsync(key, recoveryKey);
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
