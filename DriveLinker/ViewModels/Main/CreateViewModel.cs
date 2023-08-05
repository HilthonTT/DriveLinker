namespace DriveLinker.ViewModels.Main;
public partial class CreateViewModel : BaseViewModel
{
    private readonly IDriveService _driveService;
    private readonly IAccountService _accountService;
    private readonly IAuthentication _auth;

    public CreateViewModel(
        IDriveService driveService,
        IAccountService accountService,
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        ITimerTracker timerTracker)
        : base(
            languageDictionary,
            auth,
            timerTracker)
    {
        _driveService = driveService;
        _accountService = accountService;
        _auth = auth;
    }

    [ObservableProperty] 
    private CreateDriveModel _model = new();

    [ObservableProperty]
    private bool _clearEssentials = false;

    [ObservableProperty]
    private bool _isExitButtonEnabled = true;

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

        IsBusy = true;
        var newDrive = new Drive()
        {
            Letter = Model.Letter,
            IpAddress = Model.IpAddress,
            DriveName = Model.DriveName,
            Password = Model.Password,
            UserName = Model.UserName,
            AccountId = _auth.GetAccount().Id,
        };

        await _driveService.CreateDriveAsync(newDrive);

        await FlushCreatePageAsync();

        IsBusy = false;
    }

    [RelayCommand]
    private async Task ImportDrivesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsExitButtonEnabled = false;
        IsBusy = true;

        var options = GetPickOptions();
        var result = await FilePicker.Default.PickAsync(options);
        if (result is null)
        {
            ResetValues();
            return;
        }

        if (result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
        {
            string jsonifiedDrives = await File.ReadAllTextAsync(result.FullPath);
            var drives = JsonSerializer.Deserialize<List<Drive>>(jsonifiedDrives);

            bool answer = await Shell.Current.DisplayAlert(
                "Import file?", "Importing this file will delete all of your current drives.", YesLabel, NoLabel);

            if (answer)
            {
                await _accountService.DeleteAllAccountDrivesAsync();
                await _driveService.CreateAllDrivesAsync(drives);

                await FlushCreatePageAsync();
            }
            else
            {
                ResetValues();
            }
        }

        ResetValues();
    }

    private static PickOptions GetPickOptions()
    {
        string fileType = ".json";
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { fileType } },
                { DevicePlatform.Android, new[] { fileType } },
                { DevicePlatform.WinUI, new[] { fileType } },
                { DevicePlatform.Tizen, new[] { fileType } },
                { DevicePlatform.macOS, new[] { fileType } },
            });

        var options = new PickOptions()
        {
            PickerTitle = "Select the import file.",
            FileTypes = customFileType,
        };

        return options;
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

    private void ResetValues()
    {
        IsBusy = false;
        IsExitButtonEnabled = true;
    }

    private static async Task OpenSnackbarAsync(string text)
    {
        text = text.ToUpper();
        await Shell.Current.DisplayAlert("Error!", text, "Ok");
    }

    private async Task<bool> IsDriveTakenAsync()
    {
        var output = await _driveService.GetAllAccountDrivesAsync(_auth.GetAccount().Id);
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
