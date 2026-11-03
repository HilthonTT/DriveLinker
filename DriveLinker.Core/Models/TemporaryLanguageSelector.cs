namespace DriveLinker.Core.Models;
public partial class TemporaryLanguageSelector : ObservableObject, ITemporaryLanguageSelector
{
    [ObservableProperty]
    public partial Language SelectedLanguage { get; set; } = Language.English;
}
