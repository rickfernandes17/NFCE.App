using Microsoft.Extensions.Logging;
using NFCEApp.DBContext;
using NFCEApp.Services;

namespace NFCE.App;

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
		builder.Services.AddDbContext<AppDbContext>();
		builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotaFiscalApiService>();

        var dbContext = new AppDbContext();
        dbContext.Database.EnsureCreated();
        dbContext.Dispose();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
