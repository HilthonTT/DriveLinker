namespace DriveLinker.ViewModels.Main;
public partial class MainViewModel : BaseViewModel
{
    private readonly IDriveService _driveService;
    private readonly ISettingsService _settingsService;
    private readonly ILinker _linker;
    private readonly IWindowsHelper _windowsHelper;
    private readonly IAccount _account;

    public MainViewModel(
        IDriveService driveService,
        ISettingsService settingsService,
        ILinker linker,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker)
        : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _driveService = driveService;
        _settingsService = settingsService;
        _linker = linker;
        _windowsHelper = windowsHelper;
        _account = account;

        SetUpTimerAsync();
    }

    [ObservableProperty]
    private bool _isDrivesLoaded = false;

    [ObservableProperty]
    private bool _isLoading = true;
    
    [ObservableProperty]
    private bool _isNotAlreadyConnected = true;

    [ObservableProperty]
    private double _progress = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecentlyAddedDrives))]
    private ObservableCollection<Drive> _drives = new();

    [ObservableProperty]
    private List<string> _searchResults;

    public ObservableCollection<Drive> RecentlyAddedDrives => Drives
        .OrderByDescending(d => d.DateCreated)
        .Take(10)
        .ToObservableCollection();

    private void DrivesLoaded()
    {
        IsLoading = false;
        IsDrivesLoaded = true;
    }

    private void RecalculateProgress()
    {
        var connectedDrives = Drives.Where(d => d.Connected).ToList();

        float connectedDrivesCount = connectedDrives.Count;
        Progress = connectedDrivesCount / Drives.Count;
    }

    [RelayCommand]
    private async Task LoadDrivesAsync()
    {
        int accountId = _account.Id;

        var settings = await _settingsService.GetAccountSettingsAsync(_account.Id);
        var drives = await _driveService.GetAllAccountDrivesAsync(_account.Id);

        Parallel.ForEach(drives, (d) => _linker.IsDriveConnected(d));
        Drives = new(drives);
        RecalculateProgress();
        DrivesLoaded();

        if (settings.AutoLink && IsNotAlreadyConnected)
        {
            await LinkAllDrivesAsync();
            IsNotAlreadyConnected = false;
        }
    }

    [RelayCommand]
    private async Task LinkAllDrivesAsync()
    {
        if (Drives?.Count <= 0)
        {
            return;
        }

        var nonConnectedDrives = Drives.Where(d => d.Connected == false).ToList();

        foreach (var drive in nonConnectedDrives)
        {
            await _linker.ConnectDriveAsync(drive);
            RecalculateProgress();
        }
    }

    [RelayCommand]
    private async Task UnlinkAllDrivesAsync()
    {
        if (Drives?.Count <= 0) 
        { 
            return; 
        }

        var connectedDrives = Drives.Where(d => d.Connected).ToList();

        foreach (var drive in connectedDrives)
        {
            await _linker.DisconnectDriveAsync(drive);
            RecalculateProgress();
        }
    }

    [RelayCommand]
    public async Task ToggleLinkAsync(Drive drive)
    {
        if (drive.Connected)
        {
            await _linker.DisconnectDriveAsync(drive);
        }
        else
        {
            await _linker.ConnectDriveAsync(drive);
        }
    }

    [RelayCommand]
    private async Task PerformSearchAsync(string query)
    {
        var output = await _driveService.GetAllAccountDrivesAsync(_account.Id);

        if (string.IsNullOrWhiteSpace(query) is false)
        {
            var drivesWithScores = output.Select(d => new
            {
                Drive = d,
                Score = CalculateMatchingScore(d, query)
            });

            var sortedDrives = drivesWithScores.OrderByDescending(d => d.Score).Select(d => d.Drive).ToList();

            Drives = sortedDrives.ToObservableCollection();
        }
        else
        {
            Drives = output.ToObservableCollection();
        }
    }

    private static int CalculateMatchingScore(Drive drive, string query)
    {
        int score = 0;

        score += CountOccurrences(drive.DriveName, query, StringComparison.InvariantCultureIgnoreCase);
        score += CountOccurrences(drive.Letter, query, StringComparison.InvariantCultureIgnoreCase);

        return score;
    }

    private static int CountOccurrences(string source, string query, StringComparison comparison)
    {
        int count = 0;
        int i = 0;
        while ((i = source.IndexOf(query, i, comparison)) != -1)
        {
            i += query.Length;
            count++;
        }
        return count;
    }

    private void HandleCountdownFinished()
    {
        _windowsHelper.MinimizeWindow();
        TimerTracker.IsCountdownVisible = false;
    }

    [RelayCommand]
    private async Task SetUpTimerAsync()
    {
        var settings = await _settingsService.GetAccountSettingsAsync(_account.Id);

        if (settings?.AutoMinimize is true)
        {
            TimerTracker.IsCountdownVisible = true;

            TimerTracker.Timer = new(settings.TimerCount);
            TimerTracker.SecondsRemaining = settings.TimerCount;
            TimerTracker.Timer.Start();
            TimerTracker.Timer.CountdownTick += (e, s) => TimerTracker.SecondsRemaining = s;
            TimerTracker.Timer.CountdownFinished += (s, e) => HandleCountdownFinished();
        }
        else
        {
            TimerTracker.IsCountdownVisible = false;
        }
    }
}
