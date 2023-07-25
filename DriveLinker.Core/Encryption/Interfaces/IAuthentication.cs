namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAuthentication
{
    Task<string> FetchPasswordAsync();
    Task<string> ResetPasswordAsync(string newPassword);
    Task<string> SetPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password);
}