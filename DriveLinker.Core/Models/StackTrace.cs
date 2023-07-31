using DriveLinker.Core.Models.Interfaces;

namespace DriveLinker.Core.Models;
public partial class StackTrace : ObservableObject, IStackTrace
{
    [ObservableProperty]
    private List<string> _errorMessages;
}
