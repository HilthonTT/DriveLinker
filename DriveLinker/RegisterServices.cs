﻿namespace DriveLinker;
public static class RegisterServices
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IAccountService, AccountService>();
        builder.Services.AddTransient<IDriveService, DriveService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();

        // Misc
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ITimerTracker, TimerTracker>();
        builder.Services.AddSingleton<IAccount, Account>();
        builder.Services.AddSingleton<ITemporaryLanguageSelector, TemporaryLanguageSelector>();
        builder.Services.AddSingleton<IStackTrace, StackTrace>();

        builder.Services.AddTransient<IEncryption, Encryption>();
        builder.Services.AddTransient<IAuthentication, Authentication>();
        builder.Services.AddTransient<IPasswordGenerator, PasswordGenerator>();
        builder.Services.AddTransient<IRecoveryKeyGenerator, RecoveryKeyGenerator>();
        builder.Services.AddTransient<ILinker, Linker>();
        builder.Services.AddTransient<IWindowsHelper, WindowsHelper>();
        builder.Services.AddTransient<ILanguageHelper, LanguageHelper>();
    }

    public static void ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<AccountViewModel>();
        builder.Services.AddTransient<PasswordResetViewModel>();
        builder.Services.AddTransient<UsernameResetViewModel>();
        builder.Services.AddTransient<StackTraceViewModel>();
        builder.Services.AddTransient<AuthBaseViewModel>();
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<CreateViewModel>();
        builder.Services.AddTransient<DriveViewModel>();
        builder.Services.AddTransient<PasswordViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RecoveryKeyViewModel>();
        builder.Services.AddTransient<UpdateViewModel>();
    }

    public static void ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<AccountPage>();
        builder.Services.AddTransient<PasswordResetPage>();
        builder.Services.AddTransient<UsernameResetPage>();
        builder.Services.AddTransient<StackTracePage>();
        builder.Services.AddTransient<AuthPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CreatePage>();
        builder.Services.AddTransient<DrivePage>();
        builder.Services.AddTransient<PasswordPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<RecoveryKeyPage>();
        builder.Services.AddTransient<UpdatePage>();
    }

    public static void ConfigureLanguages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ILanguageDictionary, LanguageDictionary>();

        builder.Services.AddSingleton<IEnglishDictionary, EnglishDictionary>();
        builder.Services.AddSingleton<IFrenchDictionary, FrenchDictionary>();
        builder.Services.AddSingleton<IGermanDictionary, GermanDictionary>();
        builder.Services.AddSingleton<IIndonesianDictionary, IndonesianDictionary>();
    }

    public static void ConfigureRouting()
    {
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(CreatePage), typeof(CreatePage));
        Routing.RegisterRoute(nameof(DrivePage), typeof(DrivePage));
        Routing.RegisterRoute(nameof(PasswordPage), typeof(PasswordPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
        Routing.RegisterRoute(nameof(RecoveryKeyPage), typeof(RecoveryKeyPage));
        Routing.RegisterRoute(nameof(StackTracePage), typeof(StackTracePage));
        Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
        Routing.RegisterRoute(nameof(PasswordResetPage), typeof(PasswordResetPage));
        Routing.RegisterRoute(nameof(UsernameResetPage), typeof(UsernameResetPage));
        Routing.RegisterRoute(nameof(UpdatePage), typeof(UpdatePage));
    }
}
