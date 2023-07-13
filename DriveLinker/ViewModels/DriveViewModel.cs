namespace DriveLinker.ViewModels;

public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;

    public DriveViewModel(IDriveService driveService)
    {
        _driveService = driveService;
    }

    [ObservableProperty] private Drive _driveItem;
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        DriveItem = query[nameof(Drive)] as Drive;
    }
}
