namespace DriveLinker.Core.Languages;
public class GermanDictionary : IGermanDictionary
{
    private const string CacheName = nameof(GermanDictionary);
    private readonly IMemoryCache _cache;

    public GermanDictionary(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<Keyword, string> GetGermanDictionary()
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
            { Keyword.CreateADrive, "Ein Laufwerk erstellen" },
            { Keyword.Settings, "Einstellungen" },
            { Keyword.Search, "Suche" },
            { Keyword.RecentlyAdded, "Zuletzt hinzugefügte Laufwerke" },
            { Keyword.DriveListing, "Laufwerksliste" },
            { Keyword.LinkDrives, "Laufwerke verbinden" },
            { Keyword.UnlinkDrives, "Laufwerke trennen" },
            { Keyword.Letter, "Buchstabe" },
            { Keyword.LetterDesc, "Geben Sie den Buchstaben Ihres Laufwerks ein." },
            { Keyword.IpAddress, "IP-Adresse" },
            { Keyword.IpAddressDesc, "Geben Sie die IP-Adresse Ihres Laufwerks ein." },
            { Keyword.DriveName, "Laufwerksname" },
            { Keyword.DriveNameDesc, "Geben Sie den Namen Ihres Laufwerks ein." },
            { Keyword.Password, "Passwort" },
            { Keyword.PasswordDesc, "Geben Sie das Passwort Ihres Laufwerks ein." },
            { Keyword.UserName, "Benutzername" },
            { Keyword.UserNameDesc, "Geben Sie den Benutzernamen Ihres Laufwerks ein." },
            { Keyword.DateCreated, "Erstellungsdatum" },
            { Keyword.Create, "Erstellen" },
            { Keyword.AutoLinkDrives, "Laufwerke beim Start automatisch verbinden" },
            { Keyword.AutoMinimize, "Anwendung beim Start automatisch minimieren" },
            { Keyword.ClearOnlyLetterDriveName, "Nur Laufwerksbuchstaben / Laufwerksname löschen" },
            { Keyword.Save, "Speichern" },
            { Keyword.Language, "Sprache" },
            { Keyword.English, "Englisch" },
            { Keyword.French, "Französisch" },
            { Keyword.German, "Deutsch" },
            { Keyword.Indonesian, "Indonesisch" },
            { Keyword.DontHaveAnAccount, "Sie haben noch keinen Account?" },
            { Keyword.ForgotMyPassword, "Passwort vergessen" },
            { Keyword.Login, "Einloggen" },
        };
    }
}
