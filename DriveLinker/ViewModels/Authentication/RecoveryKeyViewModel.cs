namespace DriveLinker.ViewModels.Authentication;
public partial class RecoveryKeyViewModel : AuthBaseViewModel
{
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;
    private readonly IAccount _account;

    public RecoveryKeyViewModel(
        ILanguageDictionary languageDictionary,
        IRecoveryKeyGenerator recoveryKeyGenerator,
        ILanguageHelper languageHelper,
        IAccount account,
        ITemporaryLanguageSelector languageSelector,
        ITimerTracker timerTracker) : base(
            languageDictionary,
            languageHelper,
            languageSelector)
    {
        _recoveryKeyGenerator = recoveryKeyGenerator;
        _account = account;

        TimerTracker = (TimerTracker)timerTracker;
        IsToolbarItemsVisible = IsLoggedIn();
        AccountUsername = account.Username;
    }

    [ObservableProperty]
    private TimerTracker _timerTracker;

    [ObservableProperty]
    private List<string> _recoveryKeys;

    [ObservableProperty]
    private bool _isToolbarItemsVisible;

    [ObservableProperty]
    private string _accountUsername;

    [RelayCommand]
    private async Task GenerateRecoveryKeyAsync()
    {
        var recoveryKeys = await _recoveryKeyGenerator.GetRecoveryKeysAsync((Account)_account);
        if (recoveryKeys?.Count > 0)
        {
            RecoveryKeys = new(recoveryKeys);
        }
        else
        {
            var generatedRecoveryKeys = await _recoveryKeyGenerator.GenerateRecoveryKeysAsync((Account)_account);
            RecoveryKeys = new(generatedRecoveryKeys);
        }
    }

    [RelayCommand]
    private async Task CopyToClipboardAsync()
    {
        try
        {
            string keysText = string.Join(Environment.NewLine, RecoveryKeys);

            await Clipboard.SetTextAsync(keysText);

            await Shell.Current.DisplayAlert("Text Copied", "The text has been copied to the clipboard.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public void StopTimer()
    {
        if (TimerTracker.Timer is null)
        {
            return;
        }

        TimerTracker.Timer.Stop();
        TimerTracker.Timer = null;

        TimerTracker.IsCountdownVisible = false;
        TimerTracker.SecondsRemaining = 0;
    }

    private bool IsLoggedIn()
    {
        if (_account is null)
        {
            return false;
        }

        if (_account.Id is 0 || string.IsNullOrWhiteSpace(_account.Username))
        {
            return false;
        }

        return true;
    }
}
