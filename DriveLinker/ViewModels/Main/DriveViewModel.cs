namespace DriveLinker.ViewModels.Main;
public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;
    private readonly IEncryption _encryption;
    private readonly IMemoryCache _cache;

    public DriveViewModel(
        IDriveService driveService,
        IEncryption encryption,
        IMemoryCache cache, 
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker)
        : base(
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
            DeleteDriveLabel, DeleteDriveWarningLabel, YesLabel, NoLabel);

        if (isDelete)
        {
            await _driveService.DeleteDriveAsync(Drive);
            await ClosePageAsync();
        }
    }

    [RelayCommand]
    private async Task UpdateLetterAsync()
    {
        string result = await PromptUserAsync(EditLetterLabel, LetterDescLabel);

        if (string.IsNullOrWhiteSpace(result) is false && result.Length == 1)
        {
            Drive.Letter = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateDriveNameAsync()
    {
        string result = await PromptUserAsync(EditDriveNameLabel, EditDriveNameDescLabel);

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.DriveName = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateIpAddressAsync()
    {
        string result = await PromptUserAsync(EditIpAddressLabel, EditIpAddressDescLabel);

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.IpAddress = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdatePasswordAsync()
    {
        string result = await PromptUserAsync(EditPasswordLabel, EditPasswordDescLabel);

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.Password = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }

    [RelayCommand]
    private async Task UpdateUserNameAsync()
    {
        string result = await PromptUserAsync(EditUsernameLabel, EditUserNameDescLabel);

        if (string.IsNullOrWhiteSpace(result) is false)
        {
            Drive.UserName = result;
            await _driveService.UpdateDriveAsync(Drive);
        }
    }
}
