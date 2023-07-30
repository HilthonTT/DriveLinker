namespace DriveLinker.Core.Models;
public partial class TemporaryLanguageSelector : ObservableObject
{
    [ObservableProperty]
    private Language _selectedLanguage = Language.English;
}
