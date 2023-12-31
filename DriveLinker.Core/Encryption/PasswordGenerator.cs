﻿namespace DriveLinker.Core.Encryption;
public class PasswordGenerator : IPasswordGenerator
{
    private const string Key = nameof(PasswordGenerator);
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";
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
                output = await GeneratePassword();
            }

            _cache.Set(Key, output);
        }

        return output;
    }

    private static async Task<string> GeneratePassword()
    {
        int length = 15;

        byte[] randomBytes = new byte[length];
        char[] password = new char[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < length; i++)
        {
            int index = randomBytes[i] % ValidCharacters.Length;
            password[i] = ValidCharacters[index];
        }

        await SecureStorage.SetAsync(Key, new(password));
        return new string(password);
    }
}
