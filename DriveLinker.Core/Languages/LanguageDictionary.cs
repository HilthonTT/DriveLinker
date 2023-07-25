namespace DriveLinker.Core.Languages;
public class LanguageDictionary : ILanguageDictionary
{
    private readonly ISettingsService _settingsService;
    private readonly IEnglishDictionary _english;
    private readonly IFrenchDictionary _french;
    private readonly IIndonesianDictionary _indonesian;
    private readonly IGermanDictionary _german;
    private readonly Settings _settings;

    public LanguageDictionary(
        ISettingsService settingsService,
        IEnglishDictionary english,
        IFrenchDictionary french,
        IIndonesianDictionary indonesian,
        IGermanDictionary german)
    {
        _settingsService = settingsService;
        _english = english;
        _french = french;
        _indonesian = indonesian;
        _german = german;

        _settings = _settingsService.GetSettings();
    }

    public Dictionary<Keyword, string> GetDictionary()
    {
        return _settings.Language switch
        {
            Language.English => _english.GetEnglishDictionary(),
            Language.French => _french.GetFrenchDictionary(),
            Language.German => _german.GetGermanDictionary(),
            Language.Indonesian => _indonesian.GetIndonesianDictionary(),
            _ => _english.GetEnglishDictionary(),
        };
    }
}
