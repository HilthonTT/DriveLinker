﻿namespace DriveLinker.ViewModels.Authentication;

public partial class RegisterViewModel : AuthBaseViewModel
{
    private readonly IAuthentication _auth;
    private readonly IAccountService _accountService;
    private readonly IAccount _account;

    public RegisterViewModel(
        ILanguageDictionary languageDictionary,
        IAuthentication auth,
        IAccountService accountService,
        ILanguageHelper languageHelper,
        IAccount account,
        ITemporaryLanguageSelector languageSelector) : base(
            languageDictionary,
            languageHelper,
            languageSelector)
    {
        _auth = auth;
        _accountService = accountService;
        _account = account;
    }

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private bool _dontShowPassword = true;

    [RelayCommand]
    private void ToggleShowPassword()
    {
        DontShowPassword = !DontShowPassword;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
        {
            return;
        }

        if (await AccountExistsAsync())
        {
            await Shell.Current.DisplayAlert(
                "Error.", "An account with this username already exists.", "OK");
            return;
        }

        bool savePassword = await DisplaySavePassword();
        if (savePassword)
        {
            var account = await _accountService.CreateAccountAsync(new() { Username = Username });
            await _auth.ChangePasswordAsync(Username, Password);
            await LoadRecoveryPageAsync(account);
        }

        Username = "";
        Password = "";
    }

    private async Task<bool> DisplaySavePassword()
    {
        bool savePassword = await Shell.Current.DisplayAlert(
            "Save Password?", $"Your password is {Password}, would you like to save it?", "Save", "Cancel");

        return savePassword;
    }

    private async Task<bool> AccountExistsAsync()
    {
        var account = await _accountService.GetAccountByUsernameAsync(Username);

        if (account is not null)
        {
            return true;
        }

        return false;
    }

    private static async Task LoadRecoveryPageAsync(Account account)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Account", account },
        };

        await Shell.Current.GoToAsync(nameof(RecoveryKeyPage), true, parameters);
    }
}
