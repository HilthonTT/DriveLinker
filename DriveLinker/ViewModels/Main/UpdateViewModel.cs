namespace DriveLinker.ViewModels.Main;

public partial class UpdateViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;

    public UpdateViewModel(
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ITimerTracker timerTracker,
        IAccountService accountService) : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _accountService = accountService;
    }

    [ObservableProperty]
    private string _username;

    [ObservableProperty] 
    private string _password;

    [ObservableProperty]
    private string _ipAddress;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [ObservableProperty]
    private Color _buttonColor = Gray;

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
    private async Task UpdateAllDrivesAsync()
    {
        if (IsBusy || IsFieldNotFilled())
        {
            return;
        }

        IsBusy = true;

        bool answer = await Shell.Current.DisplayAlert(
            "Update all drives?", "Updating all the drives is irreversible", YesLabel, NoLabel);

        if (answer)
        {
            await _accountService.UpdateAllAccountDrivesAsync(Username, Password, IpAddress);
            await FlushPageAsync();
        }

        IsBusy = false;
    }

    private async Task FlushPageAsync()
    {
        Username = "";
        Password = "";
        IpAddress = "";

        await ClosePageAsync();
    }

    private bool IsFieldNotFilled()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(IpAddress))
        {
            return true;
        }

        return false;
    }
}
