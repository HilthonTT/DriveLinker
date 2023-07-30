namespace DriveLinker.Core.Models;
public partial class Settings : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AccountId { get; set; }

    [ObservableProperty]
    private bool _autoLink = true;

    [ObservableProperty]
    private bool _autoMinimize = false;

    [ObservableProperty]
    private int _timerCount = 15;

    [ObservableProperty]
    private Language _language = Language.English;

    [ObservableProperty]
    private MinimizeOption _minimizeOption = MinimizeOption.MinimizeApp;

    [ObservableProperty]
    private MinimizeAfterOption _minimizeAfter = MinimizeAfterOption.TimerFinished;
}
