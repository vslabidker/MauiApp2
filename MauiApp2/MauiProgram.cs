using MauiApp2;
using MauiApp2.Data;
using MauiApp2.Database;
using MauiApp2.ViewModels;
using MauiApp2.Views;
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
        builder.Services.AddSingleton<BookDatabase>(_ => new BookDatabase(dbPath));

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ILocalDataSource>(new LocalDataSource(dbPath));
        builder.Services.AddSingleton<IBookRepository, BookRepository>();
        builder.Services.AddSingleton<IRemoteDataSource, RemoteDataSource>();

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<FavoritesViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<FavoritesPage>();
       
#if DEBUG
        builder.Logging.AddDebug();
#endif

        var mauiApp = builder.Build();

        // Важно: передаём сервисы в App
        App.Services = mauiApp.Services;

        return mauiApp;
    }
}
