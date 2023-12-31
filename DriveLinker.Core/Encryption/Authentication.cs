﻿namespace DriveLinker.Core.Encryption;
public class Authentication : IAuthentication
{
    private const string Key = nameof(Authentication);
    private readonly IAccount _account;
    private readonly IEncryption _encryption;
    private readonly IDriveService _driveService;
    private readonly IAccountService _accountService;

    public Authentication(
        IAccount account,
        IEncryption encryption,
        IDriveService driveService,
        IAccountService accountService)
    {
        _account = account;
        _encryption = encryption;
        _driveService = driveService;
        _accountService = accountService;
    }

    public async Task LogoutAsync()
    {
        _account.Id = 0;
        _account.Username = "";

        await Shell.Current.Navigation.PopToRootAsync(true);
    }

    public Account GetAccount()
    {
        return (Account)_account;
    }

    public async Task DeleteAccountAsync()
    {
        string key = GetKey(_account.Username);
        SecureStorage.Remove(key);

        await _accountService.DeleteAccountAsync(GetAccount());
        await Shell.Current.Navigation.PopToRootAsync(true);
    }

    public async Task<string> ChangePasswordAsync(string username, string newPassword)
    {
        string key = GetKey(username);
        SecureStorage.Remove(key);

        string newHashedPassword = await _encryption.ComputeSha512Hash(newPassword);
        await SecureStorage.SetAsync(key, newHashedPassword);

        return newHashedPassword;
    }

    public async Task<string> ChangeUsernameAsync(string username, string newUsername)
    {
        string key = GetKey(username);
        string hashedPassword = await SecureStorage.GetAsync(key);
        SecureStorage.Remove(key);

        string newKey = GetKey(newUsername);
        await SecureStorage.SetAsync(newKey, hashedPassword);

        _account.Username = newUsername;
        await _accountService.UpdateAccountAsync((Account)_account);

        return newUsername;
    }

    public async Task<VerifiedAccount> VerifyPasswordAsync(string username, string password)
    {
        var account = await _accountService.GetAccountByUsernameAsync(username);
        var dirtyAccount = new VerifiedAccount() { Account = account, IsCorrect = false };

        if (account is null)
        {
            return dirtyAccount;
        }

        string hashedPassword = await _encryption.ComputeSha512Hash(password);

        string key = GetKey(username);
        string storedPassword = await SecureStorage.GetAsync(key);

        if (storedPassword is null)
        {
            return dirtyAccount;
        }

        if (hashedPassword == storedPassword)
        {
            return new() { Account = account, IsCorrect = true };
        }
        else
        {
            return dirtyAccount;
        }
    }

    private static string GetKey(string username)
    {
        return $"{Key}_{username}";
    }
}
