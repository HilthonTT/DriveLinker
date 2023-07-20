using DriveLinker.Helpers;

namespace DriveLinker.ViewModels;
public partial class MainViewModel : BaseViewModel
{
    private CountdownTimer _timer;
    private const bool Animate = true;
    
    private readonly IDriveService _driveService;
    private readonly IDummyService _dummyService;
    private readonly ISettingsService _settingsService;
    private readonly ILinker _linker;
    private readonly IWindowsHelper _windowsHelper;

    public MainViewModel(
        IDriveService driveService,
        IDummyService dummyService,
        ISettingsService settingsService,
        ILinker linker,
        IWindowsHelper windowsHelper)
        : base(settingsService, windowsHelper)
    {
        _driveService = driveService;
        _dummyService = dummyService;
        _settingsService = settingsService;
        _linker = linker;
        _windowsHelper = windowsHelper;

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
    private ObservableCollection<Drive> _drives = new();

    [ObservableProperty]
    private List<string> _searchResults;

    [ObservableProperty]
    private Drive _selectedDrive = new();

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
    private void MinimizeApp()
    {
        _windowsHelper.MinimizeWindow();
    }

    [RelayCommand]
    private async Task LoadDrivesAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        var drives = await _driveService.GetAllDrivesAsync();

        if (drives?.Count <= 0)
        {
            drives = _dummyService.GetDummyDrives();
        }

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
    private async Task PerformSearchAsync(string query)
    {
        var output = await _driveService.GetAllDrivesAsync();

        if (string.IsNullOrWhiteSpace(query) is false)
        {
            output = output.Where(d => d.DriveName.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                d.Letter.Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
        
        Drives = output.ToObservableCollection();
    }

    [RelayCommand]
    private async Task LoadDrivePageAsync()
    {
        var parameters = new Dictionary<string, object>
        {
            { "Drive", SelectedDrive },
        };

        await Shell.Current.GoToAsync(nameof(DrivePage), Animate, parameters);

        SelectedDrive = null;
    }

    [RelayCommand]
    private async Task LoadSettingsPage()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), Animate);
    }

    [RelayCommand]
    private async Task LoadCreatePage()
    {
        await Shell.Current.GoToAsync(nameof(CreatePage), Animate);
    }
}
