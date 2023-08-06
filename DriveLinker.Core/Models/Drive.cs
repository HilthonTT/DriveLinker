using System.Text.Json.Serialization;

namespace DriveLinker.Core.Models;
public partial class Drive : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AccountId { get; set; }

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
    [JsonIgnore]
    private string _key;

    [ObservableProperty]
    [JsonIgnore]
    private string _iv;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DriveButtonColorAsColor))]
    private string _buttonColor = "#FF0000";

    [ObservableProperty]
    [JsonIgnore]
    private string _driveType;

    [ObservableProperty]
    [JsonIgnore]
    private string _driveFormat;

    [ObservableProperty]
    private DateTime _dateCreated = DateTime.Now;

    [Ignore]
    [JsonIgnore]
    public Color DriveButtonColorAsColor => Color.FromArgb(ButtonColor);

    [Ignore]
    [JsonIgnore]
    public bool Connected { get; set; }
}
