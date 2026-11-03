namespace DriveLinker.Core.Encryption.Interfaces;

public interface IEncryption
{
    /// <summary>Generates a fresh random AES key and IV, both Base64-encoded.</summary>
    (string Key, string Iv) GenerateKeyAndIv();

    /// <summary>Encrypts <paramref name="plainText"/> and returns Base64 ciphertext. <c>null</c> stays <c>null</c>.</summary>
    Task<string> AesEncryptAsync(string plainText, string key, string iv);

    /// <summary>Decrypts Base64 ciphertext produced by <see cref="AesEncryptAsync"/>. Empty input is returned unchanged.</summary>
    Task<string> AesDecryptAsync(string cipherText, string key, string iv);

    /// <summary>Returns the lower-case hex SHA-512 hash of <paramref name="plainText"/>.</summary>
    Task<string> ComputeSha512Hash(string plainText);
}
