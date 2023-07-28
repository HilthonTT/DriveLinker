namespace DriveLinker.Core.Languages;
public class LanguageDictionary : ILanguageDictionary
{
    private readonly Account _account;
    private readonly ISettingsService _settingsService;
    private readonly IEnglishDictionary _english;
    private readonly IFrenchDictionary _french;
    private readonly IIndonesianDictionary _indonesian;
    private readonly IGermanDictionary _german;

    public LanguageDictionary(
        Account account,
        ISettingsService settingsService,
        IEnglishDictionary english,
        IFrenchDictionary french,
        IIndonesianDictionary indonesian,
        IGermanDictionary german)
    {
        _account = account;
        _settingsService = settingsService;
        _english = english;
        _french = french;
        _indonesian = indonesian;
        _german = german;
    }

    public Dictionary<Keyword, string> GetDictionary()
    {
        var settings = _settingsService.GetAccountSettings(_account.Id);

        return settings.Language switch
        {
            Language.English => _english.GetEnglishDictionary(),
            Language.French => _french.GetFrenchDictionary(),
            Language.German => _german.GetGermanDictionary(),
            Language.Indonesian => _indonesian.GetIndonesianDictionary(),
            _ => _english.GetEnglishDictionary(),
        };
    }
}
