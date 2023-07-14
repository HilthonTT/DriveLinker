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
}
