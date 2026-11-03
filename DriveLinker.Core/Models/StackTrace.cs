namespace DriveLinker.Core.Models;
public partial class StackTrace : ObservableObject, IStackTrace
{
    [ObservableProperty]
    public partial List<string> ErrorMessages { get; set; } = new();
}
