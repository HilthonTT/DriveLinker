namespace DriveLinker.ViewModels.Authentication;
public partial class AuthViewModel : AuthBaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IAuthentication _auth;
    private readonly IAccount _account;

    public AuthViewModel(
        ISettingsService settingsService,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ILanguageHelper languageHelper,
        IAccount account,
        ITemporaryLanguageSelector languageSelector) : base(
            languageDictionary,
            languageHelper,
            languageSelector)
    {
        _settingsService = settingsService;
        _auth = auth;
        _account = account;
    }

    [ObservableProperty]
    public partial string Password { get; set; }

    [ObservableProperty]
    public partial string Username { get; set; }

    [ObservableProperty]
    public partial bool DontShowPassword { get; set; } = true;

    [ObservableProperty]
    public partial Color ButtonColor { get; set; } = Gray;

    [RelayCommand]
    private void ToggleShowPassword()
    {
        DontShowPassword = !DontShowPassword;

        if (DontShowPassword)
        {
            ButtonColor = Gray;
        }
        else
        {
            ButtonColor = White;
        }
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
            await Shell.Current.DisplayAlertAsync(
                WrongUsernameLabel, WrongUsernameDescLabel, OkLabel);

            Username = "";
        }
        else if (verifiedAccount?.IsCorrect is true)
        {
            AssignAccount(verifiedAccount);
            await LoadHomePage();

            Username = "";
            Password = "";
        }
        else
        {
            await Shell.Current.DisplayAlertAsync(
                WrongPasswordLabel, WrongPasswordDescLabel, OkLabel);

            Password = "";
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
