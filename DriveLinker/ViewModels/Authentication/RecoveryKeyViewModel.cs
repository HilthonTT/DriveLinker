namespace DriveLinker.ViewModels.Authentication;
public partial class RecoveryKeyViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public RecoveryKeyViewModel(
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        Account account,
        TimerTracker timerTracker) : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _recoveryKeyGenerator = recoveryKeyGenerator;
    }

    [ObservableProperty]
    private Account _account;

    [ObservableProperty]
    private string _recoveryKey;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Account = query["Account"] as Account;
    }

    [RelayCommand]
    private async Task GenerateRecoveryKeyAsync()
    {
        string recoveryKey = await _recoveryKeyGenerator.GetRecoveryKeyAsync(Account);

        if (string.IsNullOrWhiteSpace(recoveryKey))
        {
            RecoveryKey = await _recoveryKeyGenerator.GenerateRecoveryKeyAsync(Account);
        }
        else
        {
            RecoveryKey = recoveryKey;
        }
    }
}
