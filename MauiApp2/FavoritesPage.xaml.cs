using System.Collections.ObjectModel;
using SQLite;

namespace MauiApp2;

public partial class FavoritesPage : ContentPage
{
    public ObservableCollection<SavedBookItem> FavoriteBooks { get; set; } = new();

    private readonly BookDatabase _database;

    public FavoritesPage()
    {
        InitializeComponent();
        BindingContext = this;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        _database = new BookDatabase(dbPath);

        LoadFavorites();
    }

    private async void LoadFavorites()
    {
        var books = await _database.GetBooksAsync();
        FavoriteBooks.Clear();

        foreach (var book in books)
        {
            FavoriteBooks.Add(new SavedBookItem
            {
                Id = book.Id,
                Title = book.Title,
                Thumbnail = book.Thumbnail,
                IsSelected = false
            });
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var selectedBooks = FavoriteBooks.Where(b => b.IsSelected).ToList();

        if (selectedBooks.Count == 0)
        {
            await DisplayAlert("Ошибка", "Выберите книги для удаления", "OK");
            return;
        }

        foreach (var book in selectedBooks)
        {
            await _database.DeleteBookAsync(new SavedBook { Id = book.Id });
            FavoriteBooks.Remove(book);
        }

        await DisplayAlert("Удалено", "Выбранные книги удалены", "OK");
    }
}

public class SavedBookItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Thumbnail { get; set; }
    public bool IsSelected { get; set; }
}
