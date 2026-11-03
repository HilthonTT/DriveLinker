namespace DriveLinker.ViewModels.Misc;
public partial class LanguageViewModel : ObservableObject
{
    private readonly ILanguageDictionary _language;
    private static Dictionary<Keyword, string> _keywords;

    public LanguageViewModel(ILanguageDictionary language)
    {
        _language = language;
        InitializeDictionary().FireAndForget();
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
        ExportDrivesLabel = _keywords[Keyword.ExportDrives];
        UpdateDrivesLabel = _keywords[Keyword.UpdateDrives];
        DeleteDrivesLabel = _keywords[Keyword.DeleteDrives];
        UpdateLabel = _keywords[Keyword.Update];
        DeleteAllDrivesLabel = _keywords[Keyword.DeleteAllDrives];
        DeleteAllDrivesDescLabel = _keywords[Keyword.DeleteAllDrivesDesc];
        ImportFileLabel = _keywords[Keyword.ImportFile];
        ImportFileDescLabel = _keywords[Keyword.ImportFileDesc];
        ImportDrivesLabel = _keywords[Keyword.ImportDrives];
    }

    [ObservableProperty]
    public partial string CountdownLabel { get; set; }

    [ObservableProperty]
    public partial string CreateDriveLabel { get; set; }

    [ObservableProperty]
    public partial string SettingsLabel { get; set; }

    [ObservableProperty]
    public partial string SearchLabel { get; set; }

    [ObservableProperty]
    public partial string RecentlyAddedLabel { get; set; }

    [ObservableProperty]
    public partial string DriveListingLabel { get; set; }

    [ObservableProperty]
    public partial string LinkDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string UnlinkDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string LetterLabel { get; set; }

    [ObservableProperty]
    public partial string LetterDescLabel { get; set; }

    [ObservableProperty]
    public partial string IpAddressLabel { get; set; }

    [ObservableProperty]
    public partial string IpAddressDescLabel { get; set; }

    [ObservableProperty]
    public partial string DriveNameLabel { get; set; }

    [ObservableProperty]
    public partial string DriveNameDescLabel { get; set; }

    [ObservableProperty]
    public partial string PasswordLabel { get; set; }

    [ObservableProperty]
    public partial string PasswordDescLabel { get; set; }

    [ObservableProperty]
    public partial string UserNameLabel { get; set; }

    [ObservableProperty]
    public partial string UserNameDescLabel { get; set; }

    [ObservableProperty]
    public partial string DateCreatedLabel { get; set; }

    [ObservableProperty]
    public partial string CreateLabel { get; set; }

    [ObservableProperty]
    public partial string AutoLinkDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string AutoMinimizeLabel { get; set; }

    [ObservableProperty]
    public partial string ClearOnlyLetterDriveNameLabel { get; set; }

    [ObservableProperty]
    public partial string SaveLabel { get; set; }

    [ObservableProperty]
    public partial string LanguageLabel { get; set; }

    [ObservableProperty]
    public partial string EnglishLabel { get; set; }

    [ObservableProperty]
    public partial string FrenchLabel { get; set; }

    [ObservableProperty]
    public partial string GermanLabel { get; set; }

    [ObservableProperty]
    public partial string IndonesianLabel { get; set; }

    [ObservableProperty]
    public partial string DontHaveAnAccountLabel { get; set; }

    [ObservableProperty]
    public partial string ForgotPasswordLabel { get; set; }

    [ObservableProperty]
    public partial string LoginLabel { get; set; }

    [ObservableProperty]
    public partial string RecoveryKeyLabel { get; set; }

    [ObservableProperty]
    public partial string RecoveryKeyDescLabel { get; set; }

    [ObservableProperty]
    public partial string RecoveryKeyHelperText { get; set; }

    [ObservableProperty]
    public partial string ClipboardLabel { get; set; }

    [ObservableProperty]
    public partial string RegisterLabel { get; set; }

    [ObservableProperty]
    public partial string LetterNotPopulatedLabel { get; set; }

