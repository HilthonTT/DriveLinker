namespace DriveLinker.Helpers.Interfaces;
public interface ILanguageHelper
{
    Language GetLanguage(string selectedLanguage);
    string GetLanguageString(Language language);
    Task<List<string>> GetStringifiedLanguagesAsync(ObservableCollection<Language> languages);
}