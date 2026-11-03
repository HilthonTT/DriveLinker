namespace DriveLinker.Core.Encryption;

/// <summary>
/// AES-CBC encryption for drive credentials and SHA-512 hashing for the login password.
/// </summary>
/// <remarks>
/// The class is stateless: every call builds its own <see cref="Aes"/> instance from the key and IV
/// it is given, so it is safe to use concurrently. Keys and IVs are passed around as Base64 strings
/// because that is how they are persisted on each <see cref="Drive"/> row.
/// </remarks>
public class Encryption : IEncryption
{
    public (string Key, string Iv) GenerateKeyAndIv()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();

        return (Convert.ToBase64String(aes.Key), Convert.ToBase64String(aes.IV));
    }

    public async Task<string> AesEncryptAsync(string plainText, string key, string iv)
    {
        if (plainText is null)
        {
            return null;
        }

        using var aes = CreateAes(key, iv);
        using ICryptoTransform encryptor = aes.CreateEncryptor();

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var msEncrypt = new MemoryStream();
        await using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            await csEncrypt.WriteAsync(plainBytes);
            await csEncrypt.FlushFinalBlockAsync();
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public async Task<string> AesDecryptAsync(string cipherText, string key, string iv)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        using var aes = CreateAes(key, iv);
        using ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using var msDecrypt = new MemoryStream(cipherBytes);
        await using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var output = new MemoryStream();

        await csDecrypt.CopyToAsync(output);

        return Encoding.UTF8.GetString(output.ToArray()).TrimEnd('\0');
    }

    public Task<string> ComputeSha512Hash(string plainText)
    {
        byte[] hashedBytes = SHA512.HashData(Encoding.UTF8.GetBytes(plainText));

        // Lower-case hex, matching the format of hashes stored by earlier versions.
        return Task.FromResult(Convert.ToHexStringLower(hashedBytes));
    }

    private static Aes CreateAes(string key, string iv)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        ArgumentException.ThrowIfNullOrEmpty(iv);

        var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        return aes;
    }
}
