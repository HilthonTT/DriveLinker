namespace DriveLinker.Core.Encryption;

/// <summary>
/// Provides the SQLCipher database password. It is generated once with a CSPRNG, stored in
/// <see cref="SecureStorage"/>, and reused for every database afterwards.
/// </summary>
public class PasswordGenerator : IPasswordGenerator
{
    private const string Key = nameof(PasswordGenerator);
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";
    private const int PasswordLength = 15;
    private readonly IMemoryCache _cache;

    public PasswordGenerator(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<string> FetchPasswordAsync()
    {
        string output = _cache.Get<string>(Key);
        if (output is null)
        {
            output = await SecureStorage.GetAsync(Key);
            if (string.IsNullOrWhiteSpace(output))
            {
                output = RandomNumberGenerator.GetString(ValidCharacters, PasswordLength);
                await SecureStorage.SetAsync(Key, output);
            }

            _cache.Set(Key, output);
        }

        return output;
    }
}
