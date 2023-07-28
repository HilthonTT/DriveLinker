namespace DriveLinker.ViewModels.Main;
public partial class LanguageViewModel : ObservableObject
{
    private readonly ILanguageDictionary _language;
    private Dictionary<Keyword, string> _keywords;

    public LanguageViewModel(ILanguageDictionary language)
    {
        _language = language;
        InitializeDictionary();      
    }

    [RelayCommand]
    public void InitializeDictionary()
    {
        _keywords = _language.GetDictionary();
        ChangeKeyWordData();
    }

    private void ChangeKeyWordData()
    {
        Countdown = _keywords[Keyword.Countdown];
        CreateDriveLabel = _keywords[Keyword.CreateADrive];
        SettingsLabel = _keywords[Keyword.Settings];
        SearchLabel = _keywords[Keyword.Search];
        RecentlyAddedLabel = _keywords[Keyword.RecentlyAdded];
        DriveListingLabel = _keywords[Keyword.DriveListing];
        LinkDrivesLabel = _keywords[Keyword.LinkDrives];
        UnlinkDrivesLabel = _keywords[Keyword.UnlinkDrives];
        LetterLabel = _keywords[Keyword.Letter];
        LetterDescLabel = _keywords[Keyword.LetterDesc];
        IpAddressLabel = _keywords[Keyword.IpAddress];
        IpAddressDescLabel = _keywords[Keyword.IpAddressDesc];
        DriveNameLabel = _keywords[Keyword.DriveName];
        DriveNameDescLabel = _keywords[Keyword.DriveNameDesc];
        PasswordLabel = _keywords[Keyword.Password];
        PasswordDescLabel = _keywords[Keyword.PasswordDesc];
        UserNameLabel = _keywords[Keyword.UserName];
        UserNameDescLabel = _keywords[Keyword.UserNameDesc];
        DateCreatedLabel = _keywords[Keyword.DateCreated];
        CreateLabel = _keywords[Keyword.Create];
        AutoLinkDrivesLabel = _keywords[Keyword.AutoLinkDrives];
        AutoMinimizeLabel = _keywords[Keyword.AutoMinimize];
        ClearOnlyLetterDriveNameLabel = _keywords[Keyword.ClearOnlyLetterDriveName];
        SaveLabel = _keywords[Keyword.Save];
        LanguageLabel = _keywords[Keyword.Language];
        EnglishLabel = _keywords[Keyword.English];
        FrenchLabel = _keywords[Keyword.French];
        GermanLabel = _keywords[Keyword.German];
        IndonesianLabel = _keywords[Keyword.Indonesian];
        DontHaveAnAccountLabel = _keywords[Keyword.DontHaveAnAccount];
        ForgotPasswordLabel = _keywords[Keyword.ForgotMyPassword];
        LoginLabel = _keywords[Keyword.Login];
    }

    [ObservableProperty]
    private string _countdown;

    [ObservableProperty]
    private string _createDriveLabel;

    [ObservableProperty]
    private string _settingsLabel;

    [ObservableProperty]
    private string _searchLabel;

    [ObservableProperty]
    private string _recentlyAddedLabel;

    [ObservableProperty]
    private string _driveListingLabel;

    [ObservableProperty]
    private string _linkDrivesLabel;

    [ObservableProperty]
    private string _unlinkDrivesLabel;

    [ObservableProperty]
    private string _letterLabel;

    [ObservableProperty]
    private string _letterDescLabel;

    [ObservableProperty]
    private string _ipAddressLabel;

    [ObservableProperty]
    private string _ipAddressDescLabel;

    [ObservableProperty]
    private string _driveNameLabel;

    [ObservableProperty]
    private string _driveNameDescLabel;

    [ObservableProperty]
    private string _passwordLabel;

    [ObservableProperty]
    private string _passwordDescLabel;

    [ObservableProperty]
    private string _userNameLabel;

    [ObservableProperty]
    private string _userNameDescLabel;

    [ObservableProperty]
    private string _dateCreatedLabel;

    [ObservableProperty]
    private string _createLabel;

    [ObservableProperty]
    private string _autoLinkDrivesLabel;

    [ObservableProperty]
    private string _autoMinimizeLabel;

    [ObservableProperty]
    private string _clearOnlyLetterDriveNameLabel;

    [ObservableProperty]
    private string _saveLabel;

    [ObservableProperty]
    private string _languageLabel;

    [ObservableProperty]
    private string _englishLabel;

    [ObservableProperty]
    private string _frenchLabel;

    [ObservableProperty]
    private string _germanLabel;

    [ObservableProperty]
    private string _indonesianLabel;

    [ObservableProperty]
    private string _dontHaveAnAccountLabel;

    [ObservableProperty]
    private string _forgotPasswordLabel;

    [ObservableProperty]
    private string _loginLabel;
}
