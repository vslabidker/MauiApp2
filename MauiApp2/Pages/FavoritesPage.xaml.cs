using System.Collections.ObjectModel;
using MauiApp2.Database;
using MauiApp2.Models;
using SQLite;

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

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var book = button?.BindingContext as SavedBook;

        if (book == null)
            return;

        bool confirm = await DisplayAlert("Удалить", $"Удалить книгу '{book.Title}' из избранного?", "Да", "Нет");
        if (!confirm)
            return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        var db = new BookDatabase(dbPath);
        await db.DeleteBookAsync(book);

        favoriteBooks.Remove(book);
    }
}
