namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAuthentication
{
    Task<string> ChangePasswordAsync(string username, string newPassword);
    Task<string> ChangeUsernameAsync(string username, string newUsername);
    Task<string> FetchPasswordAsync(string username);
    Account GetAccount();
    Task LogoutAsync();
    Task<string> ResetPasswordAsync(string username, string newPassword);
    Task<string> SetPasswordAsync(string username, string password);
    Task<VerifiedAccount> VerifyPasswordAsync(string username, string password);
}