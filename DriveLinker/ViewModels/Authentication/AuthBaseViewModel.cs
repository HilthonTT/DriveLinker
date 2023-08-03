namespace DriveLinker.ViewModels.Authentication;
public partial class AuthBaseViewModel : LanguageViewModel
{
    public static readonly Color White = Color.FromArgb("#FFFFFF");
    public static readonly Color Gray = Color.FromArgb("#808080");

    private const bool Animate = true;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly ILanguageHelper _languageHelper;
    private readonly ITemporaryLanguageSelector _languageSelector;

    public AuthBaseViewModel(
        ILanguageDictionary languageDictionary,
        ILanguageHelper languageHelper,
        ITemporaryLanguageSelector languageSelector) : base(languageDictionary)
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

    private async Task LoadLanguages()
    {
        var languages = _languageDictionary.GetLanguages();
        Languages = new(languages);

        var stringifiedLanguages = await _languageHelper.GetStringifiedLanguagesAsync(Languages);
        StringifiedLanguages = new(stringifiedLanguages);

        SelectedLanguage = _languageHelper.GetLanguageString(_languageSelector.SelectedLanguage);
    }

    [RelayCommand]
    private async Task ChangeLanguage()
    {
        _languageSelector.SelectedLanguage = _languageHelper.GetLanguage(SelectedLanguage);
        await InitializeDictionary();
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

    [RelayCommand]
    public static async Task LoadRecoveryPage()
    {
        await Shell.Current.GoToAsync(nameof(RecoveryKeyPage), Animate);
    }
}
