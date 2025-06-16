using MauiApp2.Views;

namespace MauiApp2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FavoritesPage), typeof(FavoritesPage));
        }
    }

}
