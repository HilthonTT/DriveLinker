namespace DriveLinker.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    private const bool Animate = true;

    private static CountdownTimer _timer;
    private readonly ISettingsService _settingsService;
    private readonly IWindowsHelper _windowsHelper;

    public BaseViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        TimerTracker timerTracker)
    {
        _settingsService = settingsService;
        _windowsHelper = windowsHelper;
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

    private void HandleCountdownFinished()
    {
        _windowsHelper.MinimizeWindow();
        TimerTracker.IsCountdownVisible = false;
    }

    public async Task SetUpTimerAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();

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
