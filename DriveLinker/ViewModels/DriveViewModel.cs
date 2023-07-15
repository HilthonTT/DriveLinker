namespace DriveLinker.ViewModels;

public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;

    public DriveViewModel(IDriveService driveService)
    {
        _driveService = driveService;
    }

    [ObservableProperty] 
    private Drive _drive;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Drive = query["Drive"] as Drive;
    }

    [RelayCommand]
    private static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlert( 
            "Delete Drive?", 
            "Deleting a drive is irreversible.", 
            "Yes", 
            "No");

        if (isDelete)
        {
            await _driveService.DeleteDriveAsync(Drive);
            await ClosePageAsync();
        }
    }

    [RelayCommand]
    private async Task LoadUpdatePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(UpdatePage), true);
    }
}
