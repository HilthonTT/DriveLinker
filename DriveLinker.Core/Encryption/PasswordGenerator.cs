namespace DriveLinker.Core.Encryption;
public static class PasswordGenerator
{
    private const string Key = nameof(PasswordGenerator);
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";

    public static async Task<string> FetchPasswordAsync()
    {
        string password = await SecureStorage.GetAsync(Key);
        if (string.IsNullOrWhiteSpace(password))
        {
            password = await GeneratePassword();
            return password;
        }

        return password;
    }

    private static async Task<string> GeneratePassword()
    {
        int length = 15;

        byte[] randomBytes = new byte[length];
        char[] password = new char[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < length; i++)
        {
            int index = randomBytes[i] % ValidCharacters.Length;
            password[i] = ValidCharacters[index];
        }

        await SecureStorage.SetAsync(Key, new(password));
        return new string(password);
    }
}
