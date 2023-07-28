namespace DriveLinker.ViewModels.Main;
public partial class FirstBootViewModel : BaseViewModel
{
    private readonly ILanguageService _languageService;

    public FirstBootViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        ILanguageService languageService,
        Account account,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _languageService = languageService;

        Languages = _languageService.GetLanguages().ToObservableCollection();
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages;
    
    [RelayCommand]
    private async Task SaveLanguageChoiceAsync()
    {

    }
}
