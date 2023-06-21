using Microsoft.Extensions.Logging;
using HeartRateDemo.Services;
using Shiny.Infrastructure;

namespace HeartRateDemo;

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

		builder.Services.UseShiny();
		builder.Services.AddLocal();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void UseShiny(this IServiceCollection services)
	{
		services.AddShinyCoreServices();
		services.AddBluetoothLE();
	}

	private static void AddLocal(this IServiceCollection services)
	{
		services.AddSingleton(typeof(HeartRateMonitorScanner));
		services.AddSingleton(typeof(HeartRateMonitorClient));
		services.AddTransient<MainPage>();
	}
}
