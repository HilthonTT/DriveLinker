namespace DriveLinker.ViewModels.User;
public partial class UsernameResetViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;
    private readonly IAccountService _accountService;
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public UsernameResetViewModel(
        ILanguageDictionary languageDictionary,
        ITimerTracker timerTracker,
        IAuthentication auth,
        IAccountService accountService,
        IRecoveryKeyGenerator recoveryKeyGenerator) : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _auth = auth;
        _accountService = accountService;
        _recoveryKeyGenerator = recoveryKeyGenerator;
    }

    [ObservableProperty]
    private string _currentPassword;

    [ObservableProperty]
    private string _recoveryKey;

    [ObservableProperty]
    private string _newUsername;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotRecovery))]
    private bool _isRecovery;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [ObservableProperty]
    private Color _buttonColor = Gray;

    public bool IsNotRecovery => !IsRecovery;

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
    private async Task UpdateUsernameAsync()
    {
        if (IsRecovery)
        {
            await ChangeUsernameWithRecoveryKeyAsync();
        }
        else
        {
            await ChangeUsernameWithPasswordAsync();
        }
    }

    private async Task ChangeUsernameWithRecoveryKeyAsync()
    {
        if (string.IsNullOrWhiteSpace(RecoveryKey) || string.IsNullOrWhiteSpace(NewUsername))
        {
            return;
        }

        if (await IsAccountTaken())
        {
            return;
        }

        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync(_auth.GetAccount());
        if (recoveryKeys.Contains(RecoveryKey) is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong recovery key.", OkLabel);
            return;
        }

        await _auth.ChangeUsernameAsync(_auth.GetAccount().Username, NewUsername);
        await ClosePageAsync();
    }

    private async Task ChangeUsernameWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(CurrentPassword))
        {
            return;
        }

        if (await IsAccountTaken())
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(_auth.GetAccount().Username, CurrentPassword);
        if (verifiedAccount.IsCorrect is false)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Wrong password.", OkLabel);
            return;
        }

        await _auth.ChangeUsernameAsync(_auth.GetAccount().Username, NewUsername);
        await ClosePageAsync();
    }

    private async Task<bool> IsAccountTaken()
    {
        var account = await _accountService.GetAccountByUsernameAsync(NewUsername);
        if (account is not null)
        {
            await Shell.Current.DisplayAlert(ErrorLabel, "Username already taken.", OkLabel);
            return true;
        }

        return false;
    }
}
