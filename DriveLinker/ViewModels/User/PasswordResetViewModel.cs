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

    [ObservableProperty]
    private string _currentPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _recoveryKey;

    [RelayCommand]
    private async Task UpdatePasswordAsync()
    {

    }
}
