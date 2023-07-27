namespace DriveLinker.Core.Languages;
public class EnglishDictionary : IEnglishDictionary
{
    private const string CacheName = nameof(EnglishDictionary);
    private readonly IMemoryCache _cache;

    public EnglishDictionary(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<Keyword, string> GetEnglishDictionary()
    {
        var output = _cache.Get<Dictionary<Keyword, string>>(CacheName);
        if (output is null)
        {
            output = GetDictionary();
            _cache.Set(CacheName, output);
        }

        return output;
    }

    private static Dictionary<Keyword, string> GetDictionary()
    {
        return new Dictionary<Keyword, string>()
        {
            { Keyword.Countdown, "Countdown" },
            { Keyword.CreateADrive, "Create A Drive" },
            { Keyword.Settings, "Settings" },
            { Keyword.Search, "Search" },
            { Keyword.RecentlyAdded, "Recently Added Drives" },
            { Keyword.DriveListing, "Drive Listing" },
            { Keyword.LinkDrives, "Link Drives" },
            { Keyword.UnlinkDrives, "Unlink Drives" },
            { Keyword.Letter, "Letter" },
            { Keyword.LetterDesc, "Enter your drive's letter." },
            { Keyword.IpAddress, "IP Address" },
            { Keyword.IpAddressDesc, "Enter your drives's IP Address." },
            { Keyword.DriveName, "Drive Name" },
            { Keyword.DriveNameDesc, "Enter your drive's name." },
            { Keyword.Password, "Password" },
            { Keyword.PasswordDesc, "Enter your drive's password." },
            { Keyword.UserName, "Username" },
            { Keyword.UserNameDesc, "Enter your drive's username." },
            { Keyword.DateCreated, "Date Created" },
            { Keyword.Create, "Create" },
            { Keyword.AutoLinkDrives, "Automatically Link Drives On Launch" },
            { Keyword.AutoMinimize, "Automatically Minimize App" },
            { Keyword.ClearOnlyLetterDriveName, "Clear Only Letter/Drive Name" },
            { Keyword.Save, "Save" },
            { Keyword.Language, "Language" },
            { Keyword.English, "English" },
            { Keyword.French, "French" },
            { Keyword.German, "German" },
            { Keyword.Indonesian, "Indonesian" },
            { Keyword.DontHaveAnAccount, "Don't have an account?" },
            { Keyword.ForgotMyPassword, "Forgot my password" },
            { Keyword.Login, "Login" },
        };
    }
}
