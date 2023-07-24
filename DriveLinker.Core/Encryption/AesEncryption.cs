using System.Security.Cryptography;
using System.Text;
using DriveLinker.Core.Encryption.Interfaces;

namespace DriveLinker.Core.Encryption;
public class AesEncryption : IAesEncryption
{
    private readonly Aes _aes;

    public AesEncryption()
    {
        _aes = Aes.Create();
        _aes.GenerateKey();
        _aes.GenerateIV();
    }

    public async Task<string> AesEncryptAsync(string plainText)
    {
        byte[] encrypted;

        using (ICryptoTransform encryptor = _aes.CreateEncryptor())
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                await csEncrypt.WriteAsync(plainBytes);
                await csEncrypt.FlushFinalBlockAsync();
            }

            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    public async Task<string> AesDecryptAsync(string cipherText, string key = default, string iv = default)
    {
        if (key is not null)
        {
            _aes.Key = Convert.FromBase64String(key);
        }

        if (iv is not null)
        {
            _aes.IV = Convert.FromBase64String(iv);
        }

        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using ICryptoTransform decryptor = _aes.CreateDecryptor();
        byte[] decryptedBytes;

        using (var msDecrypt = new MemoryStream(cipherBytes))
        {
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var ms = new MemoryStream();

            await csDecrypt.CopyToAsync(ms);
            decryptedBytes = ms.ToArray();
        }

        string decrytedText = Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');

        return decrytedText;
    }

    public async Task<string> ComputeSha512Hash(string plainText)
    {
        using SHA512 sha512 = SHA512.Create();
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] hashedBytes;

        using (var inputStream = new MemoryStream(plainTextBytes))
        {
            hashedBytes = await sha512.ComputeHashAsync(inputStream);
        }

        var sb = new StringBuilder();
        foreach (byte b in hashedBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public string GetKey()
    {
        string convertedArray = Convert.ToBase64String(_aes.Key);
        return convertedArray;
    }

    public string GetIV()
    {
        string convertedArray = Convert.ToBase64String(_aes.IV);
        return convertedArray;
    }
}
