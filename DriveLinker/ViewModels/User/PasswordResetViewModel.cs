namespace DriveLinker.ViewModels.User;
public partial class PasswordResetViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public PasswordResetViewModel(
        ILanguageDictionary languageDictionary,
        ITimerTracker timerTracker,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        IAuthentication auth) : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _auth = auth;
        _recoveryKeyGenerator = recoveryKeyGenerator;

        PlaceHolderText = "Current Password";
    }

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _currentPassword;

    [ObservableProperty]
    private string _textValue;

    [ObservableProperty]
    private string _placeHolderText;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotRecovery))]
    private bool _isRecovery;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [ObservableProperty]
    private Color _buttonColor = Gray;

    public bool IsNotRecovery => !IsRecovery;

    partial void OnIsRecoveryChanged(bool value)
    {
        ToggleRecoveryKey();
    }

    private void ToggleRecoveryKey()
    {
        if (IsRecovery)
        {
            PlaceHolderText = RecoveryKeyLabel;
        }
        else
        {
            PlaceHolderText = "Current Password";
        }
    }

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
    private async Task UpdatePasswordAsync()
    {
        if (IsRecovery)
        {
            await ChangePasswordWithRecoveryAsync();
        }
        else
        {
            await ChangePasswordWithPasswordAsync();
        }
    }

    private async Task ChangePasswordWithRecoveryAsync()
    {
        if (string.IsNullOrWhiteSpace(TextValue) || string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync(_auth.GetAccount());

        if (recoveryKeys.Contains(TextValue) is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong recovery key", OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_auth.GetAccount().Username, NewPassword);
        await ClosePageAsync();
    }

    private async Task ChangePasswordWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(_auth.GetAccount().Username, CurrentPassword);

        if (verifiedAccount.IsCorrect is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong password.", OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_auth.GetAccount().Username, NewPassword);
        await ClosePageAsync();
    }
}
