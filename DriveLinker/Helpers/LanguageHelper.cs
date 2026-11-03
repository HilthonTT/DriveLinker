namespace DriveLinker.Helpers;
public partial class LanguageHelper : ObservableObject, ILanguageHelper
{
    private readonly ILanguageDictionary _languageDictionary;

    public LanguageHelper(ILanguageDictionary languageDictionary)
    {
        _languageDictionary = languageDictionary;

        InitializeDictionary().FireAndForget();
    }

    [ObservableProperty]
    public partial string EnglishLabel { get; set; }

    [ObservableProperty]
    public partial string FrenchLabel { get; set; }

    [ObservableProperty]
    public partial string GermanLabel { get; set; }

    [ObservableProperty]
    public partial string IndonesianLabel { get; set; }

    private async Task InitializeDictionary()
    {
        var keywords = await _languageDictionary.GetDictionaryAsync();

        EnglishLabel = keywords[Keyword.English];
        FrenchLabel = keywords[Keyword.French];
        GermanLabel = keywords[Keyword.German];
        IndonesianLabel = keywords[Keyword.Indonesian];
    }

    public async Task<List<string>> GetStringifiedLanguagesAsync(ObservableCollection<Language> languages)
    {
        await InitializeDictionary();
        var stringifiedLanguages = new List<string>();

        foreach (var language in languages)
        {
            switch (language)
            {
                case Language.English:
                    stringifiedLanguages.Add(EnglishLabel);
                    break;

                case Language.French:
                    stringifiedLanguages.Add(FrenchLabel);
                    break;

                case Language.German:
                    stringifiedLanguages.Add(GermanLabel);
                    break;

                case Language.Indonesian:
                    stringifiedLanguages.Add(IndonesianLabel);
                    break;
                default:
                    break;
            }
        }

        return stringifiedLanguages;
    }

    public Language GetLanguage(string selectedLanguage)
    {
        if (selectedLanguage == EnglishLabel)
        {
            return Language.English;
        }
        else if (selectedLanguage == FrenchLabel)
        {
            return Language.French;
        }
        else if (selectedLanguage == GermanLabel)
        {
            return Language.German;
        }
        else if (selectedLanguage == IndonesianLabel)
        {
            return Language.Indonesian;
        }

        return Language.English;
    }

    public string GetLanguageString(Language language)
    {
        return language switch
        {
            Language.English => EnglishLabel,
            Language.French => FrenchLabel,
            Language.German => GermanLabel,
            Language.Indonesian => IndonesianLabel,
            _ => EnglishLabel,
        };
    }
}
