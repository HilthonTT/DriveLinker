namespace DriveLinker.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] 
    private bool _isBusy;

    [RelayCommand]
    public static async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
