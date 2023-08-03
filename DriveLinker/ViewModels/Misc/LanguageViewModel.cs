namespace DriveLinker.ViewModels.Misc;
public partial class LanguageViewModel : ObservableObject
{
    private readonly ILanguageDictionary _language;
    private static Dictionary<Keyword, string> _keywords;

    public LanguageViewModel(ILanguageDictionary language)
    {
        _language = language;
        InitializeDictionary();   
    }

    [RelayCommand]
    public async Task InitializeDictionary()
    {
        _keywords = await _language.GetDictionaryAsync();
        ChangeKeywordData();
    }

    private void ChangeKeywordData()
    {
        CountdownLabel = _keywords[Keyword.Countdown];
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
        RecoveryKeyLabel = _keywords[Keyword.RecoveryKey];
        RecoveryKeyDescLabel = _keywords[Keyword.RecoveryKeyDesc];
        RecoveryKeyHelperText = _keywords[Keyword.RecoveryKeyHelperText];
        ClipboardLabel = _keywords[Keyword.Copyclipboard];
        RegisterLabel = _keywords[Keyword.Register];
        LetterNotPopulatedLabel = _keywords[Keyword.LetterNotPopulated];
        EnterALetterLabel = _keywords[Keyword.EnterALetter];
        DriveNameNotPopulated = _keywords[Keyword.DriveNameNotPopulated];
        PasswordNotPopulated = _keywords[Keyword.PasswordNotPopulated];
        UsernameNotPopulated = _keywords[Keyword.UserNameNotPopulated];
        IpAddressNotPopulated = _keywords[Keyword.IpAddressNotPopulated];
        ErrorLabel = _keywords[Keyword.Error];
        LetterAndDriveNameTakenLabel = _keywords[Keyword.LetterAndDriveNameTaken];
        LetterTakenLabel = _keywords[Keyword.LetterTaken];
        DriveNameTakenLabel = _keywords[Keyword.DriveNameTaken];
        EditLetterLabel = _keywords[Keyword.EditLetter];
        EditLetterDescLabel = _keywords[Keyword.EditLetterDesc];
        EditDriveNameLabel = _keywords[Keyword.EditDriveName];
        EditDriveNameDescLabel = _keywords[Keyword.EditDriveNameDesc];
        EditIpAddressLabel = _keywords[Keyword.EditIpAddress];
        EditIpAddressDescLabel = _keywords[Keyword.EditIpAddressDesc];
        EditPasswordLabel = _keywords[Keyword.EditPassword];
        EditPasswordDescLabel = _keywords[Keyword.EditPasswordDesc];
        EditUsernameLabel = _keywords[Keyword.EditUsername];
        EditUserNameDescLabel = _keywords[Keyword.EditUsernameDesc];
        DeleteDriveLabel = _keywords[Keyword.DeleteDrive];
        DeleteDriveWarningLabel = _keywords[Keyword.DeleteDriveWarning];
        YesLabel = _keywords[Keyword.Yes];
        NoLabel = _keywords[Keyword.No];
        OkLabel = _keywords[Keyword.Ok];
        SecondsLabel = _keywords[Keyword.Seconds];
        TimerCountMinWarningLabel = _keywords[Keyword.TimerCountMinWarning];
        TimerCountMaxWarningLabel = _keywords[Keyword.TimerCountMaxWarning];
        HomePageLabel = _keywords[Keyword.HomePage];
        LogoutLabel = _keywords[Keyword.Logout];
        DriveInfoLabel = _keywords[Keyword.DriveInfo];
        StackTraceLabel = _keywords[Keyword.Stacktrace];
        ErrorCountLabel = _keywords[Keyword.ErrorCount];
        DeleteAccountLabel = _keywords[Keyword.DeleteAccount];
        DeleteAccountWarningLabel = _keywords[Keyword.DeleteAccountWarning];
        CurrentPasswordLabel = _keywords[Keyword.CurrentPassword];
        WrongRecoveryKeyLabel = _keywords[Keyword.WrongRecoveryKey];
        WrongRecoveryKeyDescLabel = _keywords[Keyword.WrongRecoveryKeyDesc];
        UsernameTakenLabel = _keywords[Keyword.UsernameTaken];
        UsernameDoesntExistLabel = _keywords[Keyword.UsernameDoesntExist];
        WrongUsernameLabel = _keywords[Keyword.WrongUsername];
        WrongUsernameDescLabel = _keywords[Keyword.WrongUsernameDesc];
        WrongPasswordLabel = _keywords[Keyword.WrongPassword];
        WrongPasswordDescLabel = _keywords[Keyword.WrongPasswordDesc];
        TextCopiedLabel = _keywords[Keyword.TextCopied];
        TextCopiedDescLabel = _keywords[Keyword.TextCopiedDesc];
        RegisterDescLabel = _keywords[Keyword.RegisterDesc];
        AccountLabel = _keywords[Keyword.Account];
    }

