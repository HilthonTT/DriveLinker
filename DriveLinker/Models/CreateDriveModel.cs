namespace DriveLinker.Models;

public partial class CreateDriveModel : ObservableObject
{
    [ObservableProperty]
    private string _letter;

    [ObservableProperty]
    private string _ipAddress;

    [ObservableProperty]
    private string _driveName;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _userName;

    public CreateDriveModel()
    {
        
    }

    public CreateDriveModel(Drive drive)
    {
        Letter = drive.Letter;
        IpAddress = drive.IpAddress;
        DriveName = drive.DriveName;
        Password = drive.Password;
        UserName = drive.UserName;
    }
}
