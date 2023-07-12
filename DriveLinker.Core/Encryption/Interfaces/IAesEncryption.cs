namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAesEncryption
{
    Task<string> DecryptAsync(string cipherText, string key = null, string iv = null);
    Task<string> EncryptAsync(string plainText);
    string GetIV();
    string GetKey();
}