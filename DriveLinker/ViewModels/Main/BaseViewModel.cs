namespace DriveLinker.ViewModels.Main;
public partial class BaseViewModel : LanguageViewModel
{
    public static readonly Color White = Color.FromArgb("#FFFFFF");
    public static readonly Color Gray = Color.FromArgb("#808080");

    private const bool Animate = true;
    private readonly IAccount _account;
    private readonly IAuthentication _auth;

    public BaseViewModel(
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ITimerTracker timerTracker)
        : base(languageDictionary)
    {
        _auth = auth;
        TimerTracker = (TimerTracker)timerTracker;
        AccountUsername = _auth.GetAccount().Username;
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
    public static async Task LoadSettingsPage()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), Animate);
    }

    [RelayCommand]
    public static async Task LoadCreatePage()
    {
        await Shell.Current.GoToAsync(nameof(CreatePage), Animate);
    }

    [RelayCommand]
    public static async Task LoadHomePage()
    {
        await Shell.Current.GoToAsync(nameof(MainPage), Animate);
    }

    [RelayCommand]
    public static async Task LoadRecoveryPage()
    {
        await Shell.Current.GoToAsync(nameof(RecoveryKeyPage), Animate);
    }

    [RelayCommand]
    public static async Task LoadStackTracePage()
    {
        await Shell.Current.GoToAsync(nameof(StackTracePage), Animate);
    }

    [RelayCommand]
    public static async Task LoadAccountPage()
    {
        await Shell.Current.GoToAsync(nameof(AccountPage), Animate);
    }

    [RelayCommand]
    public async Task LogOut()
    {
        await _auth.LogoutAsync();
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
