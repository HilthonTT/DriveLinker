using DriveLinker.Core.Enums;
using DriveLinker.Core.Services.Interfaces;

namespace DriveLinker.Core.Services;
public class LanguageService : ILanguageService
{
    private readonly ISettingsService _settingsService;

    public LanguageService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

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
