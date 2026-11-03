namespace DriveLinker.Models;
public partial class TimerTracker : ObservableObject, ITimerTracker
{
    [ObservableProperty]
    public partial bool IsCountdownVisible { get; set; }

    [ObservableProperty]
    public partial int SecondsRemaining { get; set; }

    public CountdownTimer Timer { get; set; } 
}
