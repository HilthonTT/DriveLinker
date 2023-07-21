using DriveLinker.Core.Enums;

namespace DriveLinker.Core.Languages.Interfaces;
public interface IEnglishDictionary
{
    Dictionary<Keyword, string> GetEnglishDictionary();
}