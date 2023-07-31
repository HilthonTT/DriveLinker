﻿namespace DriveLinker.ViewModels.Main;

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
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ErrorCount))]
    private ObservableCollection<string> _errorMessages;

    public int ErrorCount => ErrorMessages.Count;
}
