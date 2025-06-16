using MauiApp2.ViewModels;

namespace MauiApp2.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnFavoritesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavoritesPage));
    }
}
