namespace DriveLinker.ViewModels.User;
public partial class PasswordResetViewModel : BaseViewModel
{
    public PasswordResetViewModel(
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            account,
            timerTracker)
    {
    }
}
