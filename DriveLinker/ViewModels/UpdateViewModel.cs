using CommunityToolkit.Maui.Alerts;
using DriveLinker.Models;

namespace DriveLinker.ViewModels;

public partial class UpdateViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDriveService _driveService;

    public UpdateViewModel(IDriveService driveService)
    {
        _driveService = driveService;
    }

    [ObservableProperty]
    private Drive _drive = new();

    [ObservableProperty]
    private CreateDriveModel _model = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Drive = query["Drive"] as Drive;
        Model = new(Drive);
    }

    [RelayCommand]
    private static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task UpdateDriveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (await IsFieldNotFilled())
        {
            return;
        }

        Drive.Letter = Model.Letter;
        Drive.IpAddress = Model.IpAddress;
        Drive.DriveName = Model.DriveName;
        Drive.Password = Model.Password;
        Drive.UserName = Model.UserName;

        await _driveService.UpdateDriveAsync(Drive);
        await ClosePageAsync();
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
