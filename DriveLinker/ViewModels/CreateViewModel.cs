using CommunityToolkit.Maui.Alerts;
using DriveLinker.Helpers;

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

    [ObservableProperty] private CreateDriveModel _model = new();

    [RelayCommand]
    private async Task CreateDriveAsync()
    {
        if (IsBusy)
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

        Model = new();
        await ClosePageAsync();
    }

    private static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private static async Task OpenSnackbarAsync(string text)
    {
        string actionButtonText = "Dismiss";
        var duration = TimeSpan.FromSeconds(3);
        var snackbar = Snackbar.Make(text, null, actionButtonText, duration);

        await snackbar.Show();
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
