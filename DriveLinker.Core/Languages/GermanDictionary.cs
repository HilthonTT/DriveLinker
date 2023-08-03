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
        return new()
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
            { Keyword.English, "English" },
            { Keyword.French, "Français" },
            { Keyword.German, "Deutsch" },
            { Keyword.Indonesian, "Indonesia" },
            { Keyword.DontHaveAnAccount, "Sie haben noch keinen Account?" },
            { Keyword.ForgotMyPassword, "Passwort vergessen" },
            { Keyword.Login, "Einloggen" },
            { Keyword.RecoveryKey, "Wiederherstellungsschlüssel" },
            { Keyword.RecoveryKeyDesc, "Geben Sie einen Ihrer Wiederherstellungsschlüssel ein" },
            { Keyword.RecoveryKeyHelperText, "Sie können sie verwenden, wenn Sie Ihr Passwort vergessen." },
            { Keyword.Copyclipboard, "In die Zwischenablage kopieren" },
            { Keyword.Register, "Registrieren" },
            { Keyword.Error, "Fehler" },
            { Keyword.Ok, "OK" },
            { Keyword.LetterAndDriveNameTaken, "Laufwerksbuchstabe und Laufwerksname sind bereits vergeben." },
            { Keyword.LetterTaken, "Laufwerksbuchstabe ist bereits vergeben." },
            { Keyword.DriveNameTaken, "Laufwerkname ist bereits vergeben." },
            { Keyword.LetterNotPopulated, "Sie haben das Buchstabenfeld nicht ausgefüllt." },
            { Keyword.EnterALetter, "Bitte geben Sie einen Buchstaben ein." },
            { Keyword.IpAddressNotPopulated, "Sie haben das IP-Adressfeld nicht ausgefüllt." },
            { Keyword.DriveNameNotPopulated, "Sie haben das Feld für den Laufwerknamen nicht ausgefüllt." },
            { Keyword.PasswordNotPopulated, "Sie haben das Passwortfeld nicht ausgefüllt." },
            { Keyword.UserNameNotPopulated, "Sie haben das Benutzernamenfeld nicht ausgefüllt." },
            { Keyword.DeleteDrive, "Laufwerk löschen?" },
            { Keyword.DeleteDriveWarning, "Das Löschen eines Laufwerks ist nicht rückgängig zu machen." },
            { Keyword.Yes, "Ja" },
            { Keyword.No, "Nein" },
            { Keyword.EditLetter, "Buchstaben bearbeiten?" },
            { Keyword.EditLetterDesc, "Was ist Ihr neuer Buchstabe?" },
            { Keyword.EditDriveName, "Laufwerkname bearbeiten?" },
            { Keyword.EditDriveNameDesc, "Was ist Ihr neuer Laufwerkname?" },
            { Keyword.EditIpAddress, "IP-Adresse bearbeiten?" },
            { Keyword.EditIpAddressDesc, "Was ist Ihre neue IP-Adresse?" },
            { Keyword.EditPassword, "Passwort bearbeiten?" },
            { Keyword.EditPasswordDesc, "Was ist Ihr neues Passwort?" },
            { Keyword.EditUsername, "Benutzernamen bearbeiten?" },
            { Keyword.EditUsernameDesc, "Was ist Ihr neuer Benutzername?" },
            { Keyword.Seconds, " Sekunden sein" },
            { Keyword.TimerCountMinWarning, $"Ihr Timer kann nicht weniger als " },
            { Keyword.TimerCountMaxWarning, $"Ihr Timer kann nicht mehr als " },
            { Keyword.HomePage, "Startseite" },
            { Keyword.Logout, "Abmelden" },
            { Keyword.DriveInfo, "Laufwerkinformationen" },
            { Keyword.Stacktrace, "Stapelverfolgung" },
            { Keyword.ErrorCount, "Fehleranzahl: " },
            { Keyword.DeleteAccount, "Konto löschen." },
            { Keyword.DeleteAccountWarning, "Sind Sie sicher, dass Sie Ihr Konto löschen möchten? Dieser Vorgang kann nicht rückgängig gemacht werden." },
            { Keyword.CurrentPassword, "Aktuelles Passwort" },
            { Keyword.WrongRecoveryKey, "Ungültiger Wiederherstellungsschlüssel!" },
            { Keyword.WrongRecoveryKeyDesc, "Der eingegebene Wiederherstellungsschlüssel ist falsch!" },
            { Keyword.UsernameTaken, "Ein Konto mit diesem Benutzernamen existiert bereits." },
            { Keyword.UsernameDoesntExist, "Ihr Benutzername existiert nicht." },
            { Keyword.WrongUsername, "Ungültiger Benutzername!" },
            { Keyword.WrongUsernameDesc, "Ihr Benutzername existiert nicht." },
            { Keyword.WrongPassword, "Ungültiges Passwort!" },
            { Keyword.WrongPasswordDesc, "Das eingegebene Passwort ist falsch." },
            { Keyword.TextCopied, "Text kopiert!" },
            { Keyword.TextCopiedDesc, "Der Text wurde in die Zwischenablage kopiert." },
            { Keyword.RegisterDesc, "Du wirst bekannt sein als: " },
        };
    }
}
