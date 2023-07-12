namespace DriveLinker.ViewModels;
public partial class MainViewModel : BaseViewModel
{
    private const bool Animate = true;

    [RelayCommand]
    private async Task LoadDrivePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(DrivesPage), Animate);
    }

    [RelayCommand]
    private async Task LoadSettingsPage()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), Animate);
    }

    [RelayCommand]
    private async Task LoadCreatePage()
    {
        await Shell.Current.GoToAsync(nameof(CreatePage), Animate);
    }
}
