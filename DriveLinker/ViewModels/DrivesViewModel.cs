namespace DriveLinker.ViewModels;
public partial class DrivesViewModel : BaseViewModel
{
    private const int ItemsPerPage = 6;

    private readonly IDriveService _driveService;
    private readonly ISettingsService _settingsService;
    private readonly IAesEncryption _encryption;
    private readonly ILinker _linker;

    public DrivesViewModel(
        IDriveService driveService,
        ISettingsService settingsService,
        IAesEncryption encryption,
        ILinker linker)
    {
        _driveService = driveService;
        _settingsService = settingsService;
        _encryption = encryption;
        _linker = linker;
        LoadData();
    }

    [ObservableProperty] private ObservableCollection<Drive> _drives = new();

    [ObservableProperty] private string _searchText;

    [ObservableProperty] private int _loadedItems = ItemsPerPage;

    [ObservableProperty] public ObservableCollection<Drive> _visibleDrives = new();

    [RelayCommand]
    private void LoadMoreData()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        int driveCount = Drives.Count;
        LoadedItems += ItemsPerPage;

        if (LoadedItems > driveCount)
        {
            LoadedItems = driveCount;
        }

        VisibleDrives = Drives.Take(LoadedItems).ToObservableCollection();
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ConnectDrivesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        foreach (var drive in Drives)
        {
            await ConnectDrive(drive);
        }

        IsBusy = false;
    }

    private async Task ConnectDrive(Drive drive)
    {
        if (_linker.IsDriveConnected(drive))
        {
            return;
        }

        await _linker.ConnectDriveAsync(drive);
    }

    private async Task FilterDrives(string value)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        var output = await _driveService.GetAllDrivesAsync();
        if (string.IsNullOrWhiteSpace(value) is false)
        {
            output = output.Where(s => s.DriveName.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
                s.Letter.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        Drives = output.ToObservableCollection();
        VisibleDrives = Drives.Take(LoadedItems).ToObservableCollection();
        IsBusy = false;
    }

    private async Task LoadData()
    {
        var drives = await _driveService.GetAllDrivesAsync();
        Drives = new ObservableCollection<Drive>(drives);

        VisibleDrives = Drives.Take(LoadedItems).ToObservableCollection();
    }

    async partial void OnSearchTextChanged(string value)
    {
        await FilterDrives(value);
    }
}
