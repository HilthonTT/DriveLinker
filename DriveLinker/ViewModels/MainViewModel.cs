namespace DriveLinker.ViewModels;
public partial class MainViewModel : BaseViewModel
{
    private const bool Animate = true;
    
    private readonly IDriveService _driveService;
    private readonly IDummyService _dummyService;
    private readonly ILinker _linker;

    public MainViewModel(
        IDriveService driveService,
        IDummyService dummyService,
        ILinker linker)
    {
        _driveService = driveService;
        _dummyService = dummyService;
        _linker = linker;
    }

    [ObservableProperty] private ObservableCollection<Drive> _drives = new();


    private void ChecksDriveConnection(Drive drive)
    {
        _linker.IsDriveConnected(drive);
    }

    [RelayCommand]
    private async Task LoadDrivesAsync()
    {
        var drives = await _driveService.GetAllDrivesAsync();

        if (drives?.Count <= 0)
        {
            drives = _dummyService.GetDummyDrives();
        }

        Parallel.ForEach(drives, ChecksDriveConnection);

        Drives = new(drives);
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
        }
    }

    [RelayCommand]
    private async Task LoadDrivePageAsync(Drive drive)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(Drive), drive },
        };

        await Shell.Current.GoToAsync(nameof(DrivePage), Animate, parameters);
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
