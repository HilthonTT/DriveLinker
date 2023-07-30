using DriveLinker.Core.Enums;

namespace DriveLinker.ViewModels.Main;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly static Color Green = Color.FromArgb("#00FF00");
    private readonly static Color Black = Color.FromArgb("#000000");

    private readonly ISettingsService _settingsService;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly Account _account;

    public SettingsViewModel(
        ISettingsService settingsService,
        ILanguageDictionary languageDictionary,
        Account account,
        TimerTracker timerTracker)
        : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _settingsService = settingsService;
        _languageDictionary = languageDictionary;
        _account = account;

        MainSettingsColor = Green;
        ExtraSettingsColor = Black;
        LoadLanguages();
        LoadExtraOptions();
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private ObservableCollection<MinimizeOption> _minimizeOptions;

    [ObservableProperty]
    private ObservableCollection<MinimizeAfterOption> _minimizeAfterOptions;

    [ObservableProperty]
    private Settings _settings;

    [ObservableProperty]
    private Color _mainSettingsColor;

    [ObservableProperty]
    private Color _extraSettingsColor;

    [ObservableProperty]
    private string _selectedMinimizeOption;

    [ObservableProperty]
    private string _selectedMinimizeAfterOption;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotMainSettings))]
    private bool _isMainSettings = true;

    public bool IsNotMainSettings => !IsMainSettings;

    private void LoadLanguages()
    {
        var languages = _languageDictionary.GetLanguages();
        Languages = new(languages);
    }

    private void LoadExtraOptions()
    {
        var minimizeOptions = new List<MinimizeOption> 
        {
            MinimizeOption.MinimizeApp, 
            MinimizeOption.QuitApp,
        };

        var minimizeAfterOptions = new List<MinimizeAfterOption> 
        {
            MinimizeAfterOption.TimerFinished, 
            MinimizeAfterOption.Linking,
        };

        MinimizeOptions = new(minimizeOptions);
        MinimizeAfterOptions = new(minimizeAfterOptions);
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

    [RelayCommand]
    private void ShowMainSettings()
    {
        IsMainSettings = true;
        ChangeMainSettingsColor();
    }

    [RelayCommand]
    private void ShowExtraSettings()
    {
        IsMainSettings = false;
        ChangeExtraSettingsColor();
    }

    private void ChangeMainSettingsColor()
    {
        MainSettingsColor = Green;
        ExtraSettingsColor = Black;
    }

    private void ChangeExtraSettingsColor()
    {
        MainSettingsColor = Black;
        ExtraSettingsColor = Green;
    }
}
