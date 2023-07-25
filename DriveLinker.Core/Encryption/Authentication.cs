namespace DriveLinker.Core.Encryption;
public class Authentication : IAuthentication
{
    private const string Key = nameof(Authentication);
    private readonly IDriveService _driveService;

    public Authentication(IDriveService driveService)
    {
        _driveService = driveService;
    }

    public async Task<string> SetPasswordAsync(string password)
    {
        string hashedPassword = await ComputeSha512Hash(password);
        await SecureStorage.SetAsync(Key, hashedPassword);

        return hashedPassword;
    }

    public async Task<bool> VerifyPasswordAsync(string password)
    {
        string hashedPassword = await ComputeSha512Hash(password);
        string storedPassword = await SecureStorage.GetAsync(Key);

        if (storedPassword is null)
        {
            return false;
        }

        return hashedPassword == storedPassword;
    }

    public async Task<string> ChangePasswordAsync(string newPassword)
    {
        bool isRemoved = SecureStorage.Remove(Key);

        if (isRemoved)
        {
            string newHashedPassword = await ComputeSha512Hash(newPassword);
            await SecureStorage.SetAsync(Key, newHashedPassword);

            return newHashedPassword;
        }

        return "";
    }

    public async Task<string> ResetPasswordAsync(string newPassword)
    {
        bool isRemoved = SecureStorage.Remove(Key);

        if (isRemoved)
        {
            string newHashedPassword = await ComputeSha512Hash(newPassword);
            await SecureStorage.SetAsync(Key, newHashedPassword);

            // Removes all drives to not gain access to it.
            await _driveService.DeleteAllAsync();

            return newHashedPassword;
        }

        return "";
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
}
