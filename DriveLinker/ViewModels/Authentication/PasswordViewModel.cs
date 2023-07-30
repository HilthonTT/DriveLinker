namespace DriveLinker.ViewModels.Authentication;

public partial class PasswordViewModel : AuthBaseViewModel
{
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;
    private readonly IAccountService _accountService;
    private readonly Account _account;

    public PasswordViewModel(
        ILanguageDictionary languageDictionary,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        IAccountService accountService,
        ILanguageHelper languageHelper,
        Account account,
        TemporaryLanguageSelector languageSelector) : base(
            languageDictionary,
            languageHelper,
            languageSelector)
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

        Username = "";
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
