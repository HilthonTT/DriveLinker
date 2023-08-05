namespace DriveLinker.Core.Languages;
public class EnglishDictionary : IEnglishDictionary
{
    private const string CacheName = nameof(EnglishDictionary);
    private readonly IMemoryCache _cache;

    public EnglishDictionary(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<Keyword, string> GetEnglishDictionary()
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
            { Keyword.Countdown, "Countdown" },
            { Keyword.CreateADrive, "Create A Drive" },
            { Keyword.Settings, "Settings" },
            { Keyword.Search, "Search" },
            { Keyword.RecentlyAdded, "Recently Added Drives" },
            { Keyword.DriveListing, "Drive Listing" },
            { Keyword.LinkDrives, "Link Drives" },
            { Keyword.UnlinkDrives, "Unlink Drives" },
            { Keyword.Letter, "Letter" },
            { Keyword.LetterDesc, "Enter your drive's letter." },
            { Keyword.IpAddress, "IP Address" },
            { Keyword.IpAddressDesc, "Enter your drives's IP Address." },
            { Keyword.DriveName, "Drive Name" },
            { Keyword.DriveNameDesc, "Enter your drive's name." },
            { Keyword.Password, "Password" },
            { Keyword.PasswordDesc, "Enter your drive's password." },
            { Keyword.UserName, "Username" },
            { Keyword.UserNameDesc, "Enter your drive's username." },
            { Keyword.DateCreated, "Date Created" },
            { Keyword.Create, "Create" },
            { Keyword.AutoLinkDrives, "Automatically Link Drives On Launch" },
            { Keyword.AutoMinimize, "Automatically Minimize App" },
            { Keyword.ClearOnlyLetterDriveName, "Clear Only Letter/Drive Name" },
            { Keyword.Save, "Save" },
            { Keyword.Language, "Language" },
            { Keyword.English, "English" },
            { Keyword.French, "Français" },
            { Keyword.German, "Deutsch" },
            { Keyword.Indonesian, "Indonesia" },
            { Keyword.DontHaveAnAccount, "Don't have an account?" },
            { Keyword.ForgotMyPassword, "Forgot my password" },
            { Keyword.Login, "Login" },
            { Keyword.RecoveryKey, "Recovery Keys" },
            { Keyword.RecoveryKeyDesc, "Enter one of your recovery keys" },
            { Keyword.RecoveryKeyHelperText, "You can use them if you ever forget your password." },
            { Keyword.Copyclipboard, "Copy to clipboard" },
            { Keyword.Register, "Register" },
            { Keyword.Error,"Error" },
            { Keyword.Ok, "OK" },
            { Keyword.LetterAndDriveNameTaken, "Drive letter and drive name are both taken." },
            { Keyword.LetterTaken, "Drive letter is already taken" },
            { Keyword.DriveNameTaken, "Drive name is already taken." },
            { Keyword.LetterNotPopulated, "You didn't populate your letter field." },
            { Keyword.EnterALetter, "Please enter a letter." },
            { Keyword.IpAddressNotPopulated, "You didn't populate your IP Address field." },
            { Keyword.DriveNameNotPopulated, "You didn't populate your drive's name field." },
            { Keyword.PasswordNotPopulated, "You didn't populate your password's field." },
            { Keyword.UserNameNotPopulated, "You didn't populate your username's field." },
            { Keyword.DeleteDrive, "Delete Drive?" },
            { Keyword.DeleteDriveWarning, "Deleting a drive is irreversible." },
            { Keyword.Yes, "Yes" },
            { Keyword.No, "No" },
            { Keyword.EditLetter, "Edit Letter?" },
            { Keyword.EditLetterDesc, "What's your new letter?" },
            { Keyword.EditDriveName, "Edit Drive Name?" },
            { Keyword.EditDriveNameDesc, "What's your new drive name?" },
            { Keyword.EditIpAddress, "Edit IP Address?" },
            { Keyword.EditIpAddressDesc, "What's your new IP Address?" },
            { Keyword.EditPassword, "Edit Password?" },
            { Keyword.EditPasswordDesc, "What's your new Password?" },
            { Keyword.EditUsername, "Edit Username?" },
            { Keyword.EditUsernameDesc, "What's your new Username?" },
            { Keyword.Seconds, " seconds" },
            { Keyword.TimerCountMinWarning, $"Your timer count can't be less than " },
            { Keyword.TimerCountMaxWarning, $"Your timer count can't be more than " },
            { Keyword.HomePage, "Home" },
            { Keyword.Logout, "Logout" },
            { Keyword.DriveInfo, "Drive Information" },
            { Keyword.Stacktrace, "Stacktrace" },
            { Keyword.ErrorCount, "Error Count: " },
            { Keyword.DeleteAccount, "Delete Account." },
            { Keyword.DeleteAccountWarning, "Are you sure you want to delete your account? This is irreversible." },
            { Keyword.CurrentPassword, "Current Password" },
            { Keyword.WrongRecoveryKey, "Invalid recovery key!" },
            { Keyword.WrongRecoveryKeyDesc, "The recovery key you've enter is wrong!" },
            { Keyword.UsernameTaken, "An account with this username already exists." },
            { Keyword.UsernameDoesntExist, "Your account's username doesn't exists." },
            { Keyword.WrongUsername, "Invalid username!" },
            { Keyword.WrongUsernameDesc, "Your account's username doesn't exists." },
            { Keyword.WrongPassword, "Invalid password!" },
            { Keyword.WrongPasswordDesc, "The password you've enter is wrong." },
            { Keyword.TextCopied, "Text Copied!" },
            { Keyword.TextCopiedDesc, "The text has been copied to the clipboard." },
            { Keyword.RegisterDesc, "You will be known as: " },
            { Keyword.Account, "Account" },
            { Keyword.ExportDrives, "Export Drives" },
            { Keyword.UpdateDrives, "Update Drives" },
            { Keyword.DeleteDrives, "Delete Drives" },
            { Keyword.ImportDrives, "Import Drives" },
            { Keyword.Update, "Update" },
            { Keyword.DeleteAllDrives, "Delete All Drives?" },
            { Keyword.DeleteAllDrivesDesc, "Deleting all the drives is irreversible." },
            { Keyword.ImportFile, "Import File?" },
            { Keyword.ImportFileDesc, "Importing the file will delete all of your drives." },
        };
    }
}
