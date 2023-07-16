namespace DriveLinker.ViewModels;

public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;
    private readonly IAesEncryption _encryption;

    public DriveViewModel(
        IDriveService driveService,
        IAesEncryption encryption)
    {
        _driveService = driveService;
        _encryption = encryption;
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
        Drive.Password = await _encryption.DecryptAsync(Drive.Password, Drive.Key, Drive.Iv);
        Drive.UserName = await _encryption.DecryptAsync(Drive.UserName, Drive.Key, Drive.Iv);
        Drive.IpAddress = await _encryption.DecryptAsync(Drive.IpAddress, Drive.Key, Drive.Iv);
    }

    private static async Task<string> PromptUserAsync(string title, string message)
    {
        return await Shell.Current.DisplayPromptAsync(title, message);
    }

    [RelayCommand]
    private static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..", true);
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
