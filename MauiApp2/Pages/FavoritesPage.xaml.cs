using System.Collections.ObjectModel;
using MauiApp2.Database;
using MauiApp2.Models;

namespace MauiApp2.Pages;

public partial class FavoritesPage : ContentPage
{
    private ObservableCollection<SavedBook> favoriteBooks;

    public FavoritesPage()
    {
        InitializeComponent();
        LoadFavorites();
    }

    private async void LoadFavorites()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        var db = new BookDatabase(dbPath);
        var books = await db.GetBooksAsync();
        favoriteBooks = new ObservableCollection<SavedBook>(books);
        FavoritesCollectionView.ItemsSource = favoriteBooks;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DeleteButton.IsVisible = e.CurrentSelection.Any();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var selected = FavoritesCollectionView.SelectedItems.Cast<SavedBook>().ToList();
        if (!selected.Any())
        {
            await DisplayAlert("Внимание", "Выберите книги для удаления", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Удалить", $"Удалить {selected.Count} книг(и) из избранного?", "Да", "Нет");
        if (!confirm) return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        var db = new BookDatabase(dbPath);

        foreach (var book in selected)
        {
            await db.DeleteBookAsync(book);
            favoriteBooks.Remove(book);
        }

        DeleteButton.IsVisible = false;
    }
}
