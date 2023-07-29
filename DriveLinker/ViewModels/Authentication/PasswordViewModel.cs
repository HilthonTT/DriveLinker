namespace DriveLinker.ViewModels.Authentication;

public partial class PasswordViewModel : BaseViewModel
{
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;
    private readonly IAccountService _accountService;
    private readonly Account _account;

    public PasswordViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        IAccountService accountService,
        Account account,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _recoveryKeyGenerator = recoveryKeyGenerator;
        _accountService = accountService;
        _account = account;
    }

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _recoveryKey;

    [ObservableProperty]
    private List<string> _recoveryKeys;

    [RelayCommand]
    private async Task VerifyRecoveryKeyAsync()
    {
        if (IsFormInvalid())
        {
            return;
        }

        var account = await _accountService.GetAccountByUsernameAsync(Username);
        if (account is null)
        {
            await Shell.Current.DisplayAlert(
                "Error.", $"There is not such account with username: {Username}.", "OK");
            return;
        }

        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync(account);

        if (recoveryKeys.Contains(RecoveryKey))
        {
            _account.Id = account.Id;
            _account.Username = account.Username;

            await LoadHomePage();
        }
        else
        {
            await Shell.Current.DisplayAlert(
                "Error.", $"Invalid recovery key.", "OK");
        }
    }

    private bool IsFormInvalid()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(RecoveryKey))
        {
            return true;
        }

        return false;
    }
}
