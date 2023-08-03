namespace DriveLinker.Core.Encryption.Interfaces;

public interface IAuthentication
{
    Task<string> ChangePasswordAsync(string username, string newPassword);
    Task<string> ChangeUsernameAsync(string username, string newUsername);
    Account GetAccount();
    Task LogoutAsync();
    Task<VerifiedAccount> VerifyPasswordAsync(string username, string password);
}