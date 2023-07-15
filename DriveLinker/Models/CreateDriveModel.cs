namespace DriveLinker.Models;

public class CreateDriveModel
{
    public string Letter { get; set; }
    public string IpAddress { get; set; }
    public string DriveName { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }

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
