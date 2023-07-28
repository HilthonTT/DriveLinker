namespace DriveLinker.ViewModels;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly ILanguageService _languageService;
    private readonly Account _account;

    public SettingsViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageService languageService,
        ILanguageDictionary languageDictionary,
        Account account,
        TimerTracker timerTracker)
        : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _settingsService = settingsService;
        _languageService = languageService;
        _account = account;
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages = new();

    [ObservableProperty]
    private Settings _settings = new();

    [ObservableProperty]
    private bool _autoLink = false;

    [ObservableProperty]
    private bool _autoMinimize = false;

    [ObservableProperty]
    private Language _selectedLanguage = Language.English;

    [RelayCommand]
    private void LoadLanguages()
    {
        var languages = _languageService.GetLanguages();
        Languages = new(languages);
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        Settings = await _settingsService.GetAccountSettingsAsync(_account.Id);
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        Settings.AccountId = _account.Id;

        await _settingsService.SetSettingsAsync(Settings);
        await ClosePageAsync();
    }
}
