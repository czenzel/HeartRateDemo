using Microsoft.Extensions.Logging;
using HeartRateDemo.Services;
using HeartRateDemo.Interfaces;
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
		builder.Services.UseHeartRateMonitor();
		builder.Services.UseViews();

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

	private static void UseHeartRateMonitor(this IServiceCollection services)
	{
		services.AddSingleton<IHeartRateMonitorScanner, HeartRateMonitorScanner>();
		services.AddSingleton<IHeartRateMonitorClient, HeartRateMonitorClient>();
	}

	private static void UseViews(this IServiceCollection services)
	{
        services.AddTransient<MainPage>();
    }
}
