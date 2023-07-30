namespace DriveLinker.Core.Languages.Interfaces;
public interface ILanguageDictionary
{
    Task<Dictionary<Keyword, string>> GetDictionaryAsync();
    Dictionary<Keyword, string> GetDictionaryWithEnum(Language language);
    List<Language> GetLanguages();
}