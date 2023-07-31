namespace DriveLinker.ViewModels.Main;
public partial class CreateViewModel : BaseViewModel
{
    private readonly IDriveService _driveService;
    private readonly IAccount _account;

    public CreateViewModel(
        IDriveService driveService,
        ILanguageDictionary languageDictionary,
        IAccount account,
        ITimerTracker timerTracker)
        : base(
            languageDictionary,
            account,
            timerTracker)
    {
        _driveService = driveService;
        _account = account;
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
            AccountId = _account.Id,
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
        var output = await _driveService.GetAllAccountDrivesAsync(_account.Id);
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
            await OpenSnackbarAsync(LetterAndDriveNameTakenLabel);
            return true;
        }

        if (isDriveNameTaken)
        {
            await OpenSnackbarAsync(DriveNameTakenLabel);
            return true;
        }

        if (isLetterTaken)
        {
            await OpenSnackbarAsync(LetterTakenLabel);
            return true;
        }

        return false;
    }

    private async Task<bool> IsFieldNotFilled()
    {
        if (string.IsNullOrWhiteSpace(Model.Letter))
        {
            await OpenSnackbarAsync(LetterNotPopulatedLabel);
            return true;
        }

        if (Model.Letter.Length > 1)
        {
            await OpenSnackbarAsync(EnterALetterLabel);
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.IpAddress))
        {
            await OpenSnackbarAsync(IpAddressNotPopulated);
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.DriveName))
        {
            await OpenSnackbarAsync(DriveNameNotPopulated);
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.Password))
        {
            await OpenSnackbarAsync(PasswordNotPopulated);
            return true;
        }

        if (string.IsNullOrWhiteSpace(Model.UserName))
        {
            await OpenSnackbarAsync(UsernameNotPopulated);
            return true;
        }

        return false;
    }
}
