namespace DriveLinker.ViewModels.Main;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly static Color Green = Color.FromArgb("#00FF00");
    private readonly static Color Black = Color.FromArgb("#000000");

    private readonly ISettingsService _settingService;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly ILanguageHelper _languageHelper;
    private readonly IAuthentication _auth;

    public SettingsViewModel(
        ISettingsService settingService,
        ILanguageDictionary languageDictionary,
        ILanguageHelper languageHelper,
        ITimerTracker timerTracker,
        IAuthentication auth)
        : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _settingService = settingService;
        _languageDictionary = languageDictionary;
        _languageHelper = languageHelper;
        _auth = auth;

        LoadLanguages();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StringifiedLanguages))]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private List<string> _stringifiedLanguages;

    [ObservableProperty]
    private Settings _settings;

    [ObservableProperty]
    private Color _mainSettingsColor = Green;

    [ObservableProperty]
    private Color _extraSettingsColor = Black;

    [ObservableProperty]
    private string _selectedLanguage;

    private async Task LoadLanguages()
    {
        var languages = _languageDictionary.GetLanguages();
        Languages = new(languages);

        StringifiedLanguages = await _languageHelper.GetStringifiedLanguagesAsync(Languages);
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        Settings = await _settingService.GetAccountSettingsAsync(_auth.GetAccount().Id);
        SelectedLanguage = _languageHelper.GetLanguageString(Settings.Language);
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        int minSeconds = 5;
        int maxSeconds = 500;

        if (Settings.TimerCount < minSeconds)
        {
            await Shell.Current.DisplayAlert(
                ErrorLabel, TimerCountMinWarningLabel + minSeconds + SecondsLabel, OkLabel);
            return;
        }

        if (Settings.TimerCount > maxSeconds)
        {
            await Shell.Current.DisplayAlert(
                ErrorLabel, TimerCountMaxWarningLabel + maxSeconds + SecondsLabel, OkLabel);
            return;
        }


        Settings.AccountId = _auth.GetAccount().Id;
        Settings.Language = _languageHelper.GetLanguage(SelectedLanguage);

        await _settingService.SetSettingsAsync(Settings);
        await ClosePageAsync();
    }
}
