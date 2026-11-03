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

        PlaceHolderText = CurrentPasswordLabel;
    }

    [ObservableProperty]
    public partial string NewUsername { get; set; }

    [ObservableProperty]
    public partial string PlaceHolderText { get; set; }

    [ObservableProperty]
    public partial string TextValue { get; set; }

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
        if (string.IsNullOrWhiteSpace(TextValue) || string.IsNullOrWhiteSpace(NewUsername))
        {
            return;
        }

        if (await IsAccountTaken())
        {
            return;
        }

        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync(_auth.GetAccount());
        if (recoveryKeys.Contains(TextValue) is false)
        {
            await Shell.Current.DisplayAlertAsync(WrongRecoveryKeyLabel, WrongRecoveryKeyDescLabel, OkLabel);
            return;
        }

        await _auth.ChangeUsernameAsync(_auth.GetAccount().Username, NewUsername);
        await LoadHomePage();
    }

    private async Task ChangeUsernameWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(TextValue))
        {
            return;
        }

        if (await IsAccountTaken())
        {
            return;
        }

        var verifiedAccount = await _auth.VerifyPasswordAsync(_auth.GetAccount().Username, TextValue);
        if (verifiedAccount.IsCorrect is false)
        {
            await Shell.Current.DisplayAlertAsync(WrongPasswordLabel, WrongPasswordDescLabel, OkLabel);
            return;
        }

        await _auth.ChangeUsernameAsync(_auth.GetAccount().Username, NewUsername);
        await LoadHomePage();
    }

    private async Task<bool> IsAccountTaken()
    {
        var account = await _accountService.GetAccountByUsernameAsync(NewUsername);
        if (account is not null)
        {
            await Shell.Current.DisplayAlertAsync(ErrorCountLabel, UsernameTakenLabel, OkLabel);
            return true;
        }

        return false;
    }
}
