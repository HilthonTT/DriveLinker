namespace DriveLinker.ViewModels.User;
public partial class AccountViewModel : BaseViewModel
{
    private readonly IAccount _account;

    public AccountViewModel(
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _account = account;

        Model.Id = _account.Id;
        Model.Username = _account.Username;
    }

    [ObservableProperty]
    private Account _model = new();


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
