namespace DriveLinker.Models;
public partial class TimerTracker : ObservableObject, ITimerTracker
{
    [ObservableProperty]
    private bool _isCountdownVisible;

    [ObservableProperty]
    private int _secondsRemaining;

    public CountdownTimer Timer { get; set; } 
}
