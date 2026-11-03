namespace DriveLinker.ViewModels.Main;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly static Color Green = Color.FromArgb("#00FF00");
    private readonly static Color Black = Color.FromArgb("#000000");

    private readonly ISettingsService _settingService;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly ILanguageHelper _languageHelper;
    private readonly IWindowsHelper _windowsHelper;
    private readonly IAuthentication _auth;

    public SettingsViewModel(
        ISettingsService settingService,
        ILanguageDictionary languageDictionary,
        ILanguageHelper languageHelper,
        ITimerTracker timerTracker,
        IWindowsHelper windowsHelper,
        IAuthentication auth)
        : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _settingService = settingService;
        _languageDictionary = languageDictionary;
        _languageHelper = languageHelper;
        _windowsHelper = windowsHelper;
        _auth = auth;

        IsSetOnStartup = _windowsHelper.GetCheckedValue();
        LoadLanguages().FireAndForget();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StringifiedLanguages))]
    public partial ObservableCollection<Language> Languages { get; set; }

    [ObservableProperty]
    public partial List<string> StringifiedLanguages { get; set; }

    [ObservableProperty]
    public partial Settings Settings { get; set; }

    [ObservableProperty]
    public partial Color MainSettingsColor { get; set; } = Green;

    [ObservableProperty]
    public partial Color ExtraSettingsColor { get; set; } = Black;

    [ObservableProperty]
    public partial string SelectedLanguage { get; set; }

    [ObservableProperty]
    public partial bool IsSetOnStartup { get; set; }

    partial void OnIsSetOnStartupChanged(bool value)
    {
        if (IsSetOnStartup)
        {
            _windowsHelper.ToggleStartup(true);
        }
        else
        {
            _windowsHelper.ToggleStartup(false);
        }
    }

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
            await Shell.Current.DisplayAlertAsync(
                ErrorLabel, TimerCountMinWarningLabel + minSeconds + SecondsLabel, OkLabel);
            return;
        }

        if (Settings.TimerCount > maxSeconds)
        {
            await Shell.Current.DisplayAlertAsync(
                ErrorLabel, TimerCountMaxWarningLabel + maxSeconds + SecondsLabel, OkLabel);
            return;
        }


        Settings.AccountId = _auth.GetAccount().Id;
        Settings.Language = _languageHelper.GetLanguage(SelectedLanguage);

        await _settingService.SetSettingsAsync(Settings);
        await ClosePageAsync();
    }
}
