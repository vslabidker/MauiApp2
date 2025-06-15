using Microsoft.Extensions.Logging;

namespace MauiApp2;

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

        builder.Services.AddSingleton<BookDatabase>(s =>
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
            return new BookDatabase(dbPath);
        });

        return builder.Build();
	}
}
