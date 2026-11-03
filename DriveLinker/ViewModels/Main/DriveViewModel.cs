namespace DriveLinker.ViewModels.Main;
public partial class DriveViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;

    public DriveViewModel(
        IDriveService driveService,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ITimerTracker timerTracker)
        : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _driveService = driveService;
    }

    [ObservableProperty]
    public partial Drive Drive { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Drives handed over by the main page are already decrypted by DriveService.
        Drive = query["Drive"] as Drive;
    }

    private static async Task<string> PromptUserAsync(string title, string message)
    {
        return await Shell.Current.DisplayPromptAsync(title, message);
    }

    [RelayCommand]
    private async Task DeleteRequestAsync()
    {
        bool isDelete = await Shell.Current.DisplayAlertAsync( 
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
