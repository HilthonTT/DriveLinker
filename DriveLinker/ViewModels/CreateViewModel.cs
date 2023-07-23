namespace DriveLinker.ViewModels;
public partial class CreateViewModel : BaseViewModel
{
    private readonly IDriveService _driveService;

    public CreateViewModel(
        IDriveService driveService,
        ISettingsService settingsService,
        IWindowsHelper windowsHelper,
        TimerTracker timerTracker)
        : base(settingsService, windowsHelper, timerTracker)
    {
        _driveService = driveService;
    }

    [ObservableProperty] 
    private CreateDriveModel _model = new();

    [ObservableProperty]
    private bool _clearEssentials = false;


    [RelayCommand]
    private async Task CreateDriveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (await IsDriveTakenAsync())
        {
            return;
        }

        if (await IsFieldNotFilled())
        {
            return;
        }

        var newDrive = new Drive()
        {
            Letter = Model.Letter,
            IpAddress = Model.IpAddress,
            DriveName = Model.DriveName,
            Password = Model.Password,
            UserName = Model.UserName,
        };

        await _driveService.CreateDriveAsync(newDrive);

        await FlushCreatePageAsync();
    }

    private async Task FlushCreatePageAsync()
    {
        if (ClearEssentials)
        {
            Model.DriveName = "";
            Model.Letter = "";
        }
        else
        {
            Model = new();
            await ClosePageAsync();
        }
    }

    private static async Task OpenSnackbarAsync(string text)
    {
        text = text.ToUpper();
        await Shell.Current.DisplayAlert("Error!", text, "Ok");
    }

    private async Task<bool> IsDriveTakenAsync()
    {
        var output = await _driveService.GetAllDrivesAsync();
        bool isLetterTaken = false;
        bool isDriveNameTaken = false;

        var driveWithLetter = output.FirstOrDefault(d => d.Letter == Model.Letter);
        if (driveWithLetter is not null)
        {
            Model.Letter = "";
            isLetterTaken = true;
        }

        var driveWithName = output.FirstOrDefault(d => d.DriveName == Model.DriveName);
        if (driveWithName is not null)
        {
            Model.DriveName = "";
            isDriveNameTaken = true;
        }
        
        if (isLetterTaken && isDriveNameTaken)
        {
            await OpenSnackbarAsync("Drive letter and drive name are both taken.");
            return true;
        }

        if (isDriveNameTaken)
        {
            await OpenSnackbarAsync("Drive name is already taken.");
            return true;
        }

        if (isLetterTaken)
        {
            await OpenSnackbarAsync("Drive letter is already taken.");
            return true;
        }

        return false;
    }

    private async Task<bool> IsFieldNotFilled()
    {
        if (string.IsNullOrWhiteSpace(Model.Letter))
        {
            await OpenSnackbarAsync("You didn't populate your letter field.");
            return true;
        }

        if (Model.Letter.Length > 1)
        {
            await OpenSnackbarAsync("Please enter a letter.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.IpAddress))
        {
            await OpenSnackbarAsync("You didn't populate your IP Address field.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.DriveName))
        {
            await OpenSnackbarAsync("You didn't populate your drive's name field.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.Password))
        {
            await OpenSnackbarAsync("You didn't populate your password's field.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.UserName))
        {
            await OpenSnackbarAsync("You didn't populate your username's field.");
            return true;
        }

        return false;
    }
}
