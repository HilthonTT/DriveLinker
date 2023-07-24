namespace DriveLinker.ViewModels;
public partial class LanguageViewModel : ObservableObject
{
    private readonly ILanguageDictionary _language;

    public LanguageViewModel(ILanguageDictionary language)
    {
        _language = language;
    }

    private Dictionary<Keyword, string> GetDictionary()
    {
        var dictionary = _language.GetDictionary();
        return dictionary;
    }

    public string Countdown => GetDictionary()[Keyword.Countdown];
    public string CreateDriveLabel => GetDictionary()[Keyword.CreateADrive];
    public string SettingsLabel => GetDictionary()[Keyword.Settings];
    public string SearchLabel => GetDictionary()[Keyword.Search];
    public string RecentlyAddedLabel => GetDictionary()[Keyword.RecentlyAdded];
    public string DriveListingLabel => GetDictionary()[Keyword.DriveListing];
    public string LinkDrivesLabel => GetDictionary()[Keyword.LinkDrives];
    public string UnlinkDrivesLabel => GetDictionary()[Keyword.UnlinkDrives];
    public string LetterLabel => GetDictionary()[Keyword.Letter];
    public string LetterDescLabel => GetDictionary()[Keyword.LetterDesc];
    public string IpAddressLabel => GetDictionary()[Keyword.IpAddress];
    public string IpAddressDescLabel => GetDictionary()[Keyword.IpAddressDesc];
    public string DriveNameLabel => GetDictionary()[Keyword.DriveName];
    public string DriveNameDescLabel => GetDictionary()[Keyword.DriveNameDesc];
    public string PasswordLabel => GetDictionary()[Keyword.Password];
    public string PasswordDescLabel => GetDictionary()[Keyword.PasswordDesc];
    public string UserNameLabel => GetDictionary()[Keyword.UserName];
    public string UserNameDescLabel => GetDictionary()[Keyword.UserNameDesc];
    public string DateCreatedLabel => GetDictionary()[Keyword.DateCreated];
    public string CreateLabel => GetDictionary()[Keyword.Create];
    public string AutoLinkDrivesLabel => GetDictionary()[Keyword.AutoLinkDrives];
    public string AutoMinimizeLabel => GetDictionary()[Keyword.AutoMinimize];
    public string ClearOnlyLetterDriveNameLabel => GetDictionary()[Keyword.ClearOnlyLetterDriveName];
    public string SaveLabel => GetDictionary()[Keyword.Save];
    public string LanguageLabel => GetDictionary()[Keyword.Language];
    public string EnglishLabel => GetDictionary()[Keyword.English];
    public string FrenchLabel => GetDictionary()[Keyword.French];
    public string GermanLabel => GetDictionary()[Keyword.German];
    public string IndonesianLabel => GetDictionary()[Keyword.Indonesian];
}
