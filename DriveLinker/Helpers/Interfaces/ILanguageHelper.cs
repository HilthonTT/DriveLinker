namespace DriveLinker.Helpers.Interfaces;
public interface ILanguageHelper
{
    Language GetLanguage(string selectedLanguage);
    string GetLanguageString(Language language);
    List<string> GetStringifiedLanguages(ObservableCollection<Language> languages);
}