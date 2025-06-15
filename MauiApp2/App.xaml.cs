using MauiApp2;
using MauiApp2.Data;

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
            });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");

        builder.Services.AddSingleton(new LocalDataSource(dbPath));
        builder.Services.AddSingleton<RemoteDataSource>();
        builder.Services.AddSingleton<BookRepository>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<FavoritesPage>();

        return builder.Build();
    }
}
