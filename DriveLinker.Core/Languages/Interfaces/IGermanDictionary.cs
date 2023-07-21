using DriveLinker.Core.Enums;

namespace DriveLinker.Core.Languages.Interfaces;
public interface IGermanDictionary
{
    Dictionary<Keyword, string> GetGermanDictionary();
}