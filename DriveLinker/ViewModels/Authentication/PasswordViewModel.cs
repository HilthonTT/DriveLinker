namespace DriveLinker.ViewModels.Authentication;

public partial class PasswordViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;

    public PasswordViewModel(
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
        _auth = auth;
    }

    [ObservableProperty]
    private string _newPassword;

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
    private async Task SetNewPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(Username))
        {
            return;
        }

        bool saveNewPassword = await DisplaySavePassword();
        if (saveNewPassword)
        {
            await _auth.ResetPasswordAsync(Username, NewPassword);
            await LoadHomePage();
        }
    }

    private async Task<bool> DisplaySavePassword()
    {
        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"Your password is {NewPassword}, would you like to save it?", "Save", "Cancel");

        return savePassword;
    }
}
