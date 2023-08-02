namespace DriveLinker.ViewModels.User;
public partial class AccountViewModel : BaseViewModel
{
    private readonly IAccount _account;
    private readonly IAccountService _accountService;

    public AccountViewModel(
        ILanguageDictionary languageDictionary,
        IAccount account,
        IAccountService accountService,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _account = account;
        _accountService = accountService;
        Model.Id = _account.Id;
        Model.Username = _account.Username;
    }

    [ObservableProperty]
    private Account _model = new();

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlert(
            "Delete Account", "Are you sure you want to delete your account? This is irreversible", YesLabel, NoLabel);

        if (isDelete)
        {
            await _accountService.DeleteAccountAsync((Account)_account);

            await Shell.Current.Navigation.PopToRootAsync(true);
        }
    }

    [RelayCommand]
    private static async Task LoadUsernameResetPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(UsernameResetPage));
    }

    [RelayCommand]
    private static async Task LoadPasswordResetPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(PasswordResetPage));
    }
}
