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
        string newPassword = await DisplayRegisterPasswordForm();

        bool savePassword = await DisplaySavePassword(newPassword);

        if (savePassword)
        {
            await _auth.ResetPasswordAsync(newPassword);
            await LoadHomePageAsync();
        }
    }

    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        bool changePassword = await DisplayForgotPassword();

        if (changePassword is false)
        {
            return;
        }

        string newPassword = await DisplayForgotPasswordForm();
        bool savePassword = await DisplaySavePassword(newPassword);

        if (savePassword)
        {
            await _auth.ResetPasswordAsync(newPassword);
            await LoadHomePageAsync();
        }
    }

    private async Task LoadHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }

    private static async Task<string> DisplayForgotPasswordForm()
    {
        string newPassword = await Shell.Current.DisplayPromptAsync("Forgot my password", "Enter your password.");
        return newPassword;
    }

    private static async Task<bool> DisplaySavePassword(string newPassword)
    {
        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"Your password is {newPassword}, would you like to save it?", "Save", "Cancel");

        return savePassword;
    }

    private static async Task<string> DisplayRegisterPasswordForm()
    {
        string newPassword = await Shell.Current.DisplayPromptAsync("Register", "Enter your password.");

        return newPassword;
    }

    private static async Task<bool> DisplayForgotPassword()
    {
        bool changePassword = await Shell.Current.DisplayAlert(
            "Forgot your password?", "Changing your password will delete all your drives.", "I forgot my password.", "Cancel");

        return changePassword;
    }
}
