namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAesEncryption
{
    Task<string> AesDecryptAsync(string cipherText, string key = null, string iv = null);
    Task<string> AesEncryptAsync(string plainText);
    Task<string> ComputeSha512Hash(string plainText);
    string GetIV();
    string GetKey();
}