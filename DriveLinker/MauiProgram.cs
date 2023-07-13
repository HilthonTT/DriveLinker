namespace DriveLinker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.UseMauiCommunityToolkitCore();
        builder.UseMauiCommunityToolkit();

		builder.Services.AddMemoryCache();

		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<DrivesViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		builder.Services.AddTransient<CreateViewModel>();
		builder.Services.AddTransient<DriveViewModel>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DrivesPage>();
		builder.Services.AddTransient<SettingsPage>();
		builder.Services.AddTransient<CreatePage>();
		builder.Services.AddTransient<DrivePage>();

		builder.Services.AddTransient<IDriveService, DriveService>();
		builder.Services.AddTransient<ISettingsService, SettingsService>();
		builder.Services.AddTransient<IDummyService, DummyService>();

		builder.Services.AddTransient<IAesEncryption, AesEncryption>();
		builder.Services.AddTransient<ILinker, Linker>();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(DrivesPage), typeof(DrivesPage));
		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
		Routing.RegisterRoute(nameof(CreatePage), typeof(CreatePage));
		Routing.RegisterRoute(nameof(DrivePage), typeof(DrivePage));

		return builder.Build();
	}
}
