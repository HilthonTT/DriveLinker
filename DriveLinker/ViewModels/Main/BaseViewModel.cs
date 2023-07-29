namespace DriveLinker.ViewModels.Main;
public partial class BaseViewModel : LanguageViewModel
{
    private const bool Animate = true;
    private readonly Account _account;

    public BaseViewModel(
        ILanguageDictionary languageDictionary,
        Account account,
        TimerTracker timerTracker)
        : base(languageDictionary)
    {
        _account = account;
        TimerTracker = timerTracker;
        AccountUsername = account.Username;
    }

    [ObservableProperty]
    private TimerTracker _timerTracker;

    [ObservableProperty] 
    private bool _isBusy;

    [ObservableProperty]
    private string _accountUsername;

    [ObservableProperty]
    private Drive _selectedDrive = new();

    [RelayCommand]
    public static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..", Animate);
    }

    [RelayCommand]
    public async Task LoadDrivePageAsync()
    {
        var parameters = new Dictionary<string, object>
        {
            { "Drive", SelectedDrive },
        };

        await Shell.Current.GoToAsync(nameof(DrivePage), Animate, parameters);

        SelectedDrive = null;
    }

    [RelayCommand]
    public async Task LoadSettingsPage()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), Animate);
    }

    [RelayCommand]
    public async Task LoadCreatePage()
    {
        await Shell.Current.GoToAsync(nameof(CreatePage), Animate);
    }

    [RelayCommand]
    public async Task LoadHomePage()
    {
        await Shell.Current.GoToAsync(nameof(MainPage), Animate);
    }

    [RelayCommand]
    public async Task LoadRecoveryPage()
    {
        var parameters = new Dictionary<string, object>()
        {
            { "Account", _account },
        };

        await Shell.Current.GoToAsync(nameof(RecoveryKeyPage), Animate, parameters);
    }

    [RelayCommand]
    public async Task LogOut()
    {
        _account.Id = 0;
        _account.Username = "";

        await Shell.Current.Navigation.PopToRootAsync(Animate);
    }

    [RelayCommand]
    public void StopTimer()
    {
        if (TimerTracker.Timer is null)
        {
            return;
        }

        TimerTracker.Timer.Stop();
        TimerTracker.Timer = null;

        TimerTracker.IsCountdownVisible = false;
        TimerTracker.SecondsRemaining = 0;
    }
}
