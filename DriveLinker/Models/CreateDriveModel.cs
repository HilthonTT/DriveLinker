namespace DriveLinker.Models;

public partial class CreateDriveModel : ObservableObject
{
    [ObservableProperty]
    public partial string Letter { get; set; }

    [ObservableProperty]
    public partial string IpAddress { get; set; }

    [ObservableProperty]
    public partial string DriveName { get; set; }

    [ObservableProperty]
    public partial string Password { get; set; }

    [ObservableProperty]
    public partial string UserName { get; set; }

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
