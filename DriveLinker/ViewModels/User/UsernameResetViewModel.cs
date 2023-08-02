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

    [ObservableProperty]
    private string _currentPassword;

    [ObservableProperty]
    private string _newUsername;

    [RelayCommand]
    private async Task UpdateUsernameAsync()
    {

    }
}
