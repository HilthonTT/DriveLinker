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
    private List<string> _recoveryKeys;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Account = query["Account"] as Account;
    }

    [RelayCommand]
    private async Task GenerateRecoveryKeyAsync()
    {
        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync(Account);
        if (recoveryKeys?.Count > 0)
        {
            RecoveryKeys = new(recoveryKeys);
        }
        else
        {
            var generatedRecoveryKeys = await _recoveryKeyGenerator.GenerateRecoveryKeysAsync(Account);
            RecoveryKeys = new(generatedRecoveryKeys);
        }
    }
}
