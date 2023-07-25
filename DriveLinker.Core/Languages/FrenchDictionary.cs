namespace DriveLinker.Core.Languages;
public class FrenchDictionary : IFrenchDictionary
{
    private const string CacheName = nameof(FrenchDictionary);
    private readonly IMemoryCache _cache;

    public FrenchDictionary(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<Keyword, string> GetFrenchDictionary()
    {
        var output = _cache.Get<Dictionary<Keyword, string>>(CacheName);
        if (output is null)
        {
            output = GetDictionary();
            _cache.Set(CacheName, output);
        }

        return output;
    }

    private static Dictionary<Keyword, string> GetDictionary()
    {
        return new Dictionary<Keyword, string>()
        {
            { Keyword.Countdown, "Compte à rebours" },
            { Keyword.CreateADrive, "Créer un disque" },
            { Keyword.Settings, "Paramètres" },
            { Keyword.Search, "Rechercher" },
            { Keyword.RecentlyAdded, "Disques récemment ajoutés" },
            { Keyword.DriveListing, "Liste de disques" },
            { Keyword.LinkDrives, "Lier des disques" },
            { Keyword.UnlinkDrives, "Délier des disques" },
            { Keyword.Letter, "Lettre" },
            { Keyword.LetterDesc, "Entrez la lettre de votre disque." },
            { Keyword.IpAddress, "Adresse IP" },
            { Keyword.IpAddressDesc, "Entrez l'adresse IP de votre disque." },
            { Keyword.DriveName, "Nom du disque" },
            { Keyword.DriveNameDesc, "Entrez le nom de votre disque." },
            { Keyword.Password, "Mot de passe" },
            { Keyword.PasswordDesc, "Entrez le mot de passe de votre disque." },
            { Keyword.UserName, "Nom d'utilisateur" },
            { Keyword.UserNameDesc, "Entrez le nom d'utilisateur de votre disque." },
            { Keyword.DateCreated, " Date de création" },
            { Keyword.Create, "Créer" },
            { Keyword.AutoLinkDrives, "Lier automatiquement les disques au démarrage" },
            { Keyword.AutoMinimize, "Réduire automatiquement l'application" },
            { Keyword.ClearOnlyLetterDriveName, "Effacer uniquement la lettre / le nom du lecteur" },
            { Keyword.Save, "Sauvegarder" },
            { Keyword.Language, "Langue" },
            { Keyword.English, "Anglais" },
            { Keyword.French, "Français" },
            { Keyword.German, "Allemand" },
            { Keyword.Indonesian, "Indonésien" },
        };
    }
}
