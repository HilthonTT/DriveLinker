namespace DriveLinker.Core.Encryption;
public class Authentication : IAuthentication
{
    private const string Key = nameof(Authentication);
    private readonly IDriveService _driveService;
    private readonly IAccountService _accountService;

    public Authentication(
        IDriveService driveService,
        IAccountService accountService)
    {
        _driveService = driveService;
        _accountService = accountService;
    }

    public async Task<string> SetPasswordAsync(string username, string password)
    {
        string hashedPassword = await ComputeSha512Hash(password);

        string key = GetKey(username);
        await SecureStorage.SetAsync(key, hashedPassword);

        return hashedPassword;
    }

    public async Task<string> ChangePasswordAsync(string username, string newPassword)
    {
        string key = GetKey(username);
        SecureStorage.Remove(key);

        string newHashedPassword = await ComputeSha512Hash(newPassword);
        await SecureStorage.SetAsync(key, newHashedPassword);

        return newHashedPassword;
    }

    public async Task<VerifiedAccount> VerifyPasswordAsync(string username, string password)
    {
        var account = await _accountService.GetAccountByUsernameAsync(username);
        var dirtyAccount = new VerifiedAccount() { Account = account, IsCorrect = false };

        string hashedPassword = await ComputeSha512Hash(password);

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

    public async Task<string> ResetPasswordAsync(string username, string newPassword)
    {
        string key = GetKey(username);
        SecureStorage.Remove(key);

        string newHashedPassword = await ComputeSha512Hash(newPassword);
        await SecureStorage.SetAsync(key, newHashedPassword);

        // Delete drives to not gain access to it.
        var account = await _accountService.GetAccountByUsernameAsync(username);
        if (account is not null)
        {
            var drives = await _driveService.GetAllAccountDrivesAsync(account.Id);
            foreach (var drive in drives)
            {
                await _driveService.DeleteDriveAsync(drive);
            }
        }

        return newHashedPassword;
    }

    public async Task<string> FetchPasswordAsync()
    {
        string hashedPassword = await SecureStorage.GetAsync(Key);

        if (hashedPassword is null)
        {
            return "";
        }

        return hashedPassword;
    }

    private static async Task<string> ComputeSha512Hash(string plainText)
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

    private static string GetKey(string username)
    {
        return $"{Key}_{username}";
    }
}
