using MauiApp2.Views;

namespace MauiApp2;

public partial class App : Application
{
    public static IServiceProvider Services { get; set; }

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Services = serviceProvider;

        MainPage = Services.GetRequiredService<AppShell>();
    }
}