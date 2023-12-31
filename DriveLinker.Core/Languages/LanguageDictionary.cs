﻿namespace DriveLinker.Core.Languages;
public class LanguageDictionary : ILanguageDictionary
{
    private readonly IAccount _account;
    private readonly ITemporaryLanguageSelector _languageSelector;
    private readonly ISettingsService _settingsService;
    private readonly IEnglishDictionary _english;
    private readonly IFrenchDictionary _french;
    private readonly IIndonesianDictionary _indonesian;
    private readonly IGermanDictionary _german;

    public LanguageDictionary(
        IAccount account,
        ITemporaryLanguageSelector languageSelector,
        ISettingsService settingsService,
        IEnglishDictionary english,
        IFrenchDictionary french,
        IIndonesianDictionary indonesian,
        IGermanDictionary german)
    {
        _account = account;
        _languageSelector = languageSelector;
        _settingsService = settingsService;
        _english = english;
        _french = french;
        _indonesian = indonesian;
        _german = german;
    }

    public async Task<Dictionary<Keyword, string>> GetDictionaryAsync()
    {
        Language language;

        var settings = await _settingsService.GetAccountSettingsAsync(_account.Id);

        if (settings?.Id is 0 || _account.Id is 0)
        {
            language = _languageSelector.SelectedLanguage; 
        }
        else
        {
            language = settings.Language;
        }

        return language switch
        {
            Language.English => _english.GetEnglishDictionary(),
            Language.French => _french.GetFrenchDictionary(),
            Language.German => _german.GetGermanDictionary(),
            Language.Indonesian => _indonesian.GetIndonesianDictionary(),
            _ => _english.GetEnglishDictionary(),
        };
    }

    public Dictionary<Keyword, string> GetDictionaryWithEnum(Language language)
    {
        return language switch
        {
            Language.English => _english.GetEnglishDictionary(),
            Language.French => _french.GetFrenchDictionary(),
            Language.German => _german.GetGermanDictionary(),
            Language.Indonesian => _indonesian.GetIndonesianDictionary(),
            _ => _english.GetEnglishDictionary(),
        };
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
