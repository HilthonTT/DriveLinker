namespace DriveLinker.Core.Models;
public partial class Settings : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AccountId { get; set; }

    [ObservableProperty]
    public partial bool AutoLink { get; set; } = true;

    [ObservableProperty]
    public partial bool AutoMinimize { get; set; } = false;

    [ObservableProperty]
    public partial int TimerCount { get; set; } = 15;

    [ObservableProperty]
    public partial Language Language { get; set; } = Language.English;
}
