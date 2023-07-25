namespace DriveLinker.Core.Languages.Interfaces;
public interface ILanguageDictionary
{
    Dictionary<Keyword, string> GetDictionary();
}