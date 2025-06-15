using MauiApp2.Models;
using MauiApp2.Data;
using System.Collections.ObjectModel;
using MauiApp2.Pages;

namespace MauiApp2;

public partial class MainPage : ContentPage
{
    public ObservableCollection<BookItem> Books { get; set; } = new();

    private readonly IBookRepository _repository;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        _repository = new BookRepository(new RemoteDataSource(), new LocalDataSource(dbPath));
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        string query = SearchEntry.Text;

        if (string.IsNullOrWhiteSpace(query))
        {
            await DisplayAlert("Ошибка", "Введите название книги", "OK");
            return;
        }

        var result = await _repository.SearchBooksAsync(query);

        Books.Clear();
        foreach (var book in result)
            Books.Add(book);

        BooksCollectionView.IsVisible = Books.Any();
        SaveButton.IsVisible = Books.Any();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var selectedBooks = Books.Where(b => b.IsSelected).ToList();

        if (selectedBooks.Count == 0)
        {
            await DisplayAlert("Ошибка", "Выберите хотя бы одну книгу", "OK");
            return;
        }

        foreach (var book in selectedBooks)
            await _repository.SaveToFavoritesAsync(book);

        await DisplayAlert("Готово", "Книги добавлены в избранное", "OK");
    }

    private async void OnFavoritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritesPage());
    }
}
