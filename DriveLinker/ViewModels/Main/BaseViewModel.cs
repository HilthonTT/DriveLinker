namespace DriveLinker.ViewModels.Main;
public partial class BaseViewModel : LanguageViewModel
{
    private const bool Animate = true;

    private static CountdownTimer _timer;
    private readonly ISettingsService _settingsService;
    private readonly IWindowsHelper _windowsHelper;
    private readonly Account _account;

    public BaseViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        Account account,
        TimerTracker timerTracker)
        : base(languageDictionary)
    {
        _settingsService = settingsService;
        _windowsHelper = windowsHelper;
        _account = account;

        TimerTracker = timerTracker;
    }

    [ObservableProperty]
    private TimerTracker _timerTracker;

    [ObservableProperty] 
    private bool _isBusy;

    [ObservableProperty]
    private Drive _selectedDrive = new();

    [RelayCommand]
    public static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
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

    private void HandleCountdownFinished()
    {
        _windowsHelper.MinimizeWindow();
        TimerTracker.IsCountdownVisible = false;
    }

    public async Task SetUpTimerAsync()
    {
        var settings = await _settingsService.GetAccountSettingsAsync(_account.Id);

        if (settings?.AutoMinimize is true)
        {
            TimerTracker.IsCountdownVisible = true;

            _timer = new(15);
            _timer.Start();
            _timer.CountdownTick += (e, s) => TimerTracker.SecondsRemaining = s;
            _timer.CountdownFinished += (s, e) => HandleCountdownFinished();
        }
        else
        {
            TimerTracker.IsCountdownVisible = false;
        }
    }

    [RelayCommand]
    public void StopTimer()
    {
        if (_timer is null)
        {
            return;
        }

        _timer.Stop();
        _timer = null;

        TimerTracker.IsCountdownVisible = false;
        TimerTracker.SecondsRemaining = 0;
    }
}
