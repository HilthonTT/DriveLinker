namespace DriveLinker.Core.Languages.Interfaces;
public interface ILanguageDictionary
{
    Task<Dictionary<Keyword, string>> GetDictionaryAsync();
}