namespace DriveLinker.ViewModels.Main;
public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;
    private readonly IAesEncryption _encryption;
    private readonly IMemoryCache _cache;

    public DriveViewModel(
        IDriveService driveService,
        IAesEncryption encryption,
        IMemoryCache cache, 
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        ILanguageDictionary languageDictionary,
        Account account,
        TimerTracker timerTracker)
        : base(
            settingsService,
            windowsHelper,
            languageDictionary,
            account,
            timerTracker)
    {
        _driveService = driveService;
        _encryption = encryption;
        _cache = cache;
    }

    [ObservableProperty] 
    private Drive _drive;

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Drive = query["Drive"] as Drive;
        await DecryptInformationAsync();
    }

    private async Task DecryptInformationAsync()
    {
        string key = $"IsDecrypted-{Drive.Id}";
        bool? isDecrypted = _cache.Get<bool>(key);
        if (isDecrypted is null)
        {
            Drive.Password = await _encryption.AesDecryptAsync(Drive.Password, Drive.Key, Drive.Iv);
            Drive.UserName = await _encryption.AesDecryptAsync(Drive.UserName, Drive.Key, Drive.Iv);
            Drive.IpAddress = await _encryption.AesDecryptAsync(Drive.IpAddress, Drive.Key, Drive.Iv);

            isDecrypted = true;
            _cache.Set(key, isDecrypted);
        }
    }

    private static async Task<string> PromptUserAsync(string title, string message)
    {
        return await Shell.Current.DisplayPromptAsync(title, message);
    }

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlert( 
            "Delete Drive?", 
            "Deleting a drive is irreversible.", 
            "Yes", 
            "No");

        if (isDelete)
        {
            await _driveService.DeleteDriveAsync(Drive);
            await ClosePageAsync();
        }
    }

    [RelayCommand]
    private async Task UpdateLetterAsync()
    {
        string result = await PromptUserAsync("Edit Letter?", "What's your new letter?");

        if (string.IsNullOrWhiteSpace(result) is false && Drive.Letter.Length == 1)
        {
            Drive.Letter = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateDriveNameAsync()
    {
        string result = await PromptUserAsync("Edit Drive Name?", "What's your new drive name?");

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.DriveName = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateIpAddressAsync()
    {
        string result = await PromptUserAsync("Edit IP Address?", "What's your new IP Address?");

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.IpAddress = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdatePasswordAsync()
    {
        string result = await PromptUserAsync("Edit Password?", "What's your new Password?");

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.Password = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateUserNameAsync()
    {
        string result = await PromptUserAsync("Edit Username?", "What's your new Username?");

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.UserName = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }
}
