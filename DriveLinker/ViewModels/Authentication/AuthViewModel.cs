namespace DriveLinker.ViewModels.Authentication;
public partial class AuthViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IAuthentication _auth;
    private readonly ILanguageService _languageService;

    public AuthViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ILanguageService languageService,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            timerTracker)
    {
        _settingsService = settingsService;
        _auth = auth;
        _languageService = languageService;

        Languages = _languageService
            .GetLanguages()
            .ToObservableCollection();
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private Language _selectedLanguage = Language.English;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [ObservableProperty]
    private bool _showRegisterButton;

    [RelayCommand]
    private void ToggleShowPassword()
    {
        DontShowPassword = !DontShowPassword;
    }

    [RelayCommand]
    private async Task VerifyAccountExistance()
    {
        string hashedPassword = await SecureStorage.GetAsync(nameof(Authentication));

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            ShowRegisterButton = true;
        }
        else
        {
            ShowRegisterButton = false;
        }
    }

    [RelayCommand]
    private async Task SaveLanguageAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        settings.Language = SelectedLanguage;

        await _settingsService.UpdateSettingsAsync(settings);
        InitializeDictionary();
    }

    [RelayCommand]
    private async Task AuthenticateAsync()
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        bool isCorrectPassword = await _auth.VerifyPasswordAsync(Password);

        if (isCorrectPassword)
        {
            await LoadHomePageAsync();
        }
        else
        {
            await Shell.Current.DisplayAlert(
                "Wrong password!",
                "The password you've enter is wrong",
                "Ok");
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        await Shell.Current.GoToAsync(nameof(PasswordPage));
    }

    private async Task LoadHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
