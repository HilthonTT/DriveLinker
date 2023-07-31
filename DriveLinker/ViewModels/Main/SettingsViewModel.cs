﻿namespace DriveLinker.ViewModels.Main;
public partial class SettingsViewModel : BaseViewModel
{
    private readonly static Color Green = Color.FromArgb("#00FF00");
    private readonly static Color Black = Color.FromArgb("#000000");

    private readonly ISettingsService _settingsService;
    private readonly ILanguageDictionary _languageDictionary;
    private readonly ILanguageHelper _languageHelper;
    private readonly IAccount _account;

    public SettingsViewModel(
        ISettingsService settingsService,
        ILanguageDictionary languageDictionary,
        ILanguageHelper languageHelper,
        IAccount account,
        ITimerTracker timerTracker)
        : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _settingsService = settingsService;
        _languageDictionary = languageDictionary;
        _languageHelper = languageHelper;
        _account = account;

        MainSettingsColor = Green;
        ExtraSettingsColor = Black;
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
    private Color _mainSettingsColor;

    [ObservableProperty]
    private Color _extraSettingsColor;

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
        Settings = await _settingsService.GetAccountSettingsAsync(_account.Id);
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


        Settings.AccountId = _account.Id;
        Settings.Language = _languageHelper.GetLanguage(SelectedLanguage);

        await _settingsService.SetSettingsAsync(Settings);
        await ClosePageAsync();
    }
}