    [ObservableProperty]
    private string _countdownLabel;

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

    [ObservableProperty]
    private string _recoveryKeyLabel;

    [ObservableProperty]
    private string _recoveryKeyDescLabel;

    [ObservableProperty]
    private string _recoveryKeyHelperText;

    [ObservableProperty]
    private string _clipboardLabel;

    [ObservableProperty]
    private string _registerLabel;

    [ObservableProperty]
    private string _letterNotPopulatedLabel;

    [ObservableProperty]
    private string _enterALetterLabel;

    [ObservableProperty]
    private string _ipAddressNotPopulated;

    [ObservableProperty]
    private string _driveNameNotPopulated;

    [ObservableProperty]
    private string _passwordNotPopulated;

    [ObservableProperty]
    private string _usernameNotPopulated;

    [ObservableProperty]
    private string _errorLabel;

    [ObservableProperty]
    private string _okLabel;

    [ObservableProperty]
    private string _letterAndDriveNameTakenLabel;

    [ObservableProperty]
    private string _letterTakenLabel;

    [ObservableProperty]
    private string _driveNameTakenLabel;

    [ObservableProperty]
    private string _deleteDriveLabel;

    [ObservableProperty]
    private string _deleteDriveWarningLabel;

    [ObservableProperty]
    private string _yesLabel;

    [ObservableProperty]
    private string _noLabel;

    [ObservableProperty]
    private string _editLetterLabel;

    [ObservableProperty]
    private string _editLetterDescLabel;

    [ObservableProperty]
    private string _editDriveNameLabel;

    [ObservableProperty]
    private string _editDriveNameDescLabel;

    [ObservableProperty]
    private string _editIpAddressLabel;

    [ObservableProperty]
    private string _editIpAddressDescLabel;

    [ObservableProperty]
    private string _editPasswordLabel;

    [ObservableProperty]
    private string _editPasswordDescLabel;

    [ObservableProperty]
    private string _editUsernameLabel;

    [ObservableProperty]
    private string _editUserNameDescLabel;

    [ObservableProperty]
    private string _secondsLabel;

    [ObservableProperty]
    private string _timerCountMinWarningLabel;

    [ObservableProperty]
    private string _timerCountMaxWarningLabel;

    [ObservableProperty]
    private string _homePageLabel;

    [ObservableProperty]
    private string _logoutLabel;

    [ObservableProperty]
    private string _driveInfoLabel;

    [ObservableProperty]
    private string _stackTraceLabel;

    [ObservableProperty]
    private string _errorCountLabel;

    [ObservableProperty]
    private string _deleteAccountLabel;

    [ObservableProperty]
    private string _deleteAccountWarningLabel;

    [ObservableProperty]
    private string _currentPasswordLabel;

    [ObservableProperty]
    private string _wrongRecoveryKeyLabel;

    [ObservableProperty]
    private string _wrongRecoveryKeyDescLabel;

    [ObservableProperty]
    private string _usernameTakenLabel;

    [ObservableProperty]
    private string _usernameDoesntExistLabel;

    [ObservableProperty]
    private string _wrongUsernameLabel;

    [ObservableProperty]
    private string _wrongUsernameDescLabel;

    [ObservableProperty]
    private string _wrongPasswordLabel;

    [ObservableProperty]
    private string _wrongPasswordDescLabel;

    [ObservableProperty]
    private string _textCopiedLabel;

    [ObservableProperty]
    private string _textCopiedDescLabel;

    [ObservableProperty]
    private string _registerDescLabel;

    [ObservableProperty]
    private string _accountLabel;
}
