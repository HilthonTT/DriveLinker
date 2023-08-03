namespace DriveLinker.ViewModels.User;
public partial class AccountViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;
    private readonly IAccountService _accountService;

    public AccountViewModel(
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        IAccountService accountService,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _auth = auth;
        _accountService = accountService;
        Model.Id = _auth.GetAccount().Id;
        Model.Username = _auth.GetAccount().Username;
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
            await _accountService.DeleteAccountAsync(_auth.GetAccount());

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
