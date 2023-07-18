using System.Timers;
using STimer = System.Timers.Timer;

namespace DriveLinker.Helpers;
public partial class CountdownTimer : ObservableObject
{
    private readonly STimer _countdownTimer;
    
    public event EventHandler<int> CountdownTick;
    public event EventHandler CountdownFinished;

    [ObservableProperty]
    private int _secondsRemaining = 0;

    public CountdownTimer(int initialSeconds)
    {
        SecondsRemaining = initialSeconds;
        _countdownTimer = new STimer(1000);
        _countdownTimer.Elapsed += OnCountdownTick;
    }

    private void OnCountdownTick(object sender, ElapsedEventArgs e)
    {
        SecondsRemaining--;

        CountdownTick?.Invoke(this, SecondsRemaining);

        if (SecondsRemaining == 0)
        {
            _countdownTimer.Stop();
            CountdownFinished?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Start()
    {
        _countdownTimer.Start();
    }

    public void Stop()
    {
        _countdownTimer.Stop();
    }

    public void Reset()
    {
        _countdownTimer.Stop();
        SecondsRemaining = 0;
    }
}