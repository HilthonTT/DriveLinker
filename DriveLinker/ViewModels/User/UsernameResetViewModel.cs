namespace DriveLinker.ViewModels.User;
public partial class UsernameResetViewModel : BaseViewModel
{
    public UsernameResetViewModel(
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            account,
            timerTracker)
    {
    }
}
