namespace DriveLinker.Models.Interfaces;

public interface ITimerTracker
{
    public bool IsCountdownVisible { get; set; }
    public int SecondsRemaining { get; set; }
    public CountdownTimer Timer { get; set; }
}
