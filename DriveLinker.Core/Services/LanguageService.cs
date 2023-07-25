namespace DriveLinker.Core.Services;
public class LanguageService : ILanguageService
{
    public List<Language> GetLanguages()
    {
        return new List<Language>
        {
            Language.English,
            Language.French,
            Language.German,
            Language.Indonesian
        };
    }
}
