using System.Text.Json.Serialization;

namespace DriveLinker.Core.Models;
public partial class Drive : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AccountId { get; set; }

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

    [ObservableProperty]
    [JsonIgnore]
    public partial string Key { get; set; }

    [ObservableProperty]
    [JsonIgnore]
    public partial string Iv { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DriveButtonColorAsColor))]
    public partial string ButtonColor { get; set; } = "#FF0000";

    [ObservableProperty]
    [JsonIgnore]
    public partial string DriveType { get; set; }

    [ObservableProperty]
    [JsonIgnore]
    public partial string DriveFormat { get; set; }

    [ObservableProperty]
    public partial DateTime DateCreated { get; set; } = DateTime.Now;

    [Ignore]
    [JsonIgnore]
    public Color DriveButtonColorAsColor => Color.FromArgb(ButtonColor);

    [Ignore]
    [JsonIgnore]
    public bool Connected { get; set; }
}
