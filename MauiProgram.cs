using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NFCEApp.DBContext;
using NFCEApp.Services;
using ZXing.Net.Maui.Controls;

namespace NFCE.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddDbContext<AppDbContext>();
		builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotaFiscalApiService>();

        string conexionDb = Path.Combine(FileSystem.AppDataDirectory, "local.db");
		//if (File.Exists(conexionDb))
		//	File.Delete(conexionDb);
		var dbContext = new AppDbContext();
		//dbContext.Database.EnsureCreated();
		dbContext.Dispose();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
