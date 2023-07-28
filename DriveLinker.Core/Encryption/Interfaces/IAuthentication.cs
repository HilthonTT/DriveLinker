namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAuthentication
{
    Task<string> ChangePasswordAsync(string username, string newPassword);
    Task<string> FetchPasswordAsync();
    Task<string> ResetPasswordAsync(string username, string newPassword);
    Task<string> SetPasswordAsync(string username, string password);
    Task<VerifiedAccount> VerifyPasswordAsync(string username, string password);
}