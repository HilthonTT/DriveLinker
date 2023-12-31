﻿namespace DriveLinker.ViewModels.Main;
public partial class MainViewModel : BaseViewModel
{
    private readonly ILinker _linker;
    private readonly IWindowsHelper _windowsHelper;
    private readonly IAccountService _accountService;
    private readonly IMemoryCache _cache;

    public MainViewModel(
        ILinker linker,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IAccountService accountService,
        ITimerTracker timerTracker,
        IAuthentication auth,
        IMemoryCache cache)
        : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _linker = linker;
        _windowsHelper = windowsHelper;
        _accountService = accountService;
        _cache = cache;
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

    private static void CheckDriveType(Drive drive, List<DriveInfo> driveInfos)
    {
        if (string.IsNullOrWhiteSpace(drive.DriveType) || 
            string.IsNullOrWhiteSpace(drive.DriveFormat))
        {
            var driveInfo = GetDriveInfo(drive, driveInfos);
            drive.DriveType = driveInfo?.DriveType.ToString() ?? "N/A";
            drive.DriveFormat = driveInfo?.DriveFormat ?? "N/A";
        }
    }

    private static DriveInfo GetDriveInfo(Drive drive, List<DriveInfo> driveInfos)
    {
        var driveInfo = driveInfos.FirstOrDefault(d => d.VolumeLabel
            .Contains(drive.DriveName, StringComparison.InvariantCultureIgnoreCase) &&
            d.RootDirectory.Name.Contains(drive.Letter, StringComparison.InvariantCultureIgnoreCase));

        return driveInfo;
    }

    private void CheckDriveStatus(Drive drive, List<DriveInfo> driveInfos)
    {
        CheckDriveType(drive, driveInfos);
        _linker.IsDriveConnected(drive);
    }

    [RelayCommand]
    private async Task LoadDrivesAsync()
    {
        var driveInfos = DriveInfo.GetDrives().ToList();
        var settings = await _accountService.GetAccountSettingsAsync();
        var drives = await _accountService.GetAccountDrivesAsync();

        Parallel.ForEach(drives, (d) => CheckDriveStatus(d, driveInfos));
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
        try
        {
            if (Drives?.Count <= 0 || IsBusy)
            {
                return;
            }

            var nonConnectedDrives = Drives.Where(d => d.Connected == false).ToList();

            IsBusy = true;
            foreach (var drive in nonConnectedDrives)
            {
                await _linker.ConnectDriveAsync(drive);
                RecalculateProgress();
            }
        }
        catch (Exception ex)
        {
            await DisplayErrorAsync(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UnlinkAllDrivesAsync()
    {
        try
        {
            if (Drives?.Count <= 0 || IsBusy)
            {
                return;
            }

            IsBusy = true;

            var connectedDrives = Drives.Where(d => d.Connected).ToList();

            foreach (var drive in connectedDrives)
            {
                await _linker.DisconnectDriveAsync(drive);
                RecalculateProgress();
            }
        }
        catch (Exception ex)
        {
            await DisplayErrorAsync(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ToggleLinkAsync(Drive drive)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayErrorAsync(ex.Message);
        }
    }

    [RelayCommand]
    private async Task PerformSearchAsync(string query)
    {
        var output = await _accountService.GetAccountDrivesAsync();

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

    [RelayCommand]
    private async Task ExportDrivesAsync()
    {
        var drives = await _accountService.GetAccountDrivesAsync();

        string jsonifiedDrives = JsonSerializer.Serialize(drives);

        var result = await FolderPicker.Default.PickAsync(new());
        if (result.IsSuccessful)
        {
            string filePath = Path.Combine(result.Folder.Path, "import_drives_file.json");

            await File.WriteAllTextAsync(filePath, jsonifiedDrives);
        }
    }

    [RelayCommand]
    private async Task DeleteAllDrivesAsync()
    {
        bool answer = await Shell.Current.DisplayAlert(
            DeleteAllDrivesLabel, DeleteAllDrivesDescLabel, YesLabel, NoLabel);

        if (answer)
        {
            Drives.Clear();
            Drives = new();

            await _accountService.DeleteAllAccountDrivesAsync();
        }
    }

    private async Task DisplayErrorAsync(string error)
    {
        await Shell.Current.DisplayAlert(ErrorLabel, error, OkLabel);
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
        string key = "IsFirstTime";

        string output = _cache.Get<string>(key);

        if (string.IsNullOrWhiteSpace(output) is false)
        {
            return;
        }

        output = "NotFirstTime";
        _cache.Set(key, output);

        var settings = await _accountService.GetAccountSettingsAsync();

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
