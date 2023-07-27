namespace DriveLinker.Core.Encryption;

public interface IAuthentication
{
    Task<string> ChangePasswordAsync(string username, string newPassword);
    Task<string> FetchPasswordAsync();
    Task<string> ResetPasswordAsync(string username, string newPassword);
    Task<string> SetPasswordAsync(string username, string password);
    Task<bool> VerifyPasswordAsync(string username, string password);
}