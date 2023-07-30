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
    [NotifyPropertyChangedFor(nameof(StringifiedLanguages))]
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
    private string _selectedLanguage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotMainSettings))]
    private bool _isMainSettings = true;

    public bool IsNotMainSettings => !IsMainSettings;

    public List<string> StringifiedLanguages => GetStrigifiedLanguages();

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
        SelectedLanguage = GetLanguageString(Settings.Language);
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        Settings.AccountId = _account.Id;
        Settings.Language = GetLanguage();

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


    private List<string> GetStrigifiedLanguages()
    {
        var stringifiedLanguages = new List<string>();

        foreach (var language in Languages)
        {
            switch (language)
            {
                case Language.English:
                    stringifiedLanguages.Add(EnglishLabel);
                    break;

                case Language.French:
                    stringifiedLanguages.Add(FrenchLabel);
                    break;

                case Language.German:
                    stringifiedLanguages.Add(GermanLabel);
                    break;

                case Language.Indonesian:
                    stringifiedLanguages.Add(IndonesianLabel);
                    break;
                default:
                    break;
            }  
        }

        return stringifiedLanguages;
    }

    private Language GetLanguage()
    {
        if (SelectedLanguage == EnglishLabel)
        {
            return Language.English;
        }
        else if (SelectedLanguage == FrenchLabel)
        {
            return Language.French;
        }
        else if (SelectedLanguage == GermanLabel)
        {
            return Language.German;
        }
        else if (SelectedLanguage == IndonesianLabel)
        {
            return Language.Indonesian;
        }
        
        return Language.English;
    }

    private string GetLanguageString(Language language)
    {
        return language switch
        {
            Language.English => EnglishLabel,
            Language.French => FrenchLabel,
            Language.German => GermanLabel,
            Language.Indonesian => IndonesianLabel,
            _ => EnglishLabel,
        };
    }
}
