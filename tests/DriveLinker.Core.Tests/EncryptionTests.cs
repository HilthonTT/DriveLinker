using System.Security.Cryptography;
using System.Text;
using DriveLinker.Core.Encryption;

namespace DriveLinker.Core.Tests;

public class EncryptionTests
{
    private readonly Encryption.Encryption _encryption = new();

    [Theory]
    [InlineData("hunter2")]
    [InlineData("pass word with spaces & \"quotes\"")]
    [InlineData("ünïcödé-🔑")]
    [InlineData("")]
    public async Task Encrypt_then_decrypt_round_trips(string plainText)
    {
        var (key, iv) = _encryption.GenerateKeyAndIv();

        string cipherText = await _encryption.AesEncryptAsync(plainText, key, iv);
        string decrypted = await _encryption.AesDecryptAsync(cipherText, key, iv);

        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public async Task Encrypt_returns_null_for_null_input()
    {
        var (key, iv) = _encryption.GenerateKeyAndIv();

        Assert.Null(await _encryption.AesEncryptAsync(null, key, iv));
    }

    [Fact]
    public void GenerateKeyAndIv_returns_fresh_values_of_the_right_size()
    {
        var first = _encryption.GenerateKeyAndIv();
        var second = _encryption.GenerateKeyAndIv();

        Assert.NotEqual(first.Key, second.Key);
        Assert.NotEqual(first.Iv, second.Iv);
        Assert.Equal(32, Convert.FromBase64String(first.Key).Length); // AES-256
        Assert.Equal(16, Convert.FromBase64String(first.Iv).Length);
    }

    [Fact]
    public async Task Decrypts_data_written_by_earlier_versions()
    {
        // Earlier versions encrypted with a default Aes instance (AES-CBC, PKCS7) and stored
        // Base64 key, IV and ciphertext. Existing databases must keep decrypting.
        using var legacyAes = Aes.Create();
        legacyAes.GenerateKey();
        legacyAes.GenerateIV();
        byte[] legacyCipher = legacyAes.EncryptCbc(Encoding.UTF8.GetBytes("legacy-secret"), legacyAes.IV);

        string decrypted = await _encryption.AesDecryptAsync(
            Convert.ToBase64String(legacyCipher),
            Convert.ToBase64String(legacyAes.Key),
            Convert.ToBase64String(legacyAes.IV));

        Assert.Equal("legacy-secret", decrypted);
    }

    [Fact]
    public async Task Sha512_hash_matches_the_format_stored_by_earlier_versions()
    {
        // Login hashes already in SecureStorage are lower-case hex built byte-by-byte with "x2".
        byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes("correct horse battery staple"));
        var legacy = new StringBuilder();
        foreach (byte b in hash)
        {
            legacy.Append(b.ToString("x2"));
        }

        string actual = await _encryption.ComputeSha512Hash("correct horse battery staple");

        Assert.Equal(legacy.ToString(), actual);
    }

    [Fact]
    public async Task Concurrent_use_with_different_keys_is_safe()
    {
        // The previous implementation shared one mutable Aes instance between all callers.
        var inputs = Enumerable.Range(0, 200).Select(i => $"secret-{i}").ToList();

        var results = await Task.WhenAll(inputs.Select(async text =>
        {
            var (key, iv) = _encryption.GenerateKeyAndIv();
            string cipher = await _encryption.AesEncryptAsync(text, key, iv);
            return await _encryption.AesDecryptAsync(cipher, key, iv);
        }));

        Assert.Equal(inputs, results);
    }
}
