namespace DriveLinker.ViewModels;
public partial class MainViewModel : BaseViewModel
{
    private const bool Animate = true;
    private readonly IDriveService _driveService;
    private readonly IDummyService _dummyService;

    public MainViewModel(
        IDriveService driveService,
        IDummyService dummyService)
    {
        _driveService = driveService;
        _dummyService = dummyService;
    }

    [ObservableProperty] private ObservableCollection<Drive> _drives = new();


    [RelayCommand]
    private async Task LoadDrivesAsync()
    {
        var drives = await _driveService.GetAllDrivesAsync();

        if (drives?.Count <= 0)
        {
            drives = _dummyService.GetDummyDrives();
        }

        Drives = new(drives);
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
