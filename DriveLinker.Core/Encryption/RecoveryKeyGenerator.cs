namespace DriveLinker.Core.Encryption;
public class RecoveryKeyGenerator : IRecoveryKeyGenerator
{
    private static readonly Random _random = new();
    private readonly IMemoryCache _cache;
    private const string KeyPrefix = $"{nameof(RecoveryKeyGenerator)}_";
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public RecoveryKeyGenerator(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<string> GenerateRecoveryKeyAsync(Account account, int keyLength = 12)
    {
        string recoveryKey = new(Enumerable.Repeat(Characters, keyLength)
            .Select(s => s[_random.Next(s.Length)]).ToArray());

        await SaveRecoveryKey(account, recoveryKey);

        return recoveryKey;
    }

    public async Task<string> GetRecoveryKeyAsync(Account account)
    {
        string key = GetKey(account);
        
        string output = _cache.Get<string>(key);
        if (output is null)
        {
            string recoveryKey = await SecureStorage.GetAsync(key); 

            _cache.Set(key, recoveryKey);
        }

        return output;
    }

    private static async Task SaveRecoveryKey(Account account, string recoveryKey)
    {
        string key = GetKey(account);
        await SecureStorage.SetAsync(key, recoveryKey);
    }

    private static string GetKey(Account account)
    {
        return KeyPrefix + account.Id;
    }
}
