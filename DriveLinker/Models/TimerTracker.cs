namespace DriveLinker.Models;
public partial class TimerTracker : ObservableObject
{
    [ObservableProperty]
    private bool _isCountdownVisible;

    [ObservableProperty]
    private int _secondsRemaining;

    public CountdownTimer Timer { get; set; } 
}
