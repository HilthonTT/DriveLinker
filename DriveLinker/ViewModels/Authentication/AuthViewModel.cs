namespace DriveLinker.ViewModels.Authentication;
public partial class AuthViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IAuthentication _auth;
    private readonly Account _account;

    public AuthViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        Account account,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _settingsService = settingsService;
        _auth = auth;
        _account = account;
    }

    [ObservableProperty]
    private ObservableCollection<Language> _languages;

    [ObservableProperty]
    private Language _selectedLanguage = Language.English;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [RelayCommand]
    private void ToggleShowPassword()
    {
        DontShowPassword = !DontShowPassword;
    }

    [RelayCommand]
    private async Task SaveLanguageAsync()
    {
        var settings = await _settingsService.GetAccountSettingsAsync(0);
        settings.Language = SelectedLanguage;

        await _settingsService.UpdateSettingsAsync(settings);
        await InitializeDictionary();
    }

    [RelayCommand]
    private async Task AuthenticateAsync()
    {
        if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(Username, Password);

        if (verifiedAccount.Account is null)
        {
            await Shell.Current.DisplayAlert(
                "Wrong username!", "Your account's username doesn't exists.", "OK");
        }

        if (verifiedAccount?.IsCorrect is true)
        {
            AssignAccount(verifiedAccount);
            await LoadHomePage();
        }
        else
        {
            await Shell.Current.DisplayAlert(
                "Wrong password!", "The password you've enter is wrong.", "OK");
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

    private void AssignAccount(VerifiedAccount verifiedAccount)
    {
        _account.Id = verifiedAccount.Account.Id;
        _account.Username = verifiedAccount.Account.Username;
    }
}
