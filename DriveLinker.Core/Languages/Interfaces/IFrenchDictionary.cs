using DriveLinker.Core.Enums;

namespace DriveLinker.Core.Languages.Interfaces;
public interface IFrenchDictionary
{
    Dictionary<Keyword, string> GetFrenchDictionary();
}