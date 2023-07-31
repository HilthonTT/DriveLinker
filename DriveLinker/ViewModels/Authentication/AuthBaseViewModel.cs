namespace DriveLinker.ViewModels.Authentication;
public partial class AuthBaseViewModel : ObservableObject
{
    private const bool Animate = true;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly ILanguageHelper _languageHelper;
    private readonly ITemporaryLanguageSelector _languageSelector;

    public AuthBaseViewModel(
        ILanguageDictionary languageDictionary,
        ILanguageHelper languageHelper,
        ITemporaryLanguageSelector languageSelector)
    {
        _languageDictionary = languageDictionary;
        _languageHelper = languageHelper;
        _languageSelector = languageSelector;

        InitializeDictionary();
        LoadLanguages();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StringifiedLanguages))]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private ObservableCollection<string> _stringifiedLanguages;

    [ObservableProperty]
    private string _selectedLanguage;

    [ObservableProperty]
    private string _usernameLabel;

    [ObservableProperty]
    private string _passwordLabel;

    [ObservableProperty]
    private string _loginLabel;

    [ObservableProperty]
    private string _dontHaveAnAccountLabel;

    [ObservableProperty]
    private string _forgotPasswordLabel;

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
    private string _saveLabel;

    private async Task LoadLanguages()
    {
        var languages = _languageDictionary.GetLanguages();
        Languages = new(languages);

        var stringifiedLanguages = await _languageHelper.GetStringifiedLanguagesAsync(Languages);
        StringifiedLanguages = new(stringifiedLanguages);

        SelectedLanguage = _languageHelper.GetLanguageString(Language.English);
    }

    [RelayCommand]
    public void InitializeDictionary()
    {
        var keywords = _languageDictionary.GetDictionaryWithEnum(_languageSelector.SelectedLanguage);

        UsernameLabel = keywords[Keyword.UserName];
        PasswordLabel = keywords[Keyword.Password];
        LoginLabel = keywords[Keyword.Login];
        DontHaveAnAccountLabel = keywords[Keyword.DontHaveAnAccount];
        ForgotPasswordLabel = keywords[Keyword.ForgotMyPassword];
        RecoveryKeyLabel = keywords[Keyword.RecoveryKey];
        RecoveryKeyDescLabel = keywords[Keyword.RecoveryKeyDesc];
        RecoveryKeyHelperText = keywords[Keyword.RecoveryKeyHelperText];
        ClipboardLabel = keywords[Keyword.Copyclipboard];
        RegisterLabel = keywords[Keyword.Register];
        SaveLabel = keywords[Keyword.Save];
    }

    [RelayCommand]
    private void ChangeLanguage()
    {
        _languageSelector.SelectedLanguage = _languageHelper.GetLanguage(SelectedLanguage);
        InitializeDictionary();
    }

    [RelayCommand]
    public static async Task ClosePage()
    {
        await Shell.Current.GoToAsync("..");
    }

    public static async Task LoadHomePage()
    {
        await Shell.Current.GoToAsync(nameof(MainPage), Animate);
    }
}
