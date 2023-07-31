namespace DriveLinker.ViewModels.Main;

public partial class StackTraceViewModel : BaseViewModel
{
    private readonly IStackTrace _stackTrace;

    public StackTraceViewModel(
        ILanguageDictionary languageDictionary,
        IStackTrace stackTrace,
        IAccount account,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _stackTrace = stackTrace;

        ErrorMessages = _stackTrace.ErrorMessages.ToObservableCollection();
        DummyData();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ErrorCount))]
    private ObservableCollection<string> _errorMessages;

    public int ErrorCount => ErrorMessages.Count;

    private void DummyData()
    {
        for (int i = 0; i < 20; i++)
        {
            ErrorMessages.Add(
                "ERROR: THIS IS 100% A REAL ERROR THAT IS VERY LONG BECAUSE " +
                "OF REASONS THAT I CAN OR CANNOT SAY, I DID NOT INVENT AN ERROR, THIS IS NOT A DRILL.");
        }
    }
}
