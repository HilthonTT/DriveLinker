namespace DriveLinker.Core.Models;
public partial class TemporaryLanguageSelector : ObservableObject, ITemporaryLanguageSelector
{
    [ObservableProperty]
    private Language _selectedLanguage = Language.English;
}
