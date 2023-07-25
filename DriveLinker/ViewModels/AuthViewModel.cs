namespace DriveLinker.ViewModels;
public partial class AuthViewModel : BaseViewModel
{
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
        _auth = auth;
        _languageService = languageService;

        Languages = _languageService
            .GetLanguages()
            .ToObservableCollection();
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private string _password;

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
        string newPassword = await Shell.Current.DisplayPromptAsync("Register", "Enter your password.");

        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"You password is {newPassword}, would you like to save it?", "Save", "Cancel");

        if (savePassword)
        {
            string hashedPassword = await _auth.ResetPasswordAsync(newPassword);

            await LoadHomePageAsync();
        }
    }

    private async Task LoadHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
