using MauiApp2;
using MauiApp2.Data;
using MauiApp2.Database;
using MauiApp2.Pages;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
               .ConfigureFonts(fonts =>
               {
                   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                   fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
               });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");

        builder.Services.AddSingleton(new LocalDataSource(dbPath));
        builder.Services.AddSingleton<RemoteDataSource>();
        builder.Services.AddSingleton<BookRepository>();
        builder.Services.AddSingleton<BookDatabase>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<FavoritesPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
