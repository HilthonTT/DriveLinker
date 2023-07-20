using DriveLinker.Helpers;

namespace DriveLinker.ViewModels;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly ILanguageService _languageService;

    public SettingsViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageService languageService,
        TimerTracker timerTracker)
        : base(settingsService, windowsHelper, timerTracker)
    {
        _settingsService = settingsService;
        _languageService = languageService;
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

    private static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void LoadLanguages()
    {
        var languages = _languageService.GetLanguages();
        Languages = new(languages);
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        Settings = await _settingsService.GetSettingsAsync();
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        await _settingsService.SetSettingsAsync(Settings);
        await ClosePageAsync();
    }
}
