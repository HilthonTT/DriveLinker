using DriveLinker.Helpers;

namespace DriveLinker.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    private CountdownTimer _timer;
    private readonly ISettingsService _settingsService;
    private readonly IWindowsHelper _windowsHelper;

    public BaseViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper)
    {
        _settingsService = settingsService;
        _windowsHelper = windowsHelper;
    }

    [ObservableProperty] 
    private bool _isBusy;

    [ObservableProperty]
    private bool _isCountdownVisible;

    [ObservableProperty]
    private int _secondsRemaining;
    
    [RelayCommand]
    public static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void HandleCountdownFinished()
    {
        _windowsHelper.MinimizeWindow();
        IsCountdownVisible = false;
    }

    public async Task SetUpTimerAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();

        if (settings?.AutoMinimize is true)
        {
            IsCountdownVisible = true;

            _timer = new(10);
            _timer.Start();
            _timer.CountdownTick += (e, s) => SecondsRemaining = s;
            _timer.CountdownFinished += (s, e) => HandleCountdownFinished();
        }
        else
        {
            IsCountdownVisible = false;
        }
    }
}
