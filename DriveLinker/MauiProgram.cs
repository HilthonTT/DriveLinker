using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace DriveLinker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkitCore()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				fonts.AddFont("Tektur-Regular.ttf", "TekturRegular");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.ConfigureViewModels();
		builder.ConfigurePages();
		builder.ConfigureServices();
		builder.ConfigureLanguages();

		RegisterServices.ConfigureRouting();

		return builder.Build();
	}
}
