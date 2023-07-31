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
        return new()
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
            { Keyword.English, "English" },
            { Keyword.French, "Français" },
            { Keyword.German, "Deutsch" },
            { Keyword.Indonesian, "Indonesia" },
            { Keyword.DontHaveAnAccount, "Vous n'avez pas de compte ?" },
            { Keyword.ForgotMyPassword, "J'ai oublié mon mot de passe" },
            { Keyword.Login, "Se connecter" },
            { Keyword.RecoveryKey, "Clé de récupération" },
            { Keyword.RecoveryKeyDesc, "Entrez l'une de vos clés de récupération" },
            { Keyword.RecoveryKeyHelperText, "Vous pouvez les utiliser si vous oubliez votre mot de passe." },
            { Keyword.Copyclipboard, "Copier dans le presse-papiers" },
            { Keyword.Register, "S'inscrire" },
            { Keyword.Error, "Erreur" },
            { Keyword.Ok, "OK" },
            { Keyword.LetterAndDriveNameTaken, "La lettre de lecteur et le nom du lecteur sont déjà utilisés." },
            { Keyword.LetterTaken, "La lettre de lecteur est déjà utilisée." },
            { Keyword.DriveNameTaken, "Le nom du lecteur est déjà utilisé." },
            { Keyword.LetterNotPopulated, "Vous n'avez pas rempli le champ de lettre." },
            { Keyword.EnterALetter, "Veuillez entrer une lettre." },
            { Keyword.IpAddressNotPopulated, "Vous n'avez pas rempli le champ d'adresse IP." },
            { Keyword.DriveNameNotPopulated, "Vous n'avez pas rempli le champ du nom du disque." },
            { Keyword.PasswordNotPopulated, "Vous n'avez pas rempli le champ du mot de passe." },
            { Keyword.UserNameNotPopulated, "Vous n'avez pas rempli le champ du nom d'utilisateur." },
            { Keyword.DeleteDrive, "Supprimer le lecteur ?" },
            { Keyword.DeleteDriveWarning, "La suppression d'un lecteur est irréversible." },
            { Keyword.Yes, "Oui" },
            { Keyword.No, "Non" },
            { Keyword.EditLetter, "Modifier la lettre ?" },
            { Keyword.EditLetterDesc, "Quelle est votre nouvelle lettre ?" },
            { Keyword.EditDriveName, "Modifier le nom du lecteur ?" },
            { Keyword.EditDriveNameDesc, "Quel est votre nouveau nom de lecteur ?" },
            { Keyword.EditIpAddress, "Modifier l'adresse IP ?" },
            { Keyword.EditIpAddressDesc, "Quelle est votre nouvelle adresse IP ?" },
            { Keyword.EditPassword, "Modifier le mot de passe ?" },
            { Keyword.EditPasswordDesc, "Quel est votre nouveau mot de passe ?" },
            { Keyword.EditUsername, "Modifier le nom d'utilisateur ?" },
            { Keyword.EditUsernameDesc, "Quel est votre nouveau nom d'utilisateur ?" },
            { Keyword.Seconds, " secondes" },
            { Keyword.TimerCountMinWarning, $"Votre compte à rebours ne peut pas être inférieur à " },
            { Keyword.TimerCountMaxWarning, $"Votre compte à rebours ne peut pas être supérieur à " },
            { Keyword.HomePage, "Accueil" },
            { Keyword.Logout, "Déconnexion" },
            { Keyword.DriveInfo, "Informations du Disque" },
            { Keyword.Stacktrace, "Trace de la pile" },
            { Keyword.ErrorCount, "Nombre d'erreurs : " },
        };
    }
}
