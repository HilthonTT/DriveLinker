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

        AccountButtonText = DeleteAccountLabel?.TrimEnd('.');
    }

    // Labels load asynchronously, so DeleteAccountLabel is often still null in the
    // constructor (which used to throw a NullReferenceException).
    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(DeleteAccountLabel))
        {
            AccountButtonText = DeleteAccountLabel?.TrimEnd('.');
        }
    }

    [ObservableProperty]
    public partial Account Model { get; set; } = new();

    [ObservableProperty]
    public partial string AccountButtonText { get; set; }

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlertAsync(
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
