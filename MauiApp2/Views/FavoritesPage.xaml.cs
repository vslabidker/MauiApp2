using MauiApp2.Models;
using MauiApp2.ViewModels;

namespace MauiApp2.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is FavoritesViewModel vm)
        {
            await vm.LoadFavoritesAsync();
            UpdateOrientation();
        }

        SizeChanged += (_, _) => UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        if (BindingContext is FavoritesViewModel vm)
        {
            bool isLandscape = Width > Height;
            vm.IsLandscape = isLandscape;
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is FavoritesViewModel vm)
        {
            var selected = e.CurrentSelection.ToList();
            vm.SelectedItems = selected;
            vm.SelectedBook = selected.FirstOrDefault() as SavedBook;
        }
    }
}
