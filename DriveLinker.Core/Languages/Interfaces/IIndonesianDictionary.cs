using DriveLinker.Core.Enums;

namespace DriveLinker.Core.Languages.Interfaces;
public interface IIndonesianDictionary
{
    Dictionary<Keyword, string> GetIndonesianDictionary();
}