namespace DriveLinker.ViewModels.Authentication;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;

    public RegisterViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            timerTracker)
    {
        _auth = auth;
    }

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [RelayCommand]
    private void ToggleShowPassword()
    {
        DontShowPassword = !DontShowPassword;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        bool savePassword = await DisplaySavePassword();
        if (savePassword)
        {
            await _auth.ResetPasswordAsync(Password);
            await LoadHomePageAsync();
        }
    }

    private async Task<bool> DisplaySavePassword()
    {
        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"Your password is {Password}, would you like to save it?", "Save", "Cancel");

        return savePassword;
    }

    private static async Task LoadHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
