namespace DriveLinker;
public static class RegisterServices
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IAccountService, AccountService>();
        builder.Services.AddTransient<IDriveService, DriveService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();
        builder.Services.AddTransient<IDummyService, DummyService>();
        builder.Services.AddTransient<ILanguageService, LanguageService>();

        // Misc
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<Account>();
        builder.Services.AddSingleton<TimerTracker>();

        builder.Services.AddTransient<IAesEncryption, AesEncryption>();
        builder.Services.AddTransient<IAuthentication, Authentication>();
        builder.Services.AddTransient<ILinker, Linker>();
        builder.Services.AddTransient<IWindowsHelper, WindowsHelper>();
    }

    public static void ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<CreateViewModel>();
        builder.Services.AddTransient<DriveViewModel>();
        builder.Services.AddTransient<PasswordViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
    }

    public static void ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<AuthPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CreatePage>();
        builder.Services.AddTransient<DrivePage>();
        builder.Services.AddTransient<PasswordPage>();
        builder.Services.AddTransient<RegisterPage>();
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
    }
}
