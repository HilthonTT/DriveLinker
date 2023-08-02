namespace DriveLinker.ViewModels.User;
public partial class PasswordResetViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;
    private readonly IAccount _account;
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public PasswordResetViewModel(
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        IAuthentication auth) : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _auth = auth;
        _account = account;
        _recoveryKeyGenerator = recoveryKeyGenerator;
    }

    [ObservableProperty]
    private string _currentPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _recoveryKey;

    [ObservableProperty]
    private bool _isRecovery;
   
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
        if (string.IsNullOrWhiteSpace(RecoveryKey) || string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync((Account)_account);

        if (recoveryKeys.Contains(RecoveryKey) is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong recovery key", OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_account.Username, NewPassword);
    }

    private async Task ChangePasswordWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword))
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(_account.Username, CurrentPassword);

        if (verifiedAccount.IsCorrect is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong password.", OkLabel);
            return;
        }

        await _auth.ChangePasswordAsync(_account.Username, NewPassword);
        await ClosePageAsync();
    }
}
