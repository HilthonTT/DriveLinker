namespace DriveLinker.ViewModels.User;
public partial class AccountViewModel : BaseViewModel
{
    private readonly IAuthentication _auth;

    public AccountViewModel(
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _auth = auth;
        Model.Id = _auth.GetAccount().Id;
        Model.Username = _auth.GetAccount().Username;

        AccountButtonText = DeleteAccountLabel.TrimEnd('.');
    }

    [ObservableProperty]
    private Account _model = new();

    [ObservableProperty]
    private string _accountButtonText;

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlert(
            DeleteAccountLabel, DeleteAccountWarningLabel, YesLabel, NoLabel);

        if (isDelete)
        {
            await _auth.DeleteAccountAsync();
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
