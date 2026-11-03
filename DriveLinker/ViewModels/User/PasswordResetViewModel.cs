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

        PlaceHolderText = CurrentPasswordLabel;
    }

    [ObservableProperty]
    public partial string NewPassword { get; set; }

    [ObservableProperty]
    public partial string TextValue { get; set; }

    [ObservableProperty]
    public partial string PlaceHolderText { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotRecovery))]
    public partial bool IsRecovery { get; set; }

    [ObservableProperty]
    public partial bool DontShowPassword { get; set; } = true;

    [ObservableProperty]
    public partial Color ButtonColor { get; set; } = Gray;

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
            PlaceHolderText = CurrentPasswordLabel;
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
            await Shell.Current.DisplayAlertAsync(WrongRecoveryKeyLabel, WrongRecoveryKeyDescLabel, OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_auth.GetAccount().Username, NewPassword);
        await ClosePageAsync();
    }

    private async Task ChangePasswordWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(TextValue) || string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(_auth.GetAccount().Username, TextValue);

        if (verifiedAccount.IsCorrect is false)
        {
            await Shell.Current.DisplayAlertAsync(WrongPasswordLabel, WrongPasswordDescLabel, OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_auth.GetAccount().Username, NewPassword);
        await ClosePageAsync();
    }
}
