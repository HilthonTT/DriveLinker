namespace DriveLinker.ViewModels.Authentication;

public partial class PasswordViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;

    public PasswordViewModel(
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
    private string _newPassword;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [RelayCommand]
    private async Task SetNewPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        bool saveNewPassword = await DisplaySavePassword();
        if (saveNewPassword)
        {
            await _auth.ResetPasswordAsync(NewPassword);
            await LoadHomePageAsync();
        }
    }

    private async Task<bool> DisplaySavePassword()
    {
        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"Your password is {NewPassword}, would you like to save it?", "Save", "Cancel");

        return savePassword;
    }

    private static async Task LoadHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
