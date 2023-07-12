using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace DriveLinker.Core.Models;
public partial class Drive : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

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

    [ObservableProperty]
    private string _key;

    [ObservableProperty]
    private string _iv;
}
