using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SQLite;

namespace MauiApp2;

public partial class MainPage : ContentPage
{
    public ObservableCollection<BookItem> Books { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        string query = SearchEntry.Text;

        if (string.IsNullOrWhiteSpace(query))
        {
            await DisplayAlert("Ошибка", "Введите название книги", "OK");
            return;
        }

        try
        {
            var httpClient = new HttpClient();
            string url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}";

            var response = await httpClient.GetStringAsync(url);
            var booksResponse = JsonConvert.DeserializeObject<GoogleBooksResponse>(response);

            Books.Clear();

            if (booksResponse?.Items?.Count > 0)
            {
                foreach (var item in booksResponse.Items)
                {
                    var title = item.VolumeInfo?.Title;
                    var thumb = item.VolumeInfo?.ImageLinks?.Thumbnail;

                    if (!string.IsNullOrEmpty(title))
                        Books.Add(new BookItem { Title = title, Thumbnail = thumb });
                }

                BooksCollectionView.IsVisible = true;
                SaveButton.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Результат", "Книги не найдены", "OK");
                BooksCollectionView.IsVisible = false;
                SaveButton.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var selectedBooks = Books.Where(b => b.IsSelected).ToList();

        if (selectedBooks.Count == 0)
        {
            await DisplayAlert("Ошибка", "Выберите хотя бы одну книгу", "OK");
            return;
        }

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        var db = new BookDatabase(dbPath);

        foreach (var book in selectedBooks)
        {
            var saved = new SavedBook
            {
                Title = book.Title,
                Thumbnail = book.Thumbnail
            };

            await db.SaveBookAsync(saved);
        }

        await DisplayAlert("Сохранено", "Книги добавлены в избранное", "OK");
    }

    private async void OnFavoritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritesPage());
    }
}

public class BookItem
{
    public string Title { get; set; }
    public string Thumbnail { get; set; }
    public bool IsSelected { get; set; }
}

public class GoogleBooksResponse
{
    [JsonProperty("items")]
    public List<Item> Items { get; set; }
}

public class Item
{
    [JsonProperty("volumeInfo")]
    public VolumeInfo VolumeInfo { get; set; }
}

public class VolumeInfo
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("imageLinks")]
    public ImageLinks ImageLinks { get; set; }
}

public class ImageLinks
{
    [JsonProperty("thumbnail")]
    public string Thumbnail { get; set; }
}

public class SavedBook
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Thumbnail { get; set; }
}

public class BookDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public BookDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<SavedBook>().Wait();
    }

    public Task<int> SaveBookAsync(SavedBook book)
    {
        return _database.InsertAsync(book);
    }

    public Task<List<SavedBook>> GetBooksAsync()
    {
        return _database.Table<SavedBook>().ToListAsync();
    }

    public Task<int> DeleteBookAsync(SavedBook book)
    {
        return _database.DeleteAsync(book);
    }
}
