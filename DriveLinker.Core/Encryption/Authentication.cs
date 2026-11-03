namespace DriveLinker.Core.Encryption;

/// <summary>
/// App-level login. Each account's password is kept only as a SHA-512 hash in
/// <see cref="SecureStorage"/>, under a key derived from the account's stored username.
/// </summary>
public class Authentication : IAuthentication
{
    private const string Key = nameof(Authentication);
    private readonly IAccount _account;
    private readonly IEncryption _encryption;
    private readonly IAccountService _accountService;
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public Authentication(
        IAccount account,
        IEncryption encryption,
        IAccountService accountService,
        IRecoveryKeyGenerator recoveryKeyGenerator)
    {
        _account = account;
        _encryption = encryption;
        _accountService = accountService;
        _recoveryKeyGenerator = recoveryKeyGenerator;
    }

    public async Task LogoutAsync()
    {
        ClearCurrentAccount();

        await Shell.Current.Navigation.PopToRootAsync(true);
    }

    public Account GetAccount()
    {
        return (Account)_account;
    }

    public async Task DeleteAccountAsync()
    {
        var account = new Account { Id = _account.Id, Username = _account.Username };

        SecureStorage.Remove(GetKey(account.Username));
        _recoveryKeyGenerator.DeleteRecoveryKeys(account);

        await _accountService.DeleteAccountAsync(account);

        ClearCurrentAccount();
        await Shell.Current.Navigation.PopToRootAsync(true);
    }

    public async Task<string> ChangePasswordAsync(string username, string newPassword)
    {
        string key = GetKey(username);

        string newHashedPassword = await _encryption.ComputeSha512Hash(newPassword);
        await SecureStorage.SetAsync(key, newHashedPassword);

        return newHashedPassword;
    }

    public async Task<string> ChangeUsernameAsync(string username, string newUsername)
    {
        if (username == newUsername)
        {
            return newUsername;
        }

        string oldKey = GetKey(username);
        string hashedPassword = await SecureStorage.GetAsync(oldKey);

        // Write the new entry before removing the old one so a failure can't leave the
        // account without a password hash.
        if (hashedPassword is not null)
        {
            await SecureStorage.SetAsync(GetKey(newUsername), hashedPassword);
        }

        SecureStorage.Remove(oldKey);

        _account.Username = newUsername;
        await _accountService.UpdateAccountAsync((Account)_account, previousUsername: username);

        return newUsername;
    }

    public async Task<VerifiedAccount> VerifyPasswordAsync(string username, string password)
    {
        var account = await _accountService.GetAccountByUsernameAsync(username);
        var dirtyAccount = new VerifiedAccount() { Account = account, IsCorrect = false };

        if (account is null || string.IsNullOrEmpty(password))
        {
            return dirtyAccount;
        }

        // Usernames are matched case-insensitively, so the hash must be looked up with the
        // username as it was stored, not as the user typed it.
        string storedPassword = await SecureStorage.GetAsync(GetKey(account.Username));
        if (storedPassword is null)
        {
            return dirtyAccount;
        }

        string hashedPassword = await _encryption.ComputeSha512Hash(password);

        bool isCorrect = CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(hashedPassword),
            Encoding.ASCII.GetBytes(storedPassword));

        return new() { Account = account, IsCorrect = isCorrect };
    }

    private void ClearCurrentAccount()
    {
        _account.Id = 0;
        _account.Username = "";
    }

    private static string GetKey(string username)
    {
        return $"{Key}_{username}";
    }
}