    [ObservableProperty]
    public partial string EnterALetterLabel { get; set; }

    [ObservableProperty]
    public partial string IpAddressNotPopulated { get; set; }

    [ObservableProperty]
    public partial string DriveNameNotPopulated { get; set; }

    [ObservableProperty]
    public partial string PasswordNotPopulated { get; set; }

    [ObservableProperty]
    public partial string UsernameNotPopulated { get; set; }

    [ObservableProperty]
    public partial string ErrorLabel { get; set; }

    [ObservableProperty]
    public partial string OkLabel { get; set; }

    [ObservableProperty]
    public partial string LetterAndDriveNameTakenLabel { get; set; }

    [ObservableProperty]
    public partial string LetterTakenLabel { get; set; }

    [ObservableProperty]
    public partial string DriveNameTakenLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteDriveLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteDriveWarningLabel { get; set; }

    [ObservableProperty]
    public partial string YesLabel { get; set; }

    [ObservableProperty]
    public partial string NoLabel { get; set; }

    [ObservableProperty]
    public partial string EditLetterLabel { get; set; }

    [ObservableProperty]
    public partial string EditLetterDescLabel { get; set; }

    [ObservableProperty]
    public partial string EditDriveNameLabel { get; set; }

    [ObservableProperty]
    public partial string EditDriveNameDescLabel { get; set; }

    [ObservableProperty]
    public partial string EditIpAddressLabel { get; set; }

    [ObservableProperty]
    public partial string EditIpAddressDescLabel { get; set; }

    [ObservableProperty]
    public partial string EditPasswordLabel { get; set; }

    [ObservableProperty]
    public partial string EditPasswordDescLabel { get; set; }

    [ObservableProperty]
    public partial string EditUsernameLabel { get; set; }

    [ObservableProperty]
    public partial string EditUserNameDescLabel { get; set; }

    [ObservableProperty]
    public partial string SecondsLabel { get; set; }

    [ObservableProperty]
    public partial string TimerCountMinWarningLabel { get; set; }

    [ObservableProperty]
    public partial string TimerCountMaxWarningLabel { get; set; }

    [ObservableProperty]
    public partial string HomePageLabel { get; set; }

    [ObservableProperty]
    public partial string LogoutLabel { get; set; }

    [ObservableProperty]
    public partial string DriveInfoLabel { get; set; }

    [ObservableProperty]
    public partial string StackTraceLabel { get; set; }

    [ObservableProperty]
    public partial string ErrorCountLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteAccountLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteAccountWarningLabel { get; set; }

    [ObservableProperty]
    public partial string CurrentPasswordLabel { get; set; }

    [ObservableProperty]
    public partial string WrongRecoveryKeyLabel { get; set; }

    [ObservableProperty]
    public partial string WrongRecoveryKeyDescLabel { get; set; }

    [ObservableProperty]
    public partial string UsernameTakenLabel { get; set; }

    [ObservableProperty]
    public partial string UsernameDoesntExistLabel { get; set; }

    [ObservableProperty]
    public partial string WrongUsernameLabel { get; set; }

    [ObservableProperty]
    public partial string WrongUsernameDescLabel { get; set; }

    [ObservableProperty]
    public partial string WrongPasswordLabel { get; set; }

    [ObservableProperty]
    public partial string WrongPasswordDescLabel { get; set; }

    [ObservableProperty]
    public partial string TextCopiedLabel { get; set; }

    [ObservableProperty]
    public partial string TextCopiedDescLabel { get; set; }

    [ObservableProperty]
    public partial string RegisterDescLabel { get; set; }

    [ObservableProperty]
    public partial string AccountLabel { get; set; }

    [ObservableProperty]
    public partial string ExportDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string UpdateDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string ImportDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string UpdateLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteAllDrivesLabel { get; set; }

    [ObservableProperty]
    public partial string DeleteAllDrivesDescLabel { get; set; }

    [ObservableProperty]
    public partial string ImportFileLabel { get; set; }

    [ObservableProperty]
    public partial string ImportFileDescLabel { get; set; }
}
